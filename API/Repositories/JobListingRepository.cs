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
        return await _db.JobListings
        .AsNoTracking()
        .Where(j => j.IsActive)
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

    public async Task UpdateAsync(JobListing listing)
    {
        _db.JobListings.Update(listing);

    await _db.SaveChangesAsync();
    }
}