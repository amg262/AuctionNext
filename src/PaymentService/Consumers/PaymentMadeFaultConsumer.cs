using Contracts;
using MassTransit;

namespace PaymentService.Consumers;

public class PaymentMadeFaultConsumer : IConsumer<Fault<PaymentMade>>
{
	public async Task Consume(ConsumeContext<Fault<PaymentMade>> context)
	{
		throw new NotImplementedException();
	}
}