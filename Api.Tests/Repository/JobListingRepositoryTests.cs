using API.Data;
using Microsoft.EntityFrameworkCore;
using API.DTOs;
using API.Models;

namespace API.Tests.Repository;
/*This basically gets me a real PostgreSQL container instead of mocks or the EF in-memory provider.
Allows me to verify database constraints, and other stuff exactly as they behave in production.*/
public class JobListingRepositoryTests : IClassFixture<PostgreSqlContainerFixture>
{
    private readonly PostgreSqlContainerFixture _fixture;

    public JobListingRepositoryTests(PostgreSqlContainerFixture fixture)
    {
        _fixture = fixture;
    }

    private JobListingDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<JobListingDbContext>().UseNpgsql(_fixture.ConnectionString).Options;

        var context = new JobListingDbContext(options);

        context.Database.Migrate();

        return context;
    }

        [Fact]
        public async Task GetActiveListingsPagedAsync_Page1_ReturnsCorrectCount()
        {
            // Arrange
            using var context = CreateContext(); //this is the real difference between Substitute.For<IJobListingRepository>();

            var company = new Company
            {
                Id = Guid.NewGuid(),
                Name = "Microsoft"
            };

            context.Companies.Add(company);

            for (int i = 1; i <= 6; i++)
            {
                context.JobListings.Add(new JobListing
                {
                    Id = Guid.NewGuid(),
                    Title = $"Job {i}",
                    Description = "Test",
                    CompanyId = company.Id,
                    Company = company,
                    Location = "Cape Town",
                    Type = JobType.FullTime,
                    MinSalary = 50000,
                    MaxSalary = 80000,
                    PostedAt = DateTime.UtcNow.AddDays(-i),
                    ClosingDate = DateTime.UtcNow.AddDays(30),
                    IsActive = true
                });
            }

            await context.SaveChangesAsync();

            var repository = new JobListingRepository(context);

            // seed data

        // Act
        var result = await repository.GetActiveListingsPagedAsync(
                Guid.Empty,
                1,
                4,
                new JobListingFilterQuery());

        // Assert
        Assert.Equal(4, result.Data.Count());
        Assert.Equal(6, result.TotalCount);
        Assert.True(result.HasNextPage);
        Assert.False(result.HasPreviousPage);
        }
}