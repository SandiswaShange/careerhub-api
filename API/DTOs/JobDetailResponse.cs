using API.Models;
public record JobDetailResponse(
    Guid Id,
    string Title,
    string Description,
    string Company,
    string Location,
    JobType Type,
    DateTime PostedAt,
    IEnumerable<ApplicationResponse> Applications
);