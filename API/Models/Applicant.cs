namespace API.Models;

public class Applicant
{
    public Guid Id { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public ICollection<Application> Applications { get; set; } //allows EF Core to understand the "many" side of my relationships
        = new List<Application>();
}