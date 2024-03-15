using Microsoft.EntityFrameworkCore;
using PaymentService.Entities;

namespace PaymentService.Data;

public static class DbInitializer
{
	public static void InitDb(WebApplication app)
	{
		using var scope = app.Services.CreateScope();

		SeedData(scope.ServiceProvider.GetService<AppDbContext>());
	}

	private static void SeedData(AppDbContext context)
	{
		context.Database.Migrate();

		if (context.Payments.Any())
		{
			Console.WriteLine("Already have data - no need to seed");
			return;
		}


		Console.WriteLine("=== Seeding data ===");
		var payments = new List<Payment>()
		{
			new() {StripeSessionId = "stripe-id-1",},
			new() {StripeSessionId = "stripe-id-2",},
		};

		context.Payments.AddRange(payments);

		context.SaveChanges();
	}
}