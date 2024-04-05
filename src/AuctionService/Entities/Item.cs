using System.ComponentModel.DataAnnotations.Schema;

namespace AuctionService.Entities;

/// <summary>
/// Entity class representing an item in an auction
/// </summary>
[Table("Items")]
public class Item
{
	public Guid Id { get; set; }
	public string Make { get; set; }
	public string Model { get; set; }
	public int Year { get; set; }
	public string Color { get; set; }
	public int Mileage { get; set; }
	public string ImageUrl { get; set; }

	public float? Length { get; set; } = 120; // inches
	public float? Width { get; set; } = 60; // inches
	public float? Height { get; set; } = 60; // inches
	public float? Weight { get; set; } = 65504; // ounces
	// nav properties in EF Core it will be used to create a foreign key relationship
	public Auction Auction { get; set; }
	public Guid AuctionId { get; set; }
}