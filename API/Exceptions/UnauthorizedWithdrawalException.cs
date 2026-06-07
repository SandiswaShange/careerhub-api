namespace API.Exceptions;

public class UnauthorizedWithdrawalException
    : Exception
{
    public UnauthorizedWithdrawalException()
        : base("Applicants may only withdraw their own applications.")
    {
    }
}