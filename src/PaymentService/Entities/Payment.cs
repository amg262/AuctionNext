using System.ComponentModel.DataAnnotations.Schema;
using PaymentService.Utility;

namespace PaymentService.Entities;

/// <summary>
/// Provides a repository for managing Payment entities, encapsulating CRUD operations and business logic related to payments,
/// utilizing Entity Framework Core for data access and AutoMapper for object mapping.
/// </summary>
public class Payment
{
	public Guid Id { get; set; }
	public string? UserId { get; set; }

	[ForeignKey("Coupon")] public int? CouponId { get; set; }
	public Coupon? Coupon { get; set; }
	public string? CouponCode { get; set; }

	public double? Discount { get; set; }
	public double? Total { get; set; }
	public string? Name { get; set; }
	public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
	public string? Status { get; set; } = PaymentHelper.StatusPending;
	public string? PaymentIntentId { get; set; }
	public string? StripeSessionId { get; set; }

	public float? Length { get; set; } //= 120; // inches
	public float? Width { get; set; } //= 60; // inches
	public float? Height { get; set; } //= 60; // inches
	public float? Weight { get; set; } //= 65504; // ounces
	public Guid? AuctionId { get; set; }
}