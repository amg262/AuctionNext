namespace Contracts;

/// <summary>
/// Represents an event indicating that a payment has been made.
/// This class is used to communicate payment information within the system and to external services.
/// </summary>
public class PaymentMade
{
	public Guid Id { get; set; }
	public string? UserId { get; set; }
	public int? CouponId { get; set; }
	public Coupon? Coupon { get; set; }
	public string? CouponCode { get; set; }
	public double? Discount { get; set; }
	public double? Total { get; set; }
	public string? Name { get; set; }
	public DateTime UpdatedAt { get; set; }
	public string? Status { get; set; }
	public string? PaymentIntentId { get; set; }
	public string? StripeSessionId { get; set; }
	public Guid? AuctionId { get; set; }
}