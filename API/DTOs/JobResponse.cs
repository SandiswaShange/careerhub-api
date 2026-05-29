using API.Models;

namespace API.DTOs;

public record JobResponse 
(
    Guid Id,

    string Title,

    string Description,

     string Company,

    string Location,
    
    JobType Type,

   
    bool IsActive, //Server — defaults to true on creation

    decimal? SalaryMin,

    decimal? SalaryMax,
    DateTime PostedAt, 
    string SalaryDisplay //Server, at the moment of creation
);