using API.DTOs;

public interface IJobListingService
{
    Task<IEnumerable<JobListResponse>>GetActiveListingsAsync();
    Task<PagedResponse<JobListResponse>>GetActiveListingsPagedAsync(Guid companyId, int page, int pageSize, JobListingFilterQuery filter);
    Task<JobDetailResponse>GetListingAsync(Guid listingId);

    Task<JobResponse> CreateListingAsync(CreateJobRequest dto);

    Task<JobResponse> UpdateListingAsync(Guid listingId, UpdateJobRequest dto);

    Task CloseListingAsync(Guid listingId);
    Task<JobResponse> PatchAsync(Guid listingId, UpdateJobListingRequest request);
}