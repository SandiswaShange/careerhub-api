namespace API.Exceptions;

public class ApplicationNotFoundException : Exception
{
    public ApplicationNotFoundException(
        Guid applicantId,
        Guid listingId)
        : base(
            $"Application not found. Applicant: {applicantId}, Listing: {listingId}")
    {
    }
}