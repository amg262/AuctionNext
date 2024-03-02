﻿using MongoDB.Entities;

namespace SearchService;

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