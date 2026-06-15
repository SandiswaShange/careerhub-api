using API.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using Microsoft.AspNetCore.RateLimiting;
using API.Models;

namespace API.Controllers;

[ApiController]
[ApiVersion(1.0)]
[Route("api/v{version:apiVersion}/jobs")]
public class JobsController(IJobListingService service) : ControllerBase
{
    private readonly IJobListingService _service = service;

    [EndpointSummary("List all available jobs")]
    [EndpointDescription(
        "Returns a paginated list of all job listings. " +
        "The X-Total-Count response header contains the total number of listings matching the current filter")]
    //[HttpGet("company/{companyId:guid}")] edited
    [HttpGet]
    public async Task<ActionResult<PagedResponse<JobListResponse>>>
    GetCompanyListingsAsync(
        Guid companyId,
        JobListingFilterQuery filter,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20
        )
    {
    var result = await _service.GetActiveListingsPagedAsync(companyId,page,pageSize, filter);

    Response.Headers.Append("X-Total-Count",result.TotalCount.ToString());

    return Ok(result);
    }

    [EndpointSummary("Get a listing by ID")]
    [EndpointDescription(
        "Shows a booking only based on the ID. Uses an etag to let uses know if hte job was modified since their last request.")]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<JobDetailResponse>> GetListingByIdAsync(Guid id)
    {
        var job = await _service.GetListingAsync(id);

        var etag = $"\"{job.Id}-{job.PostedAt.Ticks}-{job.MinSalary}\"";

    if (Request.Headers.IfNoneMatch == etag)
    {
        return StatusCode(StatusCodes.Status304NotModified);
    }

    Response.Headers.ETag = etag;

        return Ok(job);
    }

    [HttpPost]
    [Authorize(Roles = "Employer")]
    public async Task<ActionResult<JobResponse>> CreateJobAsync([FromBody] CreateJobRequest request)
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
    public async Task<IActionResult> CloseJobAsync(Guid id)
    {
        await _service.CloseListingAsync(id);

        return NoContent();
    }
    public async Task<JobResponse> PatchAsync(Guid listingId, UpdateJobListingRequest request)
    {
        return await _service.PatchAsync(listingId, request);
    }
    
    [EndpointSummary("Patches a listing")]
    [EndpointDescription("Only updates specific parts of a listing, not the whole thing. It calls the service to apply the patch, and returns the JobResponse value. ")]
    [HttpPatch("{id:guid}")]
    [Authorize(Roles = "Employer")]
    public async Task<ActionResult<JobResponse>>PatchJobAsync(Guid id,
        [FromBody] UpdateJobListingRequest request)
    {
        return Ok(
            await _service.PatchAsync(id, request));
    }
}