using API.Models;
using API.DTOs;
public interface IApplicationRepository
{
    Task<bool>HasApplicantAppliedAsync(Guid applicantId, Guid listingId);

    Task<IEnumerable<ApplicationResponse>>GetApplicationsForListingAsync(Guid listingId);

    Task<IEnumerable<ApplicationResponse>>GetApplicationsByApplicantAsync(Guid applicantId);

    Task<Application?>GetByIdAsync(Guid applicantId, Guid listingId);

    Task<Application> AddAsync(Application application);

    Task<Application> UpdateStatusAsync(Guid applicantId, Guid listingId, ApplicationStatus status);

    Task<Application> RemoveAsync(Application application);
}