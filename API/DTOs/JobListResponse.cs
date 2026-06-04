using API.Models;

namespace API.DTOs;

public record JobListResponse(
    Guid Id,
    string Title,
    string Company,
    string Location,
    JobType Type,
    int ApplicationCount
);