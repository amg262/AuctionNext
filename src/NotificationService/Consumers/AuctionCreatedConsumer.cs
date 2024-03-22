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
		const string jsonString = """
		                          {
		                              "Id": "9f8d98c9-3f7f-4a4a-b9ed-11b92cf629a3",
		                              "UserId": "user123",
		                              "CouponId": 1,
		                              "CouponCode": "10OFF",
		                              "Coupon": {
		                                  "CouponId": 1,
		                                  "CouponCode": "10OFF",
		                                  "DiscountAmount": 10.0,
		                                  "MinAmount": 10
		                              },
		                              "Discount": 10.0,
		                              "Total": 100.0,
		                              "Name": "John Doe",
		                              "UpdatedAt": "2024-03-22T12:34:56Z",
		                              "Status": "Completed",
		                              "PaymentIntentId": "pi_123456789",
		                              "StripeSessionId": "si_123456789",
		                              "AuctionId": "ae7a3c77-7622-4da7-b1f3-123456789012"
		                          }
		                          """;

		Console.WriteLine($"--> Auction created: message received");
		await _hubContext.Clients.All.SendAsync("AuctionCreated", context.Message);
		// await _hubContext.Clients.All.SendAsync("PaymentMade", jsonString);
	}
}