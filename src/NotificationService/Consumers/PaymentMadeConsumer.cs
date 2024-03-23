using Contracts;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using NotificationService.Hubs;

namespace NotificationService.Consumers;

/// <summary>
/// Consumes the PaymentMade event messages.
/// </summary>
/// <remarks>
/// This consumer listens for PaymentMade events and notifies all connected clients via SignalR about the payment.
/// It uses the NotificationHub to broadcast the message to all connected clients.
/// </remarks>
public class PaymentMadeConsumer : IConsumer<PaymentMade>
{
	private readonly IHubContext<NotificationHub> _hubContext;

	/// <summary>
	/// Initializes a new instance of the <see cref="PaymentMadeConsumer"/> class.
	/// </summary>
	/// <param name="hubContext">The SignalR hub context used for communicating with clients.</param>
	public PaymentMadeConsumer(IHubContext<NotificationHub> hubContext)
	{
		_hubContext = hubContext;
	}

	/// <summary>
	/// Handles the received PaymentMade event message.
	/// </summary>
	/// <param name="context">The consume context which contains the message data.</param>
	/// <returns>A task that represents the asynchronous operation of processing the message.</returns>
	/// <remarks>
	/// When a PaymentMade message is received, this method broadcasts it to all connected clients
	/// using the SignalR hub context.
	/// </remarks>
	public async Task Consume(ConsumeContext<PaymentMade> context)
	{
		Console.WriteLine("--> payment made message received");
		await _hubContext.Clients.All.SendAsync("PaymentMade", context.Message);
	}
}