using API.Data;
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
//===================================================================================================================================
}