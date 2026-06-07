using API.DTOs;
using API.Models;
using API.Data;
public class JobListingRepository(JobListingDbContext db) : IJobListingRepository
{
    private readonly JobListingDbContext? _db = db;
    public Task AddAsync(JobListing listing)
    {
        throw new NotImplementedException();
    }

    public Task<JobListing> CloseAsync(JobListing listingId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<JobListResponse>> GetActiveListingsAsync()
    {
        throw new NotImplementedException();
    }

    public Task<JobListing?> GetByIdAsync(Guid listingId)
    {
        throw new NotImplementedException();
    }

    public Task<JobDetailResponse> GetListingDetailsAsync(Guid listingId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsOpenForApplicationsAsync(Guid listingId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ListingExistsAsync(Guid listingId)
    {
        throw new NotImplementedException();
    }

    public Task<JobListing> UpdateAsync(JobListing listing)
    {
        throw new NotImplementedException();
    }

    Task<JobListing> IJobListingRepository.AddAsync(JobListing listing)
    {
        throw new NotImplementedException();
    }
}