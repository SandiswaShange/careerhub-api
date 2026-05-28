using API.Models;

namespace API.Data;

public class ListingStore
{
    public static readonly List<JobListing> Jobs = new()
    {
         new JobListing
        {
            Id = 1,
            Title = "Junior Software Developer",
            Description = "Build and maintain backend services.",
            Company = "CareerHub",
            Location = "Johannesburg",
            Type = JobType.FullTime,
            SalaryMin = 25000,
            SalaryMax = 40000,
            PostedAt = DateTime.UtcNow,
            IsActive = true
        },

        new JobListing
        {
            Id = 2,
            Title = "Frontend Developer",
            Description = "Develop responsive UI applications.",
            Company = "CareerHub",
            Location = "Cape Town",
            Type = JobType.PartTime,
            SalaryMin = 18000,
            SalaryMax = 30000,
            PostedAt = DateTime.UtcNow,
            IsActive = true
        },

        new JobListing
        {
            Id = 3,
            Title = "QA Tester",
            Description = "Test software systems and report bugs.",
            Company = "CareerHub",
            Location = "London",
            Type = JobType.Contract,
            SalaryMin = 20000,
            SalaryMax = 35000,
            PostedAt = DateTime.UtcNow,
            IsActive = true
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