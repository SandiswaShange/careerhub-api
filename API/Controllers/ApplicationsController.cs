using API.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
[ApiController]
[Route("applications")]
public class ApplicationsController(IApplicationService service): ControllerBase
{
    private readonly IApplicationService _service = service;

    [HttpPost]
    public async Task<IActionResult> SubmitApplicationAsync(
        [FromBody] CreateApplicationRequest request)
    {
        await _service.SubmitApplicationAsync(request);

        return Created();
    }

    [HttpGet("listing/{listingId:guid}")]
    public async Task<ActionResult<
        IEnumerable<ApplicationResponse>>>
        GetApplicationsForListingAsync(Guid listingId)
    {
        var applications =
            await _service.GetApplicationsForListingAsync(
                listingId);

        return Ok(applications);
    }

    [HttpGet("applicant/{applicantId:guid}")]
    public async Task<ActionResult<
        IEnumerable<ApplicationResponse>>>
        GetApplicationsByApplicantAsync(
            Guid applicantId)
    {
        var applications =
            await _service.GetApplicationsByApplicantAsync(
                applicantId);
        return Ok(applications);
    }

    [HttpPatch("{applicantId:guid}/{listingId:guid}/status")]
    [Authorize(Roles = "Employer")]
    public async Task<IActionResult> UpdateStatusAsync(
        Guid applicantId,
        Guid listingId,
        [FromBody] UpdateApplicationStatusRequest request)
    {
        await _service.UpdateStatusAsync(
            applicantId,
            listingId,
            request.Status);
        return Ok();
    }
}