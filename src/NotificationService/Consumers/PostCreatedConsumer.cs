using Contracts;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using NotificationService.Hubs;

namespace NotificationService.Consumers;

/// <summary>
/// Consumes messages of type <see cref="PostCreated"/>. This consumer is responsible
/// for receiving post creation messages and notifying connected clients via SignalR.
/// </summary>
public class PostCreatedConsumer : IConsumer<PostCreated>
{
    private readonly IHubContext<NotificationHub> _hubContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="PostCreatedConsumer"/> class with the specified hub context.
    /// </summary>
    /// <param name="hubContext">The SignalR hub context used for notifications.</param>
    public PostCreatedConsumer(IHubContext<NotificationHub> hubContext)
    {
        _hubContext = hubContext;
    }

    /// <summary>
    /// Handles the consumption of the <see cref="PostCreated"/> message.
    /// When a post is created, this method is called, and it notifies all connected clients
    /// about the new post using the SignalR hub.
    /// </summary>
    /// <param name="context">The context containing the message data.</param>
    public async Task Consume(ConsumeContext<PostCreated> context)
    {
        Console.WriteLine("--> post created message received");
        await _hubContext.Clients.All.SendAsync("PostCreated", context.Message);
    }
}