namespace PaymentService.DTOs;

/// <summary>
/// Represents a data transfer object for coupon information.
/// This class is used to transfer coupon data between layers without necessarily exposing the domain model.
/// </summary>
public record CouponDto
{
	public int? CouponId { get; set; }
	public string? CouponCode { get; set; }
	public double? DiscountAmount { get; set; }
	public int? MinAmount { get; set; }
}