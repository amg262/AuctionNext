namespace PaymentService.Utility;

/// <summary>
/// Provides constants representing various statuses for payment transactions.
/// These constants can be used throughout the PaymentService to maintain consistency in status representation.
/// </summary>
public static class PaymentHelper
{
	public const string StatusPending = "Pending";
	public const string StatusApproved = "Approved";
	public const string StatusReadyForPickup = "ReadyForPickup";
	public const string StatusCompleted = "Completed";
	public const string StatusRefunded = "Refunded";
	public const string StatusCancelled = "Cancelled";
	public const string StatusSucceeded = "Succeeded";
	public const string RequiresPaymentMethod = "requires_payment_method";
}