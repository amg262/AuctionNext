using PaymentService.Entities;

namespace PaymentService.DTOs;

public class ShippingDto
{
    public int ShippingId { get; set; }
    public Guid? PaymentId { get; set; }
    public Payment Payment { get; set; }
    public string? TrackingCode { get; set; }
    public string? TrackingUrl { get; set; }
    public string? Rate { get; set; }
    public string? Carrier { get; set; }
    public DateTime? UpdatedAt { get; set; }
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