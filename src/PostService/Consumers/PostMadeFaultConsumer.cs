using Contracts;
using MassTransit;

namespace PostService.Consumers;

/// <summary>
/// A consumer that handles fault messages for the PaymentMade message type.
/// This consumer is responsible for responding to any faults that occur during
/// the processing of PaymentMade messages, such as logging the error and notifying
/// other parts of the system about the failure.
/// </summary>
public class PostMadeFaultConsumer : IConsumer<Fault<PostCreated>>
{
    /// <summary>
    /// Processes the faulted PaymentMade message, logging the error and publishing a 
    /// notification to inform other services or components about the processing failure.
    /// </summary>
    /// <param name="context">The context of the consumed fault message, providing access to fault details.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task Consume(ConsumeContext<Fault<PostCreated>> context)
    {
        // Extract and log the first exception information (if exists)
        var exceptionInfo = context.Message.Exceptions.FirstOrDefault();
        if (exceptionInfo != null)
        {
            Console.WriteLine($"Exception Type: {exceptionInfo.ExceptionType}");
            Console.WriteLine($"Exception Message: {exceptionInfo.Message}");
            // Consider logging additional details like stack trace, inner exception, etc.
            // Ideally, use a structured logging framework for better log management.
        }

        // Publish a notification about the fault
        // Replace `IPaymentFaulted` with your specific contract/message definition for notifying about payment processing faults.
        await context.Publish<IPostFaulted>(new
        {
            PostId = context.Message.Message.Id,
            Reason = "Failed to process payment.",
            ExceptionMessage = exceptionInfo?.Message,
            Timestamp = DateTime.UtcNow
        });

        // Note on Retry Policies and Dead Letter Queues:
        // - Consider configuring Retry Policies in your consumer configuration if automatic retries are desired.
        // - For messages that cannot be processed successfully after retries, consider using a dead letter queue.
        // - Ensure your message handlers are idempotent to safely allow for retries.
        // - Integrate with an alerting system to notify the appropriate personnel in case of critical failures.
    }
}

/// <summary>
/// Defines the structure of a message used to notify interested parties
/// of a fault that occurred during the processing of a payment.
/// </summary>
public interface IPostFaulted
{
    string PostId { get; }
    string Reason { get; }
    string ExceptionMessage { get; }
    DateTime Timestamp { get; }
}