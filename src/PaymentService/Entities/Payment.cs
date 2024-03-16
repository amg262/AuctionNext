using PaymentService.Utility;

namespace PaymentService.Entities;

/// <summary>
/// Provides a repository for managing Payment entities, encapsulating CRUD operations and business logic related to payments, utilizing Entity Framework Core for data access and AutoMapper for object mapping.
/// </summary>
public class Payment
{
	public Guid Id { get; set; }
	public string? UserId { get; set; }
	public string? CouponCode { get; set; }
	public double? Discount { get; set; }
	public double? Total { get; set; }
	public string? Name { get; set; }
	public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
	public string? Status { get; set; } = PaymentHelper.StatusPending;
	public string? PaymentIntentId { get; set; }
	public string? StripeSessionId { get; set; }
	public Guid? AuctionId { get; set; }
}