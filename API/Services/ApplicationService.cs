using API.DTOs;
using API.Exceptions;
using API.Models;

public class ApplicationService(IApplicationRepository applicationRepository,IJobListingRepository jobRepository,
                                    ApplicationStatusRules statusRules): IApplicationService
{
    private readonly IApplicationRepository _applicationRepository = applicationRepository;

    private readonly IJobListingRepository _jobRepository = jobRepository;

    private readonly ApplicationStatusRules _statusRules = statusRules;

    public async Task<IEnumerable<ApplicationResponse>> GetApplicationsByApplicantAsync(Guid applicantId)
    {
       return await _applicationRepository
        .GetApplicationsByApplicantAsync(applicantId);
    }

    public async Task<IEnumerable<ApplicationResponse>> GetApplicationsForListingAsync(Guid listingId)
    {
         return await _applicationRepository
        .GetApplicationsForListingAsync(listingId);
    }

    public async Task SubmitApplicationAsync(CreateApplicationRequest dto)
    {
        bool alreadyApplied = await _applicationRepository
            .HasApplicantAppliedAsync(
                dto.ApplicantId,
                dto.JobListingId);

    if (alreadyApplied)
    {
        throw new DuplicateApplicationException();
    }

    bool listingOpen = await _jobRepository
            .IsOpenForApplicationsAsync(
                dto.JobListingId);

    if (!listingOpen)
    {
        throw new ListingClosedException(dto.JobListingId);
    }

    var application = new Application
    {
        ApplicantId = dto.ApplicantId,
        JobListingId = dto.JobListingId,
        SubmittedAt = DateTime.UtcNow,
        Status = ApplicationStatus.Submitted,
        HearAboutRole = dto.HearAboutRole
    };

    await _applicationRepository
        .AddAsync(application);
    }

    public async Task UpdateStatusAsync(Guid applicantId, Guid listingId, ApplicationStatus newStatus)
    {
        var application =
        await _applicationRepository.GetByIdAsync(
            applicantId,
            listingId);

        if (application is null)
        {
            throw new ApplicationNotFoundException(
                applicantId,
                listingId);
        }

        bool allowed =
            _statusRules.IsTransitionAllowed(
                application.Status,
                newStatus);

        if (!allowed)
        {
            throw new InvalidStatusTransitionException(
                application.Status,
                newStatus);
        }

        application.Status = newStatus;

        await _applicationRepository.UpdateAsync(
            application);
    }

    public async Task WithdrawApplicationAsync(Guid applicantId, Guid listingId, Guid requestingApplicantId)
    {
        if (applicantId != requestingApplicantId)
    {
        throw new UnauthorizedWithdrawalException();
    }

    var application =
        await _applicationRepository.GetByIdAsync(
            applicantId,
            listingId);

    if (application is null)
    {
        throw new ApplicationNotFoundException(
            applicantId,
            listingId);
    }

    await _applicationRepository.RemoveAsync(
        application);
    }
    //============================================================================================================================
    private static bool IsValidTransition(ApplicationStatus current,ApplicationStatus next)
    {
        return current switch
        {
            ApplicationStatus.Submitted =>
                next is
                    ApplicationStatus.UnderReview
                    or ApplicationStatus.Rejected,

            ApplicationStatus.UnderReview =>
                next is
                    ApplicationStatus.Shortlisted
                    or ApplicationStatus.Rejected,

            ApplicationStatus.Shortlisted =>
                next is
                    ApplicationStatus.Offered
                    or ApplicationStatus.Rejected,

            ApplicationStatus.Rejected => false,

            ApplicationStatus.Offered => false,

            _ => false
        };
    }
}