using Contracts;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using NotificationService.Hubs;

namespace NotificationService.Consumers;

/// <summary>
/// Consumes messages about newly created comments and notifies all connected clients via SignalR.
/// </summary>
public class CommentCreatedConsumer : IConsumer<CommentCreated>
{
    private readonly IHubContext<NotificationHub> _hubContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="CommentCreatedConsumer"/> class.
    /// </summary>
    /// <param name="hubContext">The hub context used for SignalR communication.</param>
    public CommentCreatedConsumer(IHubContext<NotificationHub> hubContext)
    {
        _hubContext = hubContext;
    }

    /// <summary>
    /// Handles the consumption of a <see cref="CommentCreated"/> message.
    /// Notifies all connected clients that a new comment has been created.
    /// </summary>
    /// <param name="context">The consume context which contains the message data.</param>
    public async Task Consume(ConsumeContext<CommentCreated> context)
    {
        Console.WriteLine("--> comment created message received");
        await _hubContext.Clients.All.SendAsync("CommentCreated", context.Message);
    }
}