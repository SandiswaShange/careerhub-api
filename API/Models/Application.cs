namespace API.Models;

public class Application
{
    public Guid JobListingId { get; set; } //composite key

    public JobListing JobListing { get; set; } = null!;

    public Guid ApplicantId { get; set; } //composite key

    public Applicant Applicant { get; set; } = null!;

    public DateTime SubmittedAt { get; set; }

    public ApplicationStatus Status { get; set; }
}