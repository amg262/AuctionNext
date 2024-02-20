using AuctionService.Consumers;
using AuctionService.Data;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<AuctionDbContext>(opt =>
{
	opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddMassTransit(x =>
{
	x.AddEntityFrameworkOutbox<AuctionDbContext>(o =>
	{
		o.QueryDelay = TimeSpan.FromSeconds(10);
		o.UsePostgres();
		o.UseBusOutbox();
	});

	x.AddConsumersFromNamespaceContaining<AuctionCreatedFaultConsumer>();
	x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("auction", false));

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

app.UseAuthentication(); // has to be before UseAuthorization
app.UseAuthorization();

app.MapControllers();

try
{
	DbInitializer.InitDb(app);
}
catch (Exception ex)
{
	Console.WriteLine(ex.Message);
}

app.Run();