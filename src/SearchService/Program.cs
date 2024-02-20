using MassTransit;
using Polly;
using Polly.Extensions.Http;
using SearchService.Consumers;
using SearchService.Data;
using SearchService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddHttpClient<AuctionServiceHttpClient>().AddPolicyHandler(GetPolicy());
builder.Services.AddMassTransit(x =>
{
	x.AddConsumersFromNamespaceContaining<AuctionCreatedConsumer>();
	x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("search", false));
	x.UsingRabbitMq((context, cfg) =>
	{
		cfg.Host(builder.Configuration["RabbitMq:Host"], "/", h =>
		{
			h.Username(builder.Configuration.GetValue("RabbitMq:Username", "guest"));
			h.Password(builder.Configuration.GetValue("RabbitMq:Password", "guest"));
		});

		cfg.ReceiveEndpoint("search-auction-created", e =>
		{
			e.UseMessageRetry(r => r.Interval(5, 5));
			e.ConfigureConsumer<AuctionCreatedConsumer>(context);
			e.PublishFaults = true; // Enable fault handling for this endpoint.
		});
		cfg.ConfigureEndpoints(context);
	});
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Search search will still start even if the database is not available.
app.Lifetime.ApplicationStarted.Register(async () =>
{
	try
	{
		await DbInitializer.InitDb(app);
	}
	catch (Exception e)
	{
		Console.WriteLine(e);
	}
});


app.Run();
return;

static IAsyncPolicy<HttpResponseMessage> GetPolicy() => HttpPolicyExtensions.HandleTransientHttpError()
	.OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
	.WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(retryAttempt));