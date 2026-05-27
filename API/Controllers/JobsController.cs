using API.Data;
using Microsoft.AspNetCore.Mvc;
//Use IActionResult Because the method may return different HTTP responses

namespace API.Controllers;

[ApiController] //Marks this class as an API controller
[Route("jobs")] //Defines the base route
public class JobsController : ControllerBase
{
    private readonly ListingStore _ListingStore;

    public JobsController(ListingStore listingStore)
    {
        _ListingStore = listingStore;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllJobs()
    {
        var jobs = await _ListingStore.GetAllJobsAsync();

        return Ok(jobs);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetJobById(int id)
    {
        var job = await _ListingStore.GetJobByIdAsync(id);

        if (job == null)
        {
            return NotFound();
        }

        return Ok(job);
    }
}