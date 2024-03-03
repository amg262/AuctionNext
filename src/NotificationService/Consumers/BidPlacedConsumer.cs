using Contracts;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using NotificationService.Hubs;

namespace NotificationService.Consumers;

/// <summary>
/// Consumes messages indicating that a bid has been placed on an auction. This consumer receives
/// the BidPlaced message, logs its receipt, and notifies all connected clients via SignalR.
/// </summary>
public class BidPlacedConsumer : IConsumer<BidPlaced>
{
	private readonly IHubContext<NotificationHub> _hubContext;

	/// <summary>
	/// Initializes a new instance of the <see cref="BidPlacedConsumer"/> class.
	/// </summary>
	/// <param name="hubContext">The hub context for the SignalR notifications.</param>
	public BidPlacedConsumer(IHubContext<NotificationHub> hubContext)
	{
		_hubContext = hubContext;
	}

	/// <summary>
	/// Handles the reception of a BidPlaced message. This method logs the message reception
	/// and notifies all clients connected through SignalR about the new bid.
	/// </summary>
	/// <param name="context">The consume context containing the message data.</param>
	/// <returns>A task that represents the asynchronous operation of message consumption and client notification.</returns>
	public async Task Consume(ConsumeContext<BidPlaced> context)
	{
		Console.WriteLine("--> bid placed message received");

		await _hubContext.Clients.All.SendAsync("BidPlaced", context.Message);
	}
}