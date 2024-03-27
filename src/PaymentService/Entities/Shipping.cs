using System.ComponentModel.DataAnnotations;

namespace PaymentService.Entities;

public class Shipping
{
	[Key]
	public int ShippingId { get; set; }
	
}