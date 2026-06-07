using API.Models;

namespace API.Exceptions;

public class InvalidStatusTransitionException : Exception
{
    public InvalidStatusTransitionException(
        ApplicationStatus current,
        ApplicationStatus next)
        : base(
            $"Cannot move from {current} to {next}.")
    {
    }
}