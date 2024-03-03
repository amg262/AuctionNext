namespace Contracts;

/// <summary>
/// Represents the details of an auction when it is deleted.
/// This class is used to transfer auction data within the system or between systems.
/// </summary>
public class AuctionDeleted
{
	public string Id { get; set; }
}