using MongoDB.Entities;

namespace BiddingService.Models;

/// <summary>
/// Represents an auction with its properties, inheriting from MongoDB.Entities.Entity for database mapping.
/// </summary>
public class Auction : Entity
{
	public DateTime AuctionEnd { get; set; }
	public string Seller { get; set; }
	public int ReservePrice { get; set; }
	public bool Finished { get; set; }
}