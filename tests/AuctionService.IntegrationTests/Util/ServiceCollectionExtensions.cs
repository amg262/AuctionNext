using AuctionService.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AuctionService.IntegrationTests.Util;

/// <summary>
/// Provides extension methods for IServiceCollection to support integration testing scenarios.
/// </summary>
public static class ServiceCollectionExtensions
{
	/// <summary>
	/// Removes the DbContext registration from the services collection. This allows tests to register
	/// a mock or in-memory database context instead of the production database context.
	/// </summary>
	/// <typeparam name="T">The type of the DbContext to remove.</typeparam>
	/// <param name="services">The IServiceCollection from which to remove the DbContext.</param>
	public static void RemoveDbContext<T>(this IServiceCollection services) where T : DbContext
	{
		var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AuctionDbContext>));

		if (descriptor != null) services.Remove(descriptor);
	}

	/// <summary>
	/// Ensures that the database for the DbContext is created and migrates to the latest version. It also
	/// initializes the database with test data.
	/// </summary>
	/// <typeparam name="T">The type of the DbContext.</typeparam>
	/// <param name="services">The IServiceCollection to build the ServiceProvider from.</param>
	/// <remarks>
	/// This method is useful for setting up a test database environment that reflects the current
	/// state of the application's database schema. It should be used with caution if it operates
	/// on a shared or production database.
	/// </remarks>
	public static void EnsureCreated<T>(this IServiceCollection services) where T : DbContext
	{
		var sp = services.BuildServiceProvider();
		using var scope = sp.CreateScope();
		var scopedServices = scope.ServiceProvider;
		var db = scopedServices.GetRequiredService<AuctionDbContext>();
		db.Database.Migrate();
		DbHelper.InitDbForTests(db);
	}
}