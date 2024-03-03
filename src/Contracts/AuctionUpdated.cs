namespace Contracts;

/// <summary>
/// Represents the details of an auction when it is updated.
/// This class is used to transfer auction data within the system or between systems.
/// </summary>
public class AuctionUpdated
{
	public string Id { get; set; }
	public string Make { get; set; }
	public string Model { get; set; }
	public int Year { get; set; }
	public string Color { get; set; }
	public int Mileage { get; set; }
}