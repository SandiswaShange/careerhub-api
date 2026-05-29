namespace API.Models;

public record JobListing
(   Guid Id,

    string? Title,

    string? Description,

    string? Company,

    string? Location, 

    string? Type,

    DateTime PostedAt, //Server, at the moment of creation

    bool IsActive //Server — defaults to true on creation
);