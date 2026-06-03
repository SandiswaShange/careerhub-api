using API.Models;

namespace API.Data;

public class ListingStore
{
    public static readonly List<JobListing> Jobs =
    [
         new JobListing
         {
            Id = Guid.NewGuid(),
            Title = "Junior Software Developer",
            Description = "Build and maintain backend services.",
            Company = "CareerHub",
            Location = "Johannesburg",
            Type = JobType.FullTime,
            MinSalary = 25000,
            MaxSalary = 40000,
            PostedAt = DateTime.UtcNow,
            IsActive = true 
         },

        new JobListing
        {
            Id = Guid.NewGuid(),
            Title = "Frontend Developer",
            Description = "Develop responsive UI applications.",
            Company = "CareerHub",
            Location = "Cape Town",
            Type = JobType.PartTime,
            MinSalary = 30000,
            MaxSalary = 50000,
            PostedAt = DateTime.UtcNow,
            IsActive = true
        },
        new JobListing
        {
            Id = Guid.NewGuid(),
            Title = "QA Tester",
            Description = "Test software systems and report bugs.",
            Company = "CareerHub",
            Location = "London",
            Type = JobType.Contract,
            MinSalary = 20000,
            MaxSalary = 35000,
            PostedAt = DateTime.UtcNow,
            IsActive = true
        }
    ];

}