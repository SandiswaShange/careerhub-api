using API.Models;
public record UpdateJobListingRequest(
    string? Title,
    string? Description,
    string? Location,
    JobType? Type,
    decimal? SalaryMin,
    decimal? SalaryMax,
    DateTime? ClosingDate
);