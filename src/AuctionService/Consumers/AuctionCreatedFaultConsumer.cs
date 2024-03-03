using Contracts;
using MassTransit;

namespace AuctionService.Consumers;

/// <summary>
/// A consumer for processing fault messages that occur when an attempt to process an AuctionCreated event fails.
/// This consumer specifically handles faults related to AuctionCreated events, logging the fault and conditionally republishing the message.
/// </summary>
public class AuctionCreatedFaultConsumer : IConsumer<Fault<AuctionCreated>>
{
	/// <summary>
	/// Asynchronously consumes a faulted <see cref="AuctionCreated"/> event message, applying conditional logic based on the exception type encountered.
	/// </summary>
	/// <param name="context">The consume context containing the faulted message and details about the exception that triggered the fault.</param>
	/// <returns>A task representing the asynchronous consume operation.</returns>
	/// <remarks>
	/// This method processes fault messages resulting from attempts to handle <see cref="AuctionCreated"/> events. It specifically looks for faults caused by <see cref="System.ArgumentException"/>.
	/// If such an exception is the cause, it logs the occurrence and republishes the <see cref="AuctionCreated"/> message with modifications intended to correct or mitigate the issue that caused the original fault.
	/// This enables a form of error recovery where certain types of faults can be programmatically addressed, allowing the system to attempt reprocessing of the message.
	/// 
	/// If the fault was not caused by an <see cref="System.ArgumentException"/>, the method logs the fault without attempting to republish the message, under the assumption that the fault requires manual investigation or is not recoverable through simple modification and republishing.
	/// </remarks>
	public async Task Consume(ConsumeContext<Fault<AuctionCreated>> context)
	{
		Console.WriteLine("--> Consuming faulty creation");

		var exception = context.Message.Exceptions.First();

		if (exception.ExceptionType == "System.ArgumentException")
		{
			context.Message.Message.Model = "FooBar";
			await context.Publish(context.Message.Message);
		}
		else
		{
			Console.WriteLine("Not an argument exception - update error dashboard somewhere");
		}
	}
}