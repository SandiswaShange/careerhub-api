using API.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("jobs")]
public class JobsController(IJobListingService service) : ControllerBase
{
    private readonly IJobListingService _service = service;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<JobListResponse>>>
        GetListingsAsync()
    {
        var jobs = await _service.GetActiveListingsAsync();

        return Ok(jobs);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<JobDetailResponse>>
        GetListingByIdAsync(Guid id)
    {
        var job = await _service.GetListingAsync(id);

        return Ok(job);
    }

    [HttpPost]
    [Authorize(Roles = "Employer")]
    public async Task<ActionResult<JobResponse>>
        CreateJobAsync([FromBody] CreateJobRequest request)
    {
        var job = await _service.CreateListingAsync(request);

        return CreatedAtAction(
            nameof(GetListingByIdAsync),
            new { id = job.Id },
            job);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Employer")]
    public async Task<ActionResult<JobResponse>>
        UpdateJobAsync(
            Guid id,
            [FromBody] UpdateJobRequest request)
    {
        var job = await _service.UpdateListingAsync(id, request);

        return Ok(job);
    }

    [HttpPatch("{id:guid}/close")]
    [Authorize(Roles = "Employer")]
    public async Task<IActionResult>
        CloseJobAsync(Guid id)
    {
        await _service.CloseListingAsync(id);

        return NoContent();
    }
}