using System.Text.Json.Serialization;

namespace PaymentService.DTOs;

/// <summary>
/// Represents data transfer object for a Stripe payment request, containing session and payment details as well as auction information. This DTO simplifies client-server data exchange for payment processing.
/// </summary>
public class StripeRequestDto
{
	public string? StripeSessionUrl { get; set; }
	public string? StripeSessionId { get; set; }
	public string? ApprovedUrl { get; set; }
	public string? CancelUrl { get; set; }

	[JsonPropertyName("id")] public string? AuctionId { get; set; }

	public string? PaymentIntentId { get; set; }

	public string? Model { get; set; }
	public int? SoldAmount { get; set; }

	[JsonPropertyName("data")] public object? Data { get; set; }
}