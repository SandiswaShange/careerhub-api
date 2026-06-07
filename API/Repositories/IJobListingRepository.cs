using API.Models;
using API.DTOs;
public interface IJobListingRepository
{
    Task<IEnumerable<JobListResponse>>GetActiveListingsAsync();

    Task<JobDetailResponse>GetListingDetailsAsync(Guid listingId);

    Task<JobListing?>GetByIdAsync(Guid listingId);

    Task<bool>ListingExistsAsync(Guid listingId);

    Task<bool>IsOpenForApplicationsAsync(Guid listingId);

    Task<JobListing> AddAsync(JobListing listing);

    Task<JobListing> UpdateAsync(JobListing listing);

    Task<JobListing> CloseAsync(JobListing listingId);
}