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
            "Full-Time",
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
            "Remote",
            DateTime.UtcNow,
            true
        ),

        new JobListing
        (   Guid.NewGuid(),
            "QA Tester",
            "Test software systems and report bugs.",
            "CareerHub",
            "London",
            "Contract",
            DateTime.UtcNow,
            true
        )
    ];

}