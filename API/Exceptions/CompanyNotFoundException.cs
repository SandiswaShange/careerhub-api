namespace API.Exceptions;

public class CompanyNotFoundException : Exception
{
    public CompanyNotFoundException(string companyName)
        : base($"Company '{companyName}' does not exist.")
    {
    }
}