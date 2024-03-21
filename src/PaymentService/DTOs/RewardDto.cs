namespace PaymentService.DTOs;

/// <summary>
/// Represents a data transfer object for reward information.
/// This class is used to transfer reward data between layers, encapsulating information related to user rewards.
/// </summary>
public record RewardDto
{
	public int? Id { get; set; }
	public string? UserId { get; set; }
	public DateTime RewardsDate { get; set; }
	public double? RewardsActivity { get; set; }
	public Guid? PaymentId { get; set; }
}