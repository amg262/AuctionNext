using Contracts;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using NotificationService.Hubs;

namespace NotificationService.Consumers;

public class PostCreatedConsumer : IConsumer<PostCreated>
{
    private readonly IHubContext<NotificationHub> _hubContext;

    public PostCreatedConsumer(IHubContext<NotificationHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task Consume(ConsumeContext<PostCreated> message)
    {
        Console.WriteLine("--> post created message received");
        await _hubContext.Clients.All.SendAsync("PostCreated", message);
    }
}