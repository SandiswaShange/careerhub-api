using System.ComponentModel.DataAnnotations;
using API.Models;

namespace API.DTOs;

//Request DTO is what the client sends to the API/Backend to create a booking

public record CreateJobRequest
(
    [Required]
    [StringLength(120, MinimumLength = 5)]
    string Title,

    [Required]
    [StringLength(80, MinimumLength = 2)]
    string Company,

    [Required]
    string Location,

    [Required]
    [MinLength(20)]
    string Description,

    [Required]
    JobType Type,

    [Range(1, int.MaxValue)]
    int? SalaryMin,

    [Range(1, int.MaxValue)]
    int? SalaryMax,
    
    [Required]
    DateTime ClosingDate
): IValidatableObject
{
    public IEnumerable<ValidationResult> Validate(
        ValidationContext validationContext)
    {
        if (SalaryMin.HasValue &&
            SalaryMax.HasValue &&
            SalaryMax <= SalaryMin)
        {
            yield return new ValidationResult(
                "SalaryMax must be greater than SalaryMin.",
                new[] { nameof(SalaryMax) });
        }
    }
}