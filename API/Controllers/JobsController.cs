using API.Data;
using API.DTOs;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using API.Exceptions;
using Microsoft.AspNetCore.Authorization;

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
            throw new JobNotFoundException(id);
        }

        return Ok(jobListing);
    }
//=================================================================================================================================
    [HttpPost]
    [Authorize(Roles = "Employer")]
    public async Task<ActionResult<JobResponse>> CreateBookingAsync(CreateJobRequest request)
    {
        await Task.Delay(51);

        bool isDuplicate = ListingStore.Jobs.Any
        (j => j.Company == request.Company && j.Title == request.Title);
        if (isDuplicate)
        {
            throw new DuplicateJobListingException(request.Company, request.Title);
        }

        var newListing = new JobListing
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            Company = request.Company,
            Location = request.Location,
            Type = request.Type,
            MinSalary = request.SalaryMin,
            MaxSalary = request.SalaryMax,
            PostedAt = DateTime.UtcNow, // PostedAt is set by the server
            IsActive = true
        };

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
            newListing.MinSalary!.Value,
            newListing.MaxSalary!.Value,
            GetSalaryDisplay(newListing)
        );

        return CreatedAtAction(nameof(GetListingByIdAsync), new { id = response.Id }, response);
    }

 // DELETE: /api/jobs/{id}
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Employer")]
    public async Task<IActionResult> DeleteJobAsync(Guid id )
    {
        await Task.Delay(51); //will replace with an actual database call

        var job = ListingStore.Jobs.FirstOrDefault(j => j.Id == id);

        if (job is null)
        {
            throw new JobNotFoundException(id);
        }

        ListingStore.Jobs.Remove(job);
        return NoContent(); // HTTP 204 No Content — indicates success but no body is returned
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Employer")]
    public async Task<ActionResult<JobResponse>> UpdateJobAsync(Guid id,[FromBody] CreateJobRequest request)
    {
        await Task.Delay(51);

        var existingListing = ListingStore.Jobs.FirstOrDefault(b => b.Id == id);
        
        if (existingListing == null)
        {
            throw new JobNotFoundException(id);
        }

        existingListing.Title = request.Title;
        existingListing.Description = request.Description;
        existingListing.Company = request.Company;
        existingListing.Location = request.Location;
        existingListing.Type = request.Type;
        existingListing.MinSalary = request.SalaryMin;
        existingListing.MaxSalary = request.SalaryMax;

        var response = new JobResponse(
            existingListing.Id,
            existingListing.Title!,
            existingListing.Description!,
            existingListing.Company!,
            existingListing.Location!,
            existingListing.Type,
            existingListing.PostedAt,
            existingListing.IsActive,
            existingListing.MinSalary!.Value,
            existingListing.MaxSalary!.Value,
            GetSalaryDisplay(existingListing)
        );
        
        //5. Return the 200 Created = Location header
        return Ok(response);
    }
    
    
 //=================================================================================================================================   
    private static string GetSalaryDisplay(JobListing job)
    {
    if (job.MinSalary.HasValue && job.MaxSalary.HasValue)
    {
        return $"R{job.MinSalary} - R{job.MaxSalary}/month";
    }

    if (job.MinSalary.HasValue)
    {
        return $"From R{job.MinSalary}/month";
    }

    return "Salary not specified";
    }

}