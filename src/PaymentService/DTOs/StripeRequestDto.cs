using System.Text.Json.Serialization;
using PaymentService.Entities;

namespace PaymentService.DTOs;

/// <summary>
/// Represents data transfer object for a Stripe payment request, containing session and payment details as well as auction information.
/// This DTO simplifies client-server data exchange for payment processing.
/// </summary>
public class StripeRequestDto
{
	public string? StripeSessionUrl { get; set; }
	public string? StripeSessionId { get; set; }
	public string? ApprovedUrl { get; set; }
	public string? CancelUrl { get; set; }
	public string? UserId { get; set; }

	public List<Coupon>? Coupons { get; set; }
	[JsonPropertyName("id")] public Guid? AuctionId { get; set; }

	public string? PaymentIntentId { get; set; }
	public string? Model { get; set; }
	public int? SoldAmount { get; set; }

	// Properties extracted from dataObject
	public int ReservePrice { get; set; }
	public string? Seller { get; set; }
	public string? Winner { get; set; }
	public int CurrentHighBid { get; set; }
	public DateTime CreatedAt { get; set; }
	public DateTime UpdatedAt { get; set; }
	public DateTime AuctionEnd { get; set; }
	public string? Status { get; set; }
	public string? Make { get; set; }
	public int Year { get; set; }
	public string? Color { get; set; }
	public int Mileage { get; set; }
	public string? ImageUrl { get; set; }
	public string? CouponCode { get; set; }

	public Guid Guid { get; set; } = Guid.NewGuid();
}