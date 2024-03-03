namespace BiddingService.Models;

/// <summary>
/// Enumerates the possible statuses of a bid in relation to the auction's reserve price and other bids.
/// </summary>
public enum BidStatus
{
	Accepted,
	AcceptedBelowReserve,
	TooLow,
	Finished
}