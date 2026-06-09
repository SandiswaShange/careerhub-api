using API.DTOs;
using API.Models;
using API.Data;
using Microsoft.EntityFrameworkCore;
using API.Exceptions;
public class JobListingRepository(JobListingDbContext db) : IJobListingRepository
{
    private readonly JobListingDbContext _db = db;

    public async Task AddAsync(JobListing listing)
    {
       _db.JobListings.Add(listing);

    await _db.SaveChangesAsync();
    }

    public async Task CloseAsync(Guid listingId)
    {
         var listing =
        await _db.JobListings.FindAsync(listingId);

    if (listing is null)
    {
        throw new JobNotFoundException(listingId);
    }

    listing.IsActive = false;

    await _db.SaveChangesAsync();
    }

    public async Task<IEnumerable<JobListResponse>> GetActiveListingsAsync()
    {
      return await _activeListingsQuery(_db).ToListAsync();   
    }

    public async Task<JobListing?> GetByIdAsync(Guid listingId)
    {
          return await _db.JobListings
        .FirstOrDefaultAsync(j => j.Id == listingId);
    }

    public async Task<JobDetailResponse> GetListingDetailsAsync(Guid listingId)
    {
        var job = await _db.JobListings
        .AsNoTracking()
        .Where(j => j.Id == listingId)
        .Select(j => new JobDetailResponse(
            j.Id,
            j.Title,
            j.Description,
            j.Company.Name,
            j.Location,
            j.Type,
            j.PostedAt,
            j.Applications.Select(a =>
                new ApplicationResponse(
                    a.Applicant.FirstName + " " +
                    a.Applicant.LastName,
                    a.SubmittedAt,
                    a.Status.ToString()
                ))
        ))
        .SingleOrDefaultAsync();

    if (job is null)
    {
        throw new JobNotFoundException(listingId);
    }

    return job;
    }

    public async Task<bool> IsOpenForApplicationsAsync(Guid listingId)
    {
         return await _db.JobListings
        .AnyAsync(j =>
            j.Id == listingId &&
            j.IsActive &&
            j.ClosingDate > DateTime.UtcNow);
    }

    public async Task<bool> ListingExistsAsync(Guid listingId)
    {
        return await _db.JobListings
        .AnyAsync(j => j.Id == listingId);
    }

    public async Task<IEnumerable<JobListResponse>> SearchAsync(string searchTerm)
    {
        return await _db.JobListings
        .AsNoTracking()
        .Where(j =>
            j.IsActive &&
            j.ClosingDate > DateTime.UtcNow)
        .Where(j =>
            EF.Functions.ToTsVector(
                "english",
                j.Title + " " + j.Description)
            .Matches(
                EF.Functions.PlainToTsQuery(
                    "english",
                    searchTerm)))
        .Select(j => new JobListResponse(
            j.Id,
            j.Title,
            j.Company.Name,
            j.Location,
            j.Type,
            j.Applications.Count()
        ))
        .ToListAsync();
    }

    public async Task UpdateAsync(JobListing listing)
    {
        _db.JobListings.Update(listing);

    await _db.SaveChangesAsync();
    }

    public async Task<IEnumerable<JobListingStatsResponse>> GetApplicationStatsAsync(Guid companyId)
    {
       FormattableString sql = $$"""
        SELECT
        jl."Id" AS "ListingId",
        jl."Title" AS "Title",

        COUNT(*) FILTER (WHERE a."Status" = 'Submitted') AS "SubmittedCount",
        COUNT(*) FILTER (WHERE a."Status" = 'UnderReview') AS "UnderReviewCount",
        COUNT(*) FILTER (WHERE a."Status" = 'Shortlisted') AS "ShortlistedCount",
        COUNT(*) FILTER (WHERE a."Status" = 'Rejected') AS "RejectedCount",
        COUNT(*) FILTER (WHERE a."Status" = 'Offered') AS "OfferedCount",

        COUNT(a."Id") AS "TotalApplications",

        RANK() OVER (ORDER BY COUNT(a."Id") DESC) AS "Rank"

        FROM "JobListings" jl
        LEFT JOIN "Applications" a
        ON a."JobListingId" = jl."Id"

        WHERE jl."CompanyId" = {companyId}
        AND jl."IsActive" = true

        GROUP BY jl."Id", jl."Title";
        """;

    return await _db.Database.SqlQuery<JobListingStatsResponse>(sql).ToListAsync();
    }

    //=================================================================================================================================
    private static readonly Func<
    JobListingDbContext,
    IAsyncEnumerable<JobListResponse>>
    _activeListingsQuery =
        EF.CompileAsyncQuery(
            (JobListingDbContext db) =>
                db.JobListings
                    .AsNoTracking()
                    .Where(j =>
                        j.IsActive &&
                        j.ClosingDate > DateTime.UtcNow)
                    .Select(j =>
                        new JobListResponse(
                            j.Id,
                            j.Title,
                            j.Company.Name,
                            j.Location,
                            j.Type,
                            j.Applications.Count())));
}