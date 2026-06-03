using API.Data;
using CareerHub.Data;
using Microsoft.EntityFrameworkCore;
using API.DTOs;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using API.Exceptions;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers;

[ApiController]
[Route("jobs")]
public class JobsController : ControllerBase
{    
    private readonly JobListingDbContext _db;
    public JobsController(JobListingDbContext db)
    {
        _db = db;
    }
    // ── PATTERN A: IActionResult ────────────────────────────────────
    [HttpGet("v-iactionresult")]
    public async Task<IActionResult> GetListings_Untyped()
    {
        return Ok(await _db.JobListings.ToListAsync());
    }

    // ── PATTERN B: ActionResult<T> ────────────────────────────────────
    [HttpGet]
    public async Task<ActionResult<IEnumerable<JobListing>>> GetListingsAsync()
    {
        return Ok(await _db.JobListings.ToListAsync());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<JobListing>> GetListingByIdAsync(Guid id)
    {
        var jobListing = await _db.JobListings.FindAsync(id);

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
        bool isDuplicate = await _db.JobListings.AnyAsync(j => j.Company == request.Company &&j.Title == request.Title);
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
        _db.JobListings.Add(newListing);
        await _db.SaveChangesAsync();

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
        var job = await _db.JobListings.FindAsync(id);

        if (job is null)
        {
            throw new JobNotFoundException(id);
        }

        _db.JobListings.Remove(job);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Employer")]
    public async Task<ActionResult<JobResponse>> UpdateJobAsync(Guid id,[FromBody] CreateJobRequest request)
    {
        var existingListing = await _db.JobListings.FindAsync(id);

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
        await _db.SaveChangesAsync();

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