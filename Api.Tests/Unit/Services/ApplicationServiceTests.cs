using API.Exceptions;
using API.Models;
using NSubstitute;
using Xunit.Extensions;

public class ApplicationServiceTests
{
    private readonly IApplicationRepository _applicationRepository;
    private readonly IJobListingRepository _jobRepository;
    private readonly ApplicationStatusRules _statusRules;
    private readonly ApplicationService _sut;

    public ApplicationServiceTests()
    {
        _applicationRepository = Substitute.For<IApplicationRepository>();

        _jobRepository = Substitute.For<IJobListingRepository>();

        _statusRules = new ApplicationStatusRules();

        _sut = new ApplicationService(_applicationRepository, _jobRepository, _statusRules);
    }
//==========================================Illegal Application===================================================================

    [Theory]
    [InlineData(
        ApplicationStatus.Submitted,
        ApplicationStatus.UnderReview)]

    [InlineData(
        ApplicationStatus.UnderReview,
        ApplicationStatus.Shortlisted)]

    [InlineData(
        ApplicationStatus.UnderReview,
        ApplicationStatus.Rejected)]

    [InlineData(
        ApplicationStatus.Shortlisted,
        ApplicationStatus.Offered)]

    [InlineData(
        ApplicationStatus.Shortlisted,
        ApplicationStatus.Rejected)]

    public async Task
        UpdateStatusAsync_WhenTransitionIsLegal_CallsUpdateAsync(
            ApplicationStatus from,
            ApplicationStatus to)
    {
        // Arrange
        var applicantId = Guid.NewGuid();
        var listingId = Guid.NewGuid();

        var application = new Application
        {
            ApplicantId = applicantId,
            JobListingId = listingId,
            Status = from
        };

        _applicationRepository
            .GetByIdAsync(applicantId, listingId)
            .Returns(application);

        // Act
        await _sut.UpdateStatusAsync(
            applicantId,
            listingId,
            to);

        // Assert
        await _applicationRepository
            .Received(1)
            .UpdateAsync(Arg.Any<Application>());
    }
//==============================================Legal Application=================================================================
   [Theory]
    [InlineData(
    ApplicationStatus.Rejected,
    ApplicationStatus.Submitted)]

    [InlineData(
    ApplicationStatus.Offered,
    ApplicationStatus.Submitted)]

    [InlineData(
    ApplicationStatus.Rejected,
    ApplicationStatus.UnderReview)]

    [InlineData(
    ApplicationStatus.Offered,
    ApplicationStatus.Shortlisted)]
    
    public async Task UpdateStatusAsync_WhenTransitionIsIllegal_ThrowsException(
        ApplicationStatus from,
        ApplicationStatus to)
    {
        // Arrange
        var applicantId = Guid.NewGuid();
        var listingId = Guid.NewGuid();

        var application = new Application
        {
            ApplicantId = applicantId,
            JobListingId = listingId,
            Status = from
        };

        _applicationRepository
            .GetByIdAsync(applicantId, listingId)
            .Returns(application);

        // Act
        var act = () => _sut.UpdateStatusAsync(
            applicantId,
            listingId,
            to);

        // Assert
        await Assert.ThrowsAnyAsync<InvalidStatusTransitionException>(act);

        await _applicationRepository.DidNotReceive().UpdateAsync(Arg.Any<Application>());
    }
//==================================================pp not found===================================================================
    [Fact]
    public async Task
        UpdateStatusAsync_WhenApplicationNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var applicantId = Guid.NewGuid();
        var listingId = Guid.NewGuid();

        _applicationRepository
            .GetByIdAsync(applicantId, listingId)
            .Returns((Application?)null);

        // Act
        var act = () => _sut.UpdateStatusAsync(
            applicantId,
            listingId,
            ApplicationStatus.UnderReview);

        // Assert
        await Assert.ThrowsAnyAsync<
            ApplicationNotFoundException>(act);

        await _applicationRepository
            .DidNotReceive()
            .UpdateAsync(Arg.Any<Application>());
    }

}