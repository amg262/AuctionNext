using BiddingService.Consumers;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using MongoDB.Driver;
using MongoDB.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddMassTransit(x =>
{
	// x.AddEntityFrameworkOutbox<AuctionDbContext>(o =>
	// {
	// 	o.QueryDelay = TimeSpan.FromSeconds(10);
	// 	o.UsePostgres();
	// 	o.UseBusOutbox();
	// });

	x.AddConsumersFromNamespaceContaining<AuctionCreatedConsumer>();
	x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("bids", false));

	x.UsingRabbitMq((context, cfg) =>
	{
		cfg.Host(builder.Configuration["RabbitMq:Host"], "/", h =>
		{
			h.Username(builder.Configuration.GetValue("RabbitMq:Username", "guest"));
			h.Password(builder.Configuration.GetValue("RabbitMq:Password", "guest"));
		});
		cfg.ConfigureEndpoints(context);
	});
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
{
	opt.Authority = builder.Configuration["IdentityServiceUrl"];
	opt.RequireHttpsMetadata = false;
	opt.TokenValidationParameters.ValidateAudience = false;
	opt.TokenValidationParameters.NameClaimType = "username";
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

await DB.InitAsync("BidDb",
	MongoClientSettings.FromConnectionString(builder.Configuration.GetConnectionString("DefaultConnection")));

app.Run();