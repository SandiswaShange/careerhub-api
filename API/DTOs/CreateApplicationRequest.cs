using API.Models;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public record CreateApplicationRequest(
    [Required]
    Guid ApplicantId,

    [Required]
    Guid JobListingId,
    string HearAboutRole
);