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
[Collection("Shared collection")]
public class AuctionControllerTests : IAsyncLifetime // BaseTest
{
	private readonly CustomWebAppFactory _factory;
	private readonly HttpClient _httpClient;
	private const string GT_ID = "afbee524-5972-4075-8800-7d1f9d7b0a0c";

	/// <summary>
	/// Initializes a new instance of the AuctionControllerTests class using the provided CustomWebAppFactory.
	/// </summary>
	/// <param name="factory">The factory used to create instances of the test server and client.</param>
	public AuctionControllerTests(CustomWebAppFactory factory) // : base(factory)
	{
		_factory = factory;
		_httpClient = factory.CreateClient();
	}

	/// <summary>
	/// Tests the GetAuctions endpoint to ensure it returns the expected number of auction DTOs.
	/// </summary>
	[Fact]
	public async Task GetAuctions_ShouldReturn3Auctions()
	{
		// Act
		var response = await _httpClient.GetFromJsonAsync<List<AuctionDto>>("/api/auctions");

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
		var response = await _httpClient.GetFromJsonAsync<AuctionDto>($"/api/auctions/{GT_ID}");

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
		var response = await _httpClient.GetAsync($"/api/auctions/{Guid.NewGuid()}");

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
		var response = await _httpClient.GetAsync($"/api/auctions/notaguid");

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
		var response = await _httpClient.PostAsJsonAsync($"/api/auctions", auction);

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
		var auction = DbHelper.GetAuctionForCreate();
		_httpClient.SetFakeJwtBearerToken(AuthHelper.GetBearerForUser("bob"));

		// Actp
		var response = await _httpClient.PostAsJsonAsync($"/api/auctions", auction);

		// Assert
		response.EnsureSuccessStatusCode();
		Assert.Equal(HttpStatusCode.Created, response.StatusCode);
		var createdAuction = await response.Content.ReadFromJsonAsync<AuctionDto>();
		Assert.Equal("bob", createdAuction.Seller);
	}

	/// <summary>
	/// Tests the CreateAuction endpoint with an invalid CreateAuctionDto, expecting a 400 Bad Request response.
	/// </summary>
	[Fact]
	public async Task CreateAuction_WithInvalidCreateAuctionDto_ShouldReturn400()
	{
		// Arrange
		var auction = DbHelper.GetAuctionForCreate();
		_httpClient.SetFakeJwtBearerToken(AuthHelper.GetBearerForUser("bob"));
		auction.Model = null;

		// act
		var response = await _httpClient.PostAsJsonAsync($"/api/auctions", auction);

		// assert
		Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
	}

	/// <summary>
	/// Tests the UpdateAuction endpoint with a valid UpdateAuctionDto and user, expecting a 200 OK response.
	/// </summary>
	[Fact]
	public async Task UpdateAuction_WithValidUpdateDtoAndUser_ShouldReturn200()
	{
		// Arrange
		var updateAuction = new UpdateAuctionDto()
			{Make = "UpdatedMake", Model = "UpdatedModel", Color = "UpdatedColor", Mileage = 100, Year = 2024};
		_httpClient.SetFakeJwtBearerToken(AuthHelper.GetBearerForUser("bob"));

		// act
		var response = await _httpClient.PutAsJsonAsync($"/api/auctions/{GT_ID}", updateAuction);

		// assert
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
	}

	/// <summary>
	/// Tests the UpdateAuction endpoint with an invalid UpdateAuctionDto, expecting a 400 Bad Request response.
	/// </summary>
	[Fact]
	public async Task UpdateAuction_WithValidUpdateDtoAndInvalidUser_ShouldReturn403()
	{
		// Arrange
		var updateAuction = new UpdateAuctionDto()
			{Make = "UpdatedMake", Model = "UpdatedModel", Color = "UpdatedColor", Mileage = 100, Year = 2024};
		_httpClient.SetFakeJwtBearerToken(AuthHelper.GetBearerForUser("not-bob"));

		// act
		var response = await _httpClient.PutAsJsonAsync($"/api/auctions/{GT_ID}", updateAuction);

		// assert
		Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
	}

	public Task InitializeAsync() => Task.CompletedTask;


	public Task DisposeAsync()
	{
		using var scope = _factory.Services.CreateScope();
		var db = scope.ServiceProvider.GetRequiredService<AuctionDbContext>();
		DbHelper.ReinitDbForTests(db);
		return Task.CompletedTask;
	}
}