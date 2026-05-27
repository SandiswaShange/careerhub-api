using API.Models;

namespace API.Data;

public class ListingStore
{
    public static readonly List<JobListing> Jobs = new()
    {
         new JobListing
        {
            Id = 001,
            Title = "Junior Software Developer",
            Description = "Build and maintain backend services.",
            Company = "CareerHub",
            Location = "Johannesburg",
            Type = "Full-Time"
        },

        new JobListing
        {
            Id = 002,
            Title = "Frontend Developer",
            Description = "Develop responsive UI applications.",
            Company = "CareerHub",
            Location = "Cape Town",
            Type = "Remote"
        },

        new JobListing
        {
            Id = 003,
            Title = "QA Tester",
            Description = "Test software systems and report bugs.",
            Company = "CareerHub",
            Location = "London",
            Type = "Contract"
        }
    };

    public Task<List<JobListing>> GetAllJobsAsync()
    {
        return Task.FromResult(Jobs);
    }

    public Task<JobListing?> GetJobByIdAsync(int id)
    {
        var job = Jobs.FirstOrDefault(j => j.Id == id);

        return Task.FromResult(job);
    }
    
}