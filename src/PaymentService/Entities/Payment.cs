namespace PaymentService.Entities;

public class Payment
{
	public int Id { get; set; }
	public string? UserId { get; set; }
	public string? CouponCode { get; set; }
	public double? Discount { get; set; }
	public double? Total { get; set; }
	public string? Status { get; set; }
	public string? PaymentIntentId { get; set; }
	public string? StripeSessionId { get; set; }
	public Guid? AuctionId { get; set; }
}