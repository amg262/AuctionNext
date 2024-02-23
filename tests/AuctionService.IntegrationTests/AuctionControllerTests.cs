using System.Net;
using System.Net.Http.Json;
using AuctionService.Data;
using AuctionService.DTOs;
using AuctionService.IntegrationTests.Fixtures;
using AuctionService.IntegrationTests.Util;
using Microsoft.Extensions.DependencyInjection;

namespace AuctionService.IntegrationTests;

/// <summary>
/// Integration tests for AuctionController, focusing on API endpoints to ensure they behave as expected under various scenarios.
/// </summary>
public class AuctionControllerTests : IClassFixture<CustomWebAppFactory>, IAsyncLifetime
{
	private readonly CustomWebAppFactory _factory;
	private readonly HttpClient _client;
	private const string GT_ID = "afbee524-5972-4075-8800-7d1f9d7b0a0c";

	/// <summary>
	/// Initializes a new instance of the AuctionControllerTests class using the provided CustomWebAppFactory.
	/// </summary>
	/// <param name="factory">The factory used to create instances of the test server and client.</param>
	public AuctionControllerTests(CustomWebAppFactory factory)
	{
		_factory = factory;
		_client = factory.CreateClient();
	}

	/// <summary>
	/// Placeholder for asynchronous initialization logic.
	/// </summary>
	/// <returns>A task that represents the asynchronous initialization process.</returns>
	public Task InitializeAsync() => Task.CompletedTask;

	/// <summary>
	/// Cleans up the database by reinitializing it to a known state before each test run.
	/// </summary>
	public Task DisposeAsync()
	{
		using var scope = _factory.Services.CreateScope();
		var db = scope.ServiceProvider.GetRequiredService<AuctionDbContext>();
		DbHelper.ReinitDbForTests(db);
		return Task.CompletedTask;
	}

	/// <summary>
	/// Generates a CreateAuctionDto object with predefined test values.
	/// </summary>
	/// <returns>A populated CreateAuctionDto object.</returns>
	private static CreateAuctionDto GetAuctionForCreate()
	{
		return new CreateAuctionDto
		{
			Model = "testModel", Color = "test", Make = "test", ImageUrl = "test", Mileage = 10, Year = 10,
			ReservePrice = 10
		};
	}

	/// <summary>
	/// Tests the GetAuctions endpoint to ensure it returns the expected number of auction DTOs.
	/// </summary>
	[Fact]
	public async Task GetAuctions_ShouldReturn3Auctions()
	{
		// Act
		var response = await _client.GetFromJsonAsync<List<AuctionDto>>("/api/auctions");

		// Assert
		Assert.Equal(3, response.Count);
	}

	/// <summary>
	/// Tests the GetAuctionById endpoint with a valid ID, expecting to successfully retrieve the auction DTO.
	/// </summary>
	[Fact]
	public async Task GetAuctionById_WithValidId_ShouldReturnAuction()
	{
		// Act
		var response = await _client.GetFromJsonAsync<AuctionDto>($"/api/auctions/{GT_ID}");

		// Assert
		Assert.Equal("GT", response.Model);
	}

	/// <summary>
	/// Tests the GetAuctionById endpoint with an invalid ID, expecting a 404 Not Found response.
	/// </summary>
	[Fact]
	public async Task GetAuctionById_WithInValidId_ShouldReturn404()
	{
		// Act
		var response = await _client.GetAsync($"/api/auctions/{Guid.NewGuid()}");

		// Assert
		Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
	}

	/// <summary>
	/// Tests the GetAuctionById endpoint with an invalid GUID format, expecting a 400 Bad Request response.
	/// </summary>
	[Fact]
	public async Task GetAuctionById_WithInValidGuid_ShouldReturn400()
	{
		// Act
		var response = await _client.GetAsync($"/api/auctions/notaguid");

		// Assert
		Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
	}

	/// <summary>
	/// Tests the CreateAuction endpoint without authentication, expecting a 401 Unauthorized response.
	/// </summary>
	[Fact]
	public async Task CreateAuction_WithNoAuth_ShouldReturn401()
	{
		// Arrange
		var auction = new CreateAuctionDto
		{
			Model = "Test",
			Make = "Test",
			Year = 2022,
			ReservePrice = 1000,
		};

		// Act
		var response = await _client.PostAsJsonAsync($"/api/auctions", auction);

		// Assert
		Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
	}

	/// <summary>
	/// Tests the CreateAuction endpoint with authentication, expecting a 201 Created response and verifying the seller name matches.
	/// </summary>
	[Fact]
	public async Task CreateAuction_WithAuth_ShouldReturn201()
	{
		// Arrange
		var auction = GetAuctionForCreate();
		_client.SetFakeJwtBearerToken(AuthHelper.GetBearerForUser("bob"));

		// Actp
		var response = await _client.PostAsJsonAsync($"/api/auctions", auction);

		// Assert
		response.EnsureSuccessStatusCode();
		Assert.Equal(HttpStatusCode.Created, response.StatusCode);
		var createdAuction = await response.Content.ReadFromJsonAsync<AuctionDto>();
		Assert.Equal("bob", createdAuction.Seller);
	}

	[Fact]
	public async Task CreateAuction_WithInvalidCreateAuctionDto_ShouldReturn400()
	{
		// Arrange
		var auction = GetAuctionForCreate();
		_client.SetFakeJwtBearerToken(AuthHelper.GetBearerForUser("bob"));
		auction.Model = null;

		// act
		var response = await _client.PostAsJsonAsync($"/api/auctions", auction);

		// assert
		Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
	}

	[Fact]
	public async Task UpdateAuction_WithValidUpdateDtoAndUser_ShouldReturn200()
	{
		// Arrange
		var updateAuction = new UpdateAuctionDto()
			{Make = "UpdatedMake", Model = "UpdatedModel", Color = "UpdatedColor", Mileage = 100, Year = 2024};
		_client.SetFakeJwtBearerToken(AuthHelper.GetBearerForUser("bob"));

		// act
		var response = await _client.PutAsJsonAsync($"/api/auctions/{GT_ID}", updateAuction);

		// assert
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
	}

	[Fact]
	public async Task UpdateAuction_WithValidUpdateDtoAndInvalidUser_ShouldReturn403()
	{
		// Arrange
		var updateAuction = new UpdateAuctionDto()
			{Make = "UpdatedMake", Model = "UpdatedModel", Color = "UpdatedColor", Mileage = 100, Year = 2024};
		_client.SetFakeJwtBearerToken(AuthHelper.GetBearerForUser("not-bob"));

		// act
		var response = await _client.PutAsJsonAsync($"/api/auctions/{GT_ID}", updateAuction);

		// assert
		Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
	}
}