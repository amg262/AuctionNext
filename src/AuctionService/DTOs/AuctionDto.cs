﻿namespace AuctionService.DTOs;

/// <summary>
/// Represents a Data Transfer Object for auction data.
/// </summary>
public class AuctionDto
{
	public Guid Id { get; set; }
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
	public float? Length { get; set; } //= 120; // inches
	public float? Width { get; set; } //= 60; // inches
	public float? Height { get; set; } //= 60; // inches
	public float? Weight { get; set; } //= 65504; // ounces
}