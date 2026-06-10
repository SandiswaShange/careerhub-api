using API.Data;
using API.DTOs;
using API.Models;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using API.Services;
using API.Exceptions;
using Microsoft.IdentityModel.Tokens;

namespace API.Tests.Unit.Services;

public class JobListingRepositoryTests{
    private readonly IJobListingRepository _jobRepo;
    private readonly ICompanyRepository _companyRepo;
    private readonly JobListingService _sut; // this is what's under test

    private JobListingRepositoryTests()
    {
        _jobRepo = Substitute.For<IJobListingRepository>();
        _companyRepo = Substitute.For<ICompanyRepository>();
        _sut = new JobListingService(_jobRepo, _companyRepo);
    }

    [Fact]
    public async Task CreateAsync_WhenConflictExists_ThrowDUplicateListingException()
    {
        // Given
    
        // When
    
        // Then
    }
}

