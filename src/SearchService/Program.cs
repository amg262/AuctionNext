using MassTransit;
using Polly;
using Polly.Extensions.Http;
using SearchService.Data;
using SearchService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpClient<AuctionServiceHttpClient>().AddPolicyHandler(GetPolicy());
builder.Services.AddMassTransit(x => { x.UsingRabbitMq((context, cfg) => { cfg.ConfigureEndpoints(context); }); });
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