using System.Net;
using MassTransit;
using Polly;
using Polly.Extensions.Http;
using PostService.Consumers;
using PostService.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddMassTransit(x =>
{
	x.AddConsumersFromNamespaceContaining<AuctionCreatedConsumer>();

	x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("post", false));

	x.UsingRabbitMq((context, cfg) =>
	{
		cfg.UseMessageRetry(r =>
		{
			r.Handle<RabbitMqConnectionException>();
			r.Interval(5, TimeSpan.FromSeconds(10));
		});

		cfg.Host(builder.Configuration["RabbitMq:Host"], "/", host =>
		{
			host.Username(builder.Configuration.GetValue("RabbitMq:Username", "guest"));
			host.Password(builder.Configuration.GetValue("RabbitMq:Password", "guest"));
		});

		cfg.ReceiveEndpoint("search-post-created", e =>
		{
			e.UseMessageRetry(r => r.Interval(5, 5));

			e.ConfigureConsumer<AuctionCreatedConsumer>(context);
		});

		cfg.ConfigureEndpoints(context);
	});
});
// I think it needs a change to build it
var app = builder.Build();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Lifetime.ApplicationStarted.Register(async () =>
{
	await Policy.Handle<TimeoutException>().WaitAndRetryAsync(5, t => TimeSpan.FromSeconds(10))
		.ExecuteAndCaptureAsync(async () => await DbInitializer.InitDb(app));
});

app.Run();
return;


static IAsyncPolicy<HttpResponseMessage> GetPolicy()
	=> HttpPolicyExtensions
		.HandleTransientHttpError()
		.OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
		.WaitAndRetryForeverAsync(_ => TimeSpan.FromSeconds(3));