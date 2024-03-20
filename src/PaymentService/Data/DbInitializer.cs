using Microsoft.EntityFrameworkCore;
using PaymentService.Entities;
using PaymentService.Services;

namespace PaymentService.Data;

/// <summary>
/// Static class to provide functionality for initializing the application's database with seed data.
/// This includes seeding payments, rewards, and coupons.
/// </summary>
public static class DbInitializer
{
	// A flag to indicate if new data has been added during the seeding process to avoid unnecessary save operations.
	private static bool _hasNewData = false;

	/// <summary>
	/// Initializes the database with seed data if it's empty.
	/// This method should be called at application startup.
	/// </summary>
	/// <param name="app">The running WebApplication instance to access services.</param>
	public static void InitDb(WebApplication app)
	{
		using var scope = app.Services.CreateScope();
		var stripeService = scope.ServiceProvider.GetService<StripeService>();
		SeedData(scope.ServiceProvider.GetService<AppDbContext>(), stripeService);
	}

	/// <summary>
	/// Seeds payments into the database if none exist.
	/// </summary>
	/// <param name="context">The application's database context.</param>
	private static void PopulatePayments(AppDbContext context)
	{
		if (context.Payments.Any())
		{
			Console.WriteLine("Already have data - no need to seed");
			return;
		}

		Console.WriteLine("=== Seeding Payments ===");
		var payments = new List<Payment>()
		{
			new()
			{
				Id = Guid.Parse("a32cc985-f0d1-4569-baa0-36f02eca62c0"),
				StripeSessionId = "stripe-id-1",
			},
			new()
			{
				Id = Guid.Parse("39ba17e3-23ae-441c-962c-ba17b65066da"),
				StripeSessionId = "stripe-id-2",
			},
		};

		context.Payments.AddRange(payments);
		_hasNewData = true;
	}

	/// <summary>
	/// Seeds rewards into the database if none exist.
	/// </summary>
	/// <param name="context">The application's database context.</param>
	private static void PopulateRewards(AppDbContext context)
	{
		if (context.Rewards.Any())
		{
			Console.WriteLine("Already have data - no need to seed");
			return;
		}

		Console.WriteLine("=== Seeding Rewards ===");
		var rewards = new List<Reward>()
		{
			new()
			{
				UserId = "bob",
				RewardsActivity = 1000,
				PaymentId = Guid.Parse("a32cc985-f0d1-4569-baa0-36f02eca62c0"),
			},
			new()
			{
				UserId = "alice",
				RewardsActivity = 100,
				PaymentId = Guid.Parse("39ba17e3-23ae-441c-962c-ba17b65066da"),
			},
		};
		Console.WriteLine($"Added {rewards.Count} rewards");

		context.Rewards.AddRange(rewards);
		_hasNewData = true;
	}

	/// <summary>
	/// Seeds coupons into the database, fetching them from Stripe if none exist.
	/// </summary>
	/// <param name="context">The application's database context.</param>
	/// <param name="stripeService">The service used to interact with Stripe's API for fetching coupons.</param>
	private static void PopulateCoupons(AppDbContext context, StripeService stripeService)
	{
		if (context.Coupons.Any())
		{
			Console.WriteLine("Already have data - no need to seed");
			return;
		}

		Console.WriteLine("=== Seeding Coupons ===");
		var stripeCoupons = stripeService.SyncStripeCoupons().Result;
		Console.WriteLine($"Found and added {stripeCoupons.Count()} coupons");
		_hasNewData = true;
	}

	/// <summary>
	/// Orchestrates the seeding of data into the database by calling individual seed methods for payments, rewards, and coupons.
	/// Applies any pending migrations before seeding.
	/// </summary>
	/// <param name="context">The application's database context.</param>
	/// <param name="stripeService">The service used to interact with Stripe's API for fetching coupons.</param>
	private static void SeedData(AppDbContext context, StripeService stripeService)
	{
		context.Database.Migrate();
		PopulatePayments(context);
		PopulateRewards(context);
		PopulateCoupons(context, stripeService);

		if (_hasNewData) context.SaveChanges();
	}
}