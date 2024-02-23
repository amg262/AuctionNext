using AuctionService.Data;
using AuctionService.IntegrationTests.Fixtures;
using Microsoft.Extensions.DependencyInjection;

namespace AuctionService.IntegrationTests.Util;

/// <summary>
/// Provides a base class for integration tests within the AuctionService project, 
/// handling setup and teardown processes using a shared CustomWebAppFactory.
/// </summary>
[Collection("Shared collection")]
public abstract class BaseTest : IClassFixture<CustomWebAppFactory>, IAsyncLifetime
{
	private readonly CustomWebAppFactory _factory;
	protected readonly HttpClient Client;

	/// <summary>
	/// Initializes a new instance of the BaseTest class, setting up the necessary test environment.
	/// </summary>
	/// <param name="factory">The CustomWebAppFactory used to create a test server and HTTP client for integration tests.</param>
	protected BaseTest(CustomWebAppFactory factory)
	{
		_factory = factory;
		Client = factory.CreateClient();
	}

	/// <summary>
	/// Performs initial setup before each asynchronous test execution.
	/// Override this method in derived classes to implement custom initialization logic.
	/// </summary>
	public virtual Task InitializeAsync() => Task.CompletedTask;

	/// <summary>
	/// Cleans up resources and reinitializes the database to a known state after tests are executed.
	/// Override this method in derived classes to implement custom teardown logic.
	/// </summary>
	public virtual Task DisposeAsync()
	{
		using var scope = _factory.Services.CreateScope();
		var db = scope.ServiceProvider.GetRequiredService<AuctionDbContext>();
		DbHelper.ReinitDbForTests(db);
		return Task.CompletedTask;
	}
}