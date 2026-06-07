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

    [HttpDelete("{listingId:guid}/{applicantId:guid}")]
    public async Task<IActionResult>
        WithdrawApplicationAsync(
            Guid listingId,
            Guid applicantId)
    {
        await _service.WithdrawApplicationAsync(
            applicantId,
            listingId,
            applicantId);

        return NoContent();
}
}