using API.Models;
public record JobDetailResponse(
    Guid Id,
    string Title,
    string Description,
    string Company,
    string Location,
    JobType Type,
    DateTime PostedAt,
    decimal? MinSalary,
    decimal? MaxSalary,
    IEnumerable<ApplicationResponse> Applications
);