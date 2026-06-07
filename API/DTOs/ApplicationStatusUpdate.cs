using API.Models;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public record UpdateApplicationStatusRequest(
    [Required]
    Guid ApplicantId,

    [Required]
    Guid JobListingId,

    [Required]
    ApplicationStatus NewStatus
);