using API.DTOs;

public interface IJobListingService
{
    Task<IEnumerable<JobListResponse>>GetActiveListingsAsync();

    Task<JobDetailResponse>GetListingAsync(Guid listingId);

    Task<JobResponse> CreateListingAsync(CreateJobRequest dto);

    Task<JobResponse> UpdateListingAsync(Guid listingId, UpdateJobRequest dto);

    Task CloseListingAsync(Guid listingId);
    Task<IEnumerable<JobListResponse>> SearchAsync(string searchTerm);
Task<IEnumerable<JobListingStatsResponse>> GetApplicationStatsAsync(Guid companyId);}