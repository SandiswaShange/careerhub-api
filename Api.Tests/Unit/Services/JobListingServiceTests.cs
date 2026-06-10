using API.Data;
using API.DTOs;
using API.Models;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using API.Services;
using API.Exceptions;
using Microsoft.IdentityModel.Tokens;

namespace API.Tests.Unit.Services;

public class JobListingServiceTests{
    private readonly IJobListingRepository _jobRepo;
    private readonly ICompanyRepository _companyRepo;
    private readonly JobListingService _sut; // this is what's under test

    public JobListingServiceTests()
    {
        _jobRepo = Substitute.For<IJobListingRepository>();
        _companyRepo = Substitute.For<ICompanyRepository>();
        _sut = new JobListingService(_jobRepo, _companyRepo);
    }

    [Fact]
    public async Task CreateAsync_WhenSalaryMaxLessThanSalaryMin_ThrowsInvalidSalaryException()
                    //methodname_scenario_result
    {
        // Arrange
            var company = new Company
            {
                Id = Guid.NewGuid(),
                Name = "Bitcube"
            };

            _companyRepo.GetByNameAsync("Bitcube").Returns(company);

            var request = new CreateJobRequest(
                Title: "Backend Developer",
                Description: "API work",
                Company: "Bitcube",
                Location: "Johannesburg",
                Type: JobType.FullTime,
                SalaryMin: 80000,
                SalaryMax: 50000,
                ClosingDate: DateTime.UtcNow.AddDays(30)
            );

            // Act
            var act = () => _sut.CreateListingAsync(request);

            // Assert
            await Assert.ThrowsAnyAsync<ArgumentException>(act);

            await _jobRepo.DidNotReceive().AddAsync(Arg.Any<JobListing>());
    }
    
    [Fact]
    public async Task CreateAsync_WhenExpiresAtIsInThePast_ThrowsInvalidListingException()
    {
            // Arrange
        var company = new Company
        {
            Id = Guid.NewGuid(),
             Name = "Bitcube"
        };

            _companyRepo.GetByNameAsync("Bitcube").Returns(company);

            var request = new CreateJobRequest(
                Title: "Backend Developer",
                Description: "API work",
                Company: "Bitcube",
                Location: "Johannesburg",
                Type: JobType.FullTime,
                SalaryMin: 50000,
                SalaryMax: 80000,
                ClosingDate: DateTime.UtcNow.AddDays(-1)
            );

            // Act
            var act = () => _sut.CreateListingAsync(request);

            // Assert
            await Assert.ThrowsAnyAsync<ArgumentException>(act);

            await _jobRepo.DidNotReceive().AddAsync(Arg.Any<JobListing>());
        }    
    
        [Fact]
        public async Task CreateAsync_WhenValid_CallsAddAsyncExactlyOnce()
        {
            // Arrange
            var company = new Company
            {
                Id = Guid.NewGuid(),
                Name = "Bitcube"
            };

            _companyRepo.GetByNameAsync("Bitcube").Returns(company);

            var request = new CreateJobRequest(
                Title: "Backend Developer",
                Description: "API work",
                Company: "Bitcube",
                Location: "Johannesburg",
                Type: JobType.FullTime,
                SalaryMin: 50000,
                SalaryMax: 80000,
                ClosingDate: DateTime.UtcNow.AddDays(30)
            );

            // Act
            await _sut.CreateListingAsync(request);

            // Assert
            await _jobRepo.Received(1).AddAsync(Arg.Any<JobListing>());
        }
    

    [Fact]
    public async Task PatchAsync_WhenOnlySalaryMinChanged_CallsValidation()
    {
        // Arrange
        var listingId = Guid.NewGuid();

        var request = new UpdateJobListingRequest(
            null,
            null,
            null,
            null,
            90000,
            null,
            null
        );

        _jobRepo.PatchAsync(listingId, request)
            .Returns(Task.FromException<JobResponse>(
                new ArgumentException(
                    "Minimum salary cannot exceed maximum salary.")));

        // Act
        var act = () => _sut.PatchAsync(listingId, request);

        // Assert
        await Assert.ThrowsAnyAsync<ArgumentException>(act);
    }

    [Fact]
    public async Task PatchAsync_WhenListingNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var listingId = Guid.NewGuid();

        var request = new UpdateJobListingRequest(
        Title: "Updated Title",
        Description: null,
        Location: null,
        Type: null,
        SalaryMin: null,
        SalaryMax: null,
        ClosingDate: null
        );

        _jobRepo.PatchAsync(listingId, request)
            .Returns(Task.FromException<JobResponse>(
                new JobNotFoundException(listingId)));

        // Act
        var act = () => _sut.PatchAsync(
            listingId,
            request);

        // Assert
        await Assert.ThrowsAnyAsync<JobNotFoundException>(act);
    }
}

