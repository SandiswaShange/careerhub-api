namespace API.Models;

public class JobListing
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Company { get; set; } = string.Empty;

    public string Location { get; set; } = string.Empty;

    public JobType Type { get; set; }

    public decimal? MinSalary { get; set; }

    public decimal? MaxSalary { get; set; }

    public DateTime PostedAt { get; set; } = DateTime.UtcNow;

    public bool IsActive { get; set; } = true;
}