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

    DateTime PostedAt, //Server, at the moment of creation
    
    bool IsActive, //Server — defaults to true on creation

    decimal SalaryMin,

    decimal SalaryMax,
     
    string SalaryDisplay
);