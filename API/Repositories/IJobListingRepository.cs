using API.Models;
using API.DTOs;
public interface IJobListingRepository
{
    Task<IEnumerable<JobListResponse>>GetActiveListingsAsync();

    Task<JobDetailResponse>GetListingDetailsAsync(Guid listingId);

    Task<JobListing?>GetByIdAsync(Guid listingId);

    Task<bool>ListingExistsAsync(Guid listingId);

    Task<bool>IsOpenForApplicationsAsync(Guid listingId);

    Task AddAsync(JobListing listing);

    Task UpdateAsync(JobListing listing);

    Task CloseAsync(Guid listingId);
}