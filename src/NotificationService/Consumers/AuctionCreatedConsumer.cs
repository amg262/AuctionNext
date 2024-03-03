using Contracts;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using NotificationService.Hubs;

namespace NotificationService.Consumers;

/// <summary>
/// Consumes 'AuctionCreated' messages and notifies connected clients via SignalR.
/// This consumer listens for messages indicating the creation of an auction
/// and broadcasts this information to all clients connected to the NotificationHub,
/// allowing for real-time updates in the client application.
/// </summary>
public class AuctionCreatedConsumer : IConsumer<AuctionCreated>
{
	private readonly IHubContext<NotificationHub> _hubContext;

	/// <summary>
	/// Initializes a new instance of the <see cref="AuctionCreatedConsumer"/> class.
	/// </summary>
	/// <param name="hubContext">The SignalR hub context used to communicate with clients.</param>
	public AuctionCreatedConsumer(IHubContext<NotificationHub> hubContext)
	{
		_hubContext = hubContext;
	}

	/// <summary>
	/// Handles the consumption of 'AuctionCreated' messages. When an auction is created,
	/// this method is invoked, and the message is then forwarded to all connected clients
	/// via the SignalR hub, notifying them of the new auction.
	/// </summary>
	/// <param name="context">The context of the consumed message, containing the 'AuctionCreated' event data.</param>
	/// <returns>A task that represents the asynchronous operation of message consumption and client notification.</returns>
	public async Task Consume(ConsumeContext<AuctionCreated> context)
	{
		Console.WriteLine($"--> Auction created: message received");
		await _hubContext.Clients.All.SendAsync("AuctionCreated", context.Message);
	}
}