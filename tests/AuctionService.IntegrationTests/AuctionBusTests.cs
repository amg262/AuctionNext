using System.Net;
using System.Net.Http.Json;
using AuctionService.Data;
using AuctionService.DTOs;
using AuctionService.IntegrationTests.Fixtures;
using AuctionService.IntegrationTests.Util;
using Contracts;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace AuctionService.IntegrationTests;

/// <summary>
/// Integration tests focused on verifying the interaction between the Auction service and its message bus,
/// specifically testing event publishing behavior when auction-related actions are performed.
/// </summary>
[Collection("Shared collection")]
public class AuctionBusTests : IAsyncLifetime
{
	private readonly CustomWebAppFactory _factory;
	private readonly HttpClient _httpClient;
	private readonly ITestHarness _testHarness;

	/// <summary>
	/// Initializes a new instance of the AuctionBusTests class with dependencies injected through the CustomWebAppFactory.
	/// </summary>
	/// <param name="factory">A factory for creating instances of the test server and HTTP client, and for accessing service dependencies.</param>
	public AuctionBusTests(CustomWebAppFactory factory)
	{
		_factory = factory;
		_httpClient = factory.CreateClient();
		_testHarness = factory.Services.GetTestHarness();
	}

	/// <summary>
	/// Tests that creating a valid auction results in the publishing of an AuctionCreated event on the service bus.
	/// </summary>
	[Fact]
	public async Task CreateAuction_WithValidObject_ShouldPublishAuctionCreated()
	{
		// arrange
		var auction = DbHelper.GetAuctionForCreate();
		_httpClient.SetFakeJwtBearerToken(AuthHelper.GetBearerForUser("bob"));

		// act
		var response = await _httpClient.PostAsJsonAsync("api/auctions", auction);

		// assert
		response.EnsureSuccessStatusCode();
		Assert.True(await _testHarness.Published.Any<AuctionCreated>());
	}

	/// <summary>
	/// Placeholder for asynchronous initialization logic before each test method is executed.
	/// </summary>
	public Task InitializeAsync() => Task.CompletedTask;

	/// <summary>
	/// Cleans up resources after tests are executed, specifically reinitializing the database to a known clean state.
	/// </summary>
	public Task DisposeAsync()
	{
		using var scope = _factory.Services.CreateScope();
		var db = scope.ServiceProvider.GetRequiredService<AuctionDbContext>();
		DbHelper.ReinitDbForTests(db);
		return Task.CompletedTask;
	}
}