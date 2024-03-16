using Microsoft.EntityFrameworkCore;
using PaymentService.Entities;

namespace PaymentService.Data;

/// <summary>
/// Provides functionality to initialize the database with seed data.
/// </summary>
public static class DbInitializer
{
	/// <summary>
	/// Initializes the database with seed data if it's empty.
	/// This method should be called at application startup.
	/// </summary>
	/// <param name="app">The running WebApplication instance to access services.</param>
	public static void InitDb(WebApplication app)
	{
		using var scope = app.Services.CreateScope();
		SeedData(scope.ServiceProvider.GetService<AppDbContext>());
	}

	/// <summary>
	/// Seeds initial data into the AppDbContext if it is empty.
	/// Applies any pending migrations and seeds the Payments table with initial data.
	/// </summary>
	/// <param name="context">The database context instance for accessing and manipulating the database.</param>
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