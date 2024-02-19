using Contracts;
using MassTransit;

namespace AuctionService.Consumers;

public class AuctionCreatedFaultConsumer : IConsumer<Fault<AuctionCreated>>
{
	public async Task Consume(ConsumeContext<Fault<AuctionCreated>> context)
	{
		Console.WriteLine($"--> AuctionCreated fault: {context.Message.FaultId}");
		var exception = context.Message.Exceptions.First();

		
		// if (exception.ExceptionType.GetType() == typeof(System.ArgumentException)) // more dynamic
		if (exception.ExceptionType == "System.ArgumentException")
		{
			Console.WriteLine("ArgumentException, republishing.");
			context.Message.Message.Model = "FooBar";
			await context.Publish(context.Message.Message);
		}
		else
		{
			Console.WriteLine("Not an ArgumentException, not republishing.");
		}
	}
}