namespace Contracts;

/// <summary>
/// Represents a coupon entity.
/// Used for storing information about discount coupons applicable to payments or purchases.
/// </summary>
public class Coupon
{
	public int CouponId { get; set; }
	public string? CouponCode { get; set; }
	public double? DiscountAmount { get; set; }
	public int? MinAmount { get; set; }
}