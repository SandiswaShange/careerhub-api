using API.Models;
using API.DTOs;
public interface IApplicationRepository
{
    Task<bool> HasApplicantAppliedAsync(Guid applicantId,Guid listingId);
    Task<IEnumerable<ApplicationResponse>>GetApplicationsForListingAsync(Guid listingId);
    Task<IEnumerable<ApplicationResponse>>GetApplicationsByApplicantAsync(Guid applicantId);
    Task<Application?>GetByIdAsync(Guid applicantId,Guid listingId);
    Task AddAsync(Application application);
    Task UpdateAsync(Application application);
    Task RemoveAsync(Application application);
}