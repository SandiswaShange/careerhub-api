using API.Models;

namespace API.Data;

public class ListingStore
{
    public static readonly List<JobListing> Jobs =
    [
         new JobListing
        (   Guid.NewGuid(),
            "Junior Software Developer",
            "Build and maintain backend services.",
            "CareerHub",
            "Johannesburg",
            JobType.FullTime,
            25000,
            40000,
            DateTime.UtcNow,
            true
        ),

        new JobListing
        (   
            Guid.NewGuid(),
            "Frontend Developer",
            "Develop responsive UI applications.",
            "CareerHub",
            "Cape Town",
            JobType.PartTime,
            30000,
            50000,
            DateTime.UtcNow,
            true
        ),

        new JobListing
        (   Guid.NewGuid(),
            "QA Tester",
            "Test software systems and report bugs.",
            "CareerHub",
            "London",
            JobType.Contract,
            20000,
            35000,
            DateTime.UtcNow,
            true
        )
    ];

}