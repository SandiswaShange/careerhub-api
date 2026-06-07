using API.Data;
using API.DTOs;
using API.Models;
using Microsoft.EntityFrameworkCore;

public class ApplicationRepository(JobListingDbContext db): IApplicationRepository
{
    private readonly JobListingDbContext _db = db;

    public async Task AddAsync(Application application)
    {
         _db.Applications.Add(application);

        await _db.SaveChangesAsync();
    }

    public async Task<IEnumerable<ApplicationResponse>> GetApplicationsByApplicantAsync(Guid applicantId)
    {
        return await _db.Applications
        .AsNoTracking()
        .Where(a => a.ApplicantId == applicantId)
        .Select(a => new ApplicationResponse(
            a.Applicant.FirstName + " " + a.Applicant.LastName,
            a.SubmittedAt,
            a.Status.ToString()
        )).ToListAsync();
    }

    public async Task<IEnumerable<ApplicationResponse>> GetApplicationsForListingAsync(Guid listingId)
    {
        return await _db.Applications
        .AsNoTracking()
        .Where(a => a.JobListingId == listingId)
        .Select(a => new ApplicationResponse(
            a.Applicant.FirstName + " " + a.Applicant.LastName,
            a.SubmittedAt,
            a.Status.ToString()
        )).ToListAsync();
    }

    public async Task<Application?> GetByIdAsync(Guid applicantId, Guid listingId)
    {
        return await _db.Applications
        .FirstOrDefaultAsync(a =>
            a.ApplicantId == applicantId &&
            a.JobListingId == listingId);
    }

    public async Task<bool> HasApplicantAppliedAsync(Guid applicantId, Guid listingId)
    {
         return await _db.Applications.AnyAsync(a =>
        a.ApplicantId == applicantId &&
        a.JobListingId == listingId);
    }

    public async Task RemoveAsync(Application application)
    {
        _db.Applications.Remove(application);

        await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync(Application application)
    {
        _db.Applications.Update(application);

        await _db.SaveChangesAsync();
    }
}