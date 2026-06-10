using API.Models;

public class ApplicationStatusRules
{
    private static readonly Dictionary<ApplicationStatus,
        HashSet<ApplicationStatus>> AllowedTransitions = new()
    {
        {
            ApplicationStatus.Submitted,
            new()
            {
                ApplicationStatus.UnderReview
            }
        },

        {
            ApplicationStatus.UnderReview,
            new()
            {
                ApplicationStatus.Shortlisted,
                ApplicationStatus.Rejected
            }
        },

        {
            ApplicationStatus.Shortlisted,
            new()
            {
                ApplicationStatus.OfferMade,
                ApplicationStatus.Rejected
            }
        },

        {
            ApplicationStatus.OfferMade,
            new()
            {
                ApplicationStatus.Offered,
                ApplicationStatus.Rejected
            }
        }
    };

    public bool IsTransitionAllowed(
        ApplicationStatus currentStatus,
        ApplicationStatus newStatus)
    {
        return AllowedTransitions.TryGetValue(
                   currentStatus,
                   out var validTransitions)
               &&
               validTransitions.Contains(newStatus);
    }
}