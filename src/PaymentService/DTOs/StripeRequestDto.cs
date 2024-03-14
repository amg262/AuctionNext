namespace PaymentService.DTOs;

public class StripeRequestDto
{
	public string? StripeSessionUrl { get; set; }
	public string? StripeSessionId { get; set; }
	public string? ApprovedUrl { get; set; }
	public string? CancelUrl { get; set; }
	public Guid? AuctionId { get; set; }
	public string? Model { get; set; }
	public int? SoldAmount { get; set; }
}