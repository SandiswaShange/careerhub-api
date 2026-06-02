namespace API.Models;

public record JobListing
{   public Guid Id { get; set; }
    public string? Title { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public string? Company { get; set; } = string.Empty;
    public string? Location { get; set; } = string.Empty;
    public JobType Type { get; set; }
    public int? SalaryMin { get; set; }
    public int? SalaryMax { get; set; }
    public DateTime PostedAt { get; set; }
    public bool IsActive { get; set; }
}