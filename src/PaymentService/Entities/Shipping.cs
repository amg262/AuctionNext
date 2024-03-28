using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EasyPost.Models.API;

namespace PaymentService.Entities;

public class Shipping
{
	[Key] public int ShippingId { get; set; }
	[ForeignKey("PaymentId")] public Guid? PaymentId { get; set; }
	public Payment Payment { get; set; }
	public string? TrackingCode { get; set; }
	public string? TrackingUrl { get; set; }
	public string? Rate { get; set; }
	public string? Carrier { get; set; }
	public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
	public string? Name { get; set; }
	public string? Company { get; set; }
	public string? Street1 { get; set; }
	public string? Street2 { get; set; }
	public string? City { get; set; }
	public string? State { get; set; }
	public string? Zip { get; set; }
	public string? Country { get; set; }
	public string? Email { get; set; }
}