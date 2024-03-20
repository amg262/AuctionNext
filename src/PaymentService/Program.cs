using MassTransit;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using PaymentService.Consumers;
using PaymentService.Data;
using PaymentService.Services;
using Polly;
using Stripe;

var builder = WebApplication.CreateBuilder(args);
StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];
// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(opt =>
{
	opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddMassTransit(x =>
{
	x.AddEntityFrameworkOutbox<AppDbContext>(o =>
	{
		o.QueryDelay = TimeSpan.FromSeconds(10);

		o.UsePostgres();
		o.UseBusOutbox();
	});

	x.AddConsumersFromNamespaceContaining<PaymentMadeFaultConsumer>();

	x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("payment", false));

	x.UsingRabbitMq((context, cfg) =>
	{
		cfg.UseRetry(r =>
		{
			r.Handle<RabbitMqConnectionException>();
			r.Interval(5, TimeSpan.FromSeconds(10));
		});
		cfg.Host(builder.Configuration["RabbitMq:Host"], "/", host =>
		{
			host.Username(builder.Configuration.GetValue("RabbitMq:Username", "guest"));
			host.Password(builder.Configuration.GetValue("RabbitMq:Password", "guest"));
		});

		cfg.ConfigureEndpoints(context);
	});
});

builder.Services.AddScoped<StripeService>();

builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

//
// app.UseHttpsRedirection();


app.UseAuthorization();

app.MapControllers();

var retryPolicy = Policy.Handle<NpgsqlException>().WaitAndRetry(5, retryAttempt => TimeSpan.FromSeconds(10));
retryPolicy.ExecuteAndCapture(() => DbInitializer.InitDb(app));

app.Run();