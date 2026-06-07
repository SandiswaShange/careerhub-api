using API.DTOs;

public interface IJobListingService
{
    Task<IEnumerable<JobListResponse>>GetActiveListingsAsync();

    Task<JobDetailResponse>GetListingAsync(Guid listingId);

    Task CreateListingAsync(CreateJobRequest dto);

    Task UpdateListingAsync(Guid listingId, UpdateJobRequest dto);

    Task CloseListingAsync(Guid listingId);
}