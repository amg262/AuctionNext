using System.ComponentModel.DataAnnotations;

namespace PaymentService.Entities;

/// <summary>
/// Represents a reward entity in the database.
/// This entity stores information about rewards that users earn through various activities or transactions within the application.
/// </summary>
public class Reward
{
	[Key] public int Id { get; set; }
	public string? UserId { get; set; }
	public DateTime RewardsDate { get; set; } = DateTime.UtcNow;
	public double? RewardsActivity { get; set; } = 0;
	public Guid? PaymentId { get; set; }
}