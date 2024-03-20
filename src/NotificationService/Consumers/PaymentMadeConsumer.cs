using Contracts;
using MassTransit;

namespace NotificationService.Consumers;

public class PaymentMadeConsumer : IConsumer<PaymentMade>
{
	public async Task Consume(ConsumeContext<PaymentMade> context)
	{
		throw new NotImplementedException();
	}
}