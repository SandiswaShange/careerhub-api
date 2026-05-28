using System.ComponentModel.DataAnnotations;

namespace API.Models;

public record JobListing
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? Company { get; set; }

    public string? Location { get; set; } 

    public DateTime PostedAt {get; set; }  //Server, at the moment of creation
    public bool IsActive { get; set; } = true; //Server — defaults to true on creation

    public JobType Type { get; set; }

    public decimal? SalaryMin { get; set; }

    public decimal? SalaryMax { get; set; }
}