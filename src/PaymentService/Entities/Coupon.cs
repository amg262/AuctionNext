using System.ComponentModel.DataAnnotations;

namespace PaymentService.Entities;

/// <summary>
/// Represents a coupon entity in the database.
/// This entity is used to store information about discount coupons that can be applied to payments or purchases.
/// </summary>
public class Coupon
{
	[Key] public int CouponId { get; set; }
	public string? CouponCode { get; set; }
	public double? DiscountAmount { get; set; }
	public int? MinAmount { get; set; }
}