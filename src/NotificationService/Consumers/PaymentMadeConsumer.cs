using Contracts;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using NotificationService.Hubs;

namespace NotificationService.Consumers;

public class PaymentMadeConsumer : IConsumer<PaymentMade>
{
	private readonly IHubContext<NotificationHub> _hubContext;

	public PaymentMadeConsumer(IHubContext<NotificationHub> hubContext)
	{
		_hubContext = hubContext;
	}

	public async Task Consume(ConsumeContext<PaymentMade> context)
	{
		Console.WriteLine("--> payment made message received");


		await _hubContext.Clients.All.SendAsync("BidPlaced", context.Message);

		await _hubContext.Clients.All.SendAsync("PaymentMade", context.Message);
	}
}