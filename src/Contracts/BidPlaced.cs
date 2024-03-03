namespace Contracts;

/// <summary>
/// Represents a bid placed on an auction, including the bidder's information, bid amount, and the bid's status.
/// </summary>
public class BidPlaced
{
	public string Id { get; set; }
	public string AuctionId { get; set; }
	public string Bidder { get; set; }
	public DateTime BidTime { get; set; }
	public int Amount { get; set; }
	public string BidStatus { get; set; }
}