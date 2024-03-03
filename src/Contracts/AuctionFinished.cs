namespace Contracts;

/// <summary>
/// Represents the completion state of an auction, indicating whether an item was sold, to whom, and for what amount.
/// </summary>
public class AuctionFinished
{
	public bool ItemSold { get; set; }
	public string AuctionId { get; set; }
	public string Winner { get; set; }
	public string Seller { get; set; }
	public int? Amount { get; set; }
}