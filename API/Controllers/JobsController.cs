using API.Data;
using API.DTOs;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("jobs")]
public class JobsController : ControllerBase
{    // ── PATTERN A: IActionResult ────────────────────────────────────
    [HttpGet("v-iactionresult")]
    public async Task<IActionResult> GetListings_Untyped()
    {
        await Task.Delay(100);
        return Ok(ListingStore.Jobs);
    }

    // ── PATTERN B: ActionResult<T> ────────────────────────────────────
    [HttpGet]
    public async Task<ActionResult<IEnumerable<JobListing>>> GetListingsAsync()
    {
        await Task.Delay(200);
        return Ok(ListingStore.Jobs);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<JobListing>> GetListingByIdAsync(Guid id)
    {
        await Task.Delay(50);

        var jobListing = ListingStore.Jobs.FirstOrDefault(j => j.Id == id);

        if (jobListing is null)
        {
            return NotFound();
        }

        return Ok(jobListing);
    }
//=================================================================================================================================
    [HttpPost]
    public async Task<ActionResult<JobResponse>> CreateBookingAsync(CreateJobRequest request)
    {
        await Task.Delay(51);

        bool isDuplicate = ListingStore.Jobs.Any
        (j => j.Company == request.Company && j.Location == request.Location);
        if (isDuplicate)
        {
            return Conflict("A job listing already exists for this company at the specified location."); // HTTP 409 Conflict
        }

        var newListing = new JobListing(
            Guid.NewGuid(),
            request.Title,
            request.Description,
            request.Company,
            request.Location,
            request.Type,
            request.SalaryMin,
            request.SalaryMax,
            DateTime.UtcNow, // PostedAt is set by the server
            true
        );

        //3. Save the booking
        ListingStore.Jobs.Add(newListing);

        //4. Map domain Model to Response DTO
        var response = new JobResponse(
            newListing.Id,
            newListing.Title!,
            newListing.Description!,
            newListing.Company!,
            newListing.Location!,
            newListing.Type,
            newListing.PostedAt,
            newListing.IsActive,
            newListing.SalaryMin!.Value,
            newListing.SalaryMax!.Value,
            GetSalaryDisplay(newListing)
        );

        return CreatedAtAction(nameof(GetListingByIdAsync), new { id = response.Id }, response);
    }

 // DELETE: /api/jobs/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteJobAsync(Guid id )
    {
        await Task.Delay(51); //will replace with an actual database call

        var job = ListingStore.Jobs.FirstOrDefault(j => j.Id == id);

        if (job is null)
        {
            return NoContent(); // HTTP 204 No Content
        }

        ListingStore.Jobs.Remove(job);
        return NoContent(); // HTTP 204 No Content — indicates success but no body is returned
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<JobResponse>> UpdateJobAsync(Guid id,[FromBody] CreateJobRequest request)
    {
        await Task.Delay(51);

        var existingListing = ListingStore.Jobs.FirstOrDefault(b => b.Id == id);
        
        if (existingListing == null)
        {
            return Conflict("A job listing already exists for this company at the specified location."); // HTTP 409 Conflict
        }

        var updatedListing = existingListing with
        {
            Title = request.Title,
            Description = request.Description,
            Company = request.Company,
            Location = request.Location,
            Type = request.Type,
            SalaryMin = request.SalaryMin,
            SalaryMax = request.SalaryMax
        };

        //3. Save the job listing
        ListingStore.Jobs.Remove(existingListing);
        ListingStore.Jobs.Add(updatedListing);

        //4. Map domain Model to Response DTO
        var response = new JobResponse(
            updatedListing.Id,
            updatedListing.Title!,
            updatedListing.Description!,
            updatedListing.Company!,
            updatedListing.Location!,
            updatedListing.Type,
            updatedListing.PostedAt,
            updatedListing.IsActive,
            updatedListing.SalaryMin!.Value,
            updatedListing.SalaryMax!.Value,
            GetSalaryDisplay(updatedListing)
        );
        
        //5. Return the 200 Created = Location header
        return Ok(response);
    }
    
    
 //=================================================================================================================================   
    private static string GetSalaryDisplay(JobListing job)
    {
    if (job.SalaryMin.HasValue && job.SalaryMax.HasValue)
    {
        return $"R{job.SalaryMin} - R{job.SalaryMax}/month";
    }

    if (job.SalaryMin.HasValue)
    {
        return $"From R{job.SalaryMin}/month";
    }

    return "Salary not specified";
    }

}