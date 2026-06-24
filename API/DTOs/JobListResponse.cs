using API.Models;

namespace API.DTOs;

public record JobListResponse(
    Guid Id,
    string Title,
    string Company,
    string Location,
    decimal? MinSalary,
    decimal? MaxSalary,
    DateTime PostedAt,
    bool isActive,
    JobType Type,
    int ApplicationCount
);