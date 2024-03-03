using MongoDB.Entities;

namespace SearchService.Models;

/// <summary>
/// Represents an item entity in the database, used for mapping the Item collection in MongoDB.
/// Inheriting from <see cref="Entity"/> automatically provides the item with an Id property (of type string),
/// which serves as the primary key in the database and enables easy CRUD operations through MongoDB.Entities.
/// This class represents the Item entity in the database and is used to map the Item collection in the database.
/// </summary>
public class Item : Entity
{
	public int ReservePrice { get; set; }
	public string Seller { get; set; }
	public string Winner { get; set; }
	public int SoldAmount { get; set; }
	public int CurrentHighBid { get; set; }
	public DateTime CreatedAt { get; set; }
	public DateTime UpdatedAt { get; set; }
	public DateTime AuctionEnd { get; set; }
	public string Status { get; set; }
	public string Make { get; set; }
	public string Model { get; set; }
	public int Year { get; set; }
	public string Color { get; set; }
	public int Mileage { get; set; }
	public string ImageUrl { get; set; }

	public bool HasSeller() => !string.IsNullOrWhiteSpace(Seller);

	public bool HasValidCreatedDate() => CreatedAt <= DateTime.UtcNow;
}