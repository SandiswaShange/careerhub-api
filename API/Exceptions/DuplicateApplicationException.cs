namespace API.Exceptions;

public class DuplicateApplicationException : Exception
{
    public DuplicateApplicationException()
        : base("Applicant has already applied.")
    {
    }
}