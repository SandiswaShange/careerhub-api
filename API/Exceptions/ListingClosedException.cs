namespace API.Exceptions;

public class ListingClosedException : Exception
{
    public ListingClosedException(Guid listingId)
        : base($"Listing '{listingId}' is closed.")
    {
    }
}