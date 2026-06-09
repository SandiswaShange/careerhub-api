namespace API.DTOs;

public record JobListingStatsResponse(
    Guid ListingId,
    string Title,
    int SubmittedCount,
    int UnderReviewCount,
    int ShortlistedCount,
    int RejectedCount,
    int OfferedCount,
    int TotalApplications,
    int Rank
);