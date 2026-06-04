namespace API.Models;

public class Company
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public ICollection<JobListing> JobListings { get; set; } //allows EF Core to understand the "many" side of my relationships
        = new List<JobListing>();
}