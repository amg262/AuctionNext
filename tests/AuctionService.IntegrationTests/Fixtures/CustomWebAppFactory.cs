using AuctionService.Data;
using AuctionService.IntegrationTests.Util;
using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;
using WebMotions.Fake.Authentication.JwtBearer;

namespace AuctionService.IntegrationTests.Fixtures;

/// <summary>
/// Custom WebApplicationFactory for integration testing, configured to use a PostgreSQL test container and fake JWT authentication.
/// </summary>
/// <remarks>
/// This factory modifies the application's startup process for integration tests, allowing tests to run
/// against a PostgreSQL database in a Docker container and utilize fake JWT tokens for authentication. This setup ensures
/// that the tests have a consistent and isolated environment, mimicking production configurations while avoiding
/// any changes to the actual database or authentication system.
/// 
/// The MassTransit test harness is also configured here to facilitate testing of application components that interact
/// with message brokers or queues.
/// </remarks>
public class CustomWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
	private readonly PostgreSqlContainer _postgreSqlContainer = new PostgreSqlBuilder().Build();

	/// <summary>
	/// Initializes the PostgreSQL test container asynchronously before running integration tests.
	/// </summary>
	public async Task InitializeAsync()
	{
		await _postgreSqlContainer.StartAsync();
	}

	/// <summary>
	/// Configures the web host for integration tests, including setting up the test database, authentication, and messaging components.
	/// </summary>
	/// <param name="builder">The IWebHostBuilder to configure.</param>
	protected override void ConfigureWebHost(IWebHostBuilder builder)
	{
		builder.ConfigureTestServices(services =>
		{
			services.RemoveDbContext<AuctionDbContext>();
			services.AddDbContext<AuctionDbContext>(options =>
			{
				options.UseNpgsql(_postgreSqlContainer.GetConnectionString());
			});
			services.AddMassTransitTestHarness();
			services.EnsureCreated<AuctionDbContext>();
			services.AddAuthentication(FakeJwtBearerDefaults.AuthenticationScheme)
				.AddFakeJwtBearer(opt => { opt.BearerValueType = FakeJwtBearerBearerValueType.Jwt; });
		});
	}

	/// <summary>
	/// Disposes of the PostgreSQL test container asynchronously after tests are completed.
	/// </summary>
	public new Task DisposeAsync() => _postgreSqlContainer.DisposeAsync().AsTask();
}

internal class PostgresSqlContainer
{
}