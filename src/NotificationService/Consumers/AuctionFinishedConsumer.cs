using Contracts;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using NotificationService.Hubs;

namespace NotificationService.Consumers;

/// <summary>
/// Consumes messages indicating that an auction has finished. This consumer receives the
/// AuctionFinished message, logs its receipt, and notifies all connected clients via SignalR.
/// </summary>
public class AuctionFinishedConsumer : IConsumer<AuctionFinished>
{
	private readonly IHubContext<NotificationHub> _hubContext;

	/// <summary>
	/// Initializes a new instance of the <see cref="AuctionFinishedConsumer"/> class.
	/// </summary>
	/// <param name="hubContext">The hub context for the SignalR notifications.</param>
	public AuctionFinishedConsumer(IHubContext<NotificationHub> hubContext)
	{
		_hubContext = hubContext;
	}

	/// <summary>
	/// Handles the reception of an AuctionFinished message. This method logs the message reception
	/// and notifies all clients connected through SignalR about the auction's completion.
	/// </summary>
	/// <param name="context">The consume context containing the message data.</param>
	/// <returns>A task that represents the asynchronous operation of message consumption and client notification.</returns>
	public async Task Consume(ConsumeContext<AuctionFinished> context)
	{
		Console.WriteLine($"--> Auction finished: message received");
		await _hubContext.Clients.All.SendAsync("AuctionFinished", context.Message);
	}
}