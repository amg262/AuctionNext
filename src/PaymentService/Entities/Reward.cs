using System.ComponentModel.DataAnnotations;

namespace PaymentService.Entities;

public class Reward
{
	[Key] public int Id { get; set; }
	public string? UserId { get; set; }
	public DateTime RewardsDate { get; set; } = DateTime.UtcNow;
	public double? RewardsActivity { get; set; } = 0;
	public Guid? PaymentId { get; set; }
}