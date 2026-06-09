using API.Models;
using API.DTOs;
public interface IJobListingRepository
{
    Task<IEnumerable<JobListResponse>>GetActiveListingsAsync();
    Task<PagedResponse<JobListResponse>>GetActiveListingsPagedAsync(Guid companyId, int page, int pageSize, JobListingFilterQuery filter);

    Task<JobDetailResponse>GetListingDetailsAsync(Guid listingId);

    Task<JobListing?>GetByIdAsync(Guid listingId);

    Task<bool>ListingExistsAsync(Guid listingId);

    Task<bool>IsOpenForApplicationsAsync(Guid listingId);

    Task AddAsync(JobListing listing);

    Task UpdateAsync(JobListing listing);

    Task CloseAsync(Guid listingId);
    Task<IEnumerable<JobListResponse>> SearchAsync(string searchTerm);
    Task<IEnumerable<JobListingStatsResponse>>GetApplicationStatsAsync(Guid companyId);
}