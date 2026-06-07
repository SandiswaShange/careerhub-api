using API.DTOs;
using API.Models;

public interface IApplicationService
{
    Task SubmitApplicationAsync(CreateApplicationRequest dto);

    Task<IEnumerable<ApplicationResponse>>GetApplicationsForListingAsync(Guid listingId);

    Task<IEnumerable<ApplicationResponse>>GetApplicationsByApplicantAsync(Guid applicantId);

    Task UpdateStatusAsync(Guid applicantId, Guid listingId, ApplicationStatus newStatus);

    Task WithdrawApplicationAsync(Guid applicantId, Guid listingId, Guid requestingApplicantId);
}