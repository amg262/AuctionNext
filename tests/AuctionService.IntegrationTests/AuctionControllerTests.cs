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
public class AuctionControllerTests : IAsyncLifetime
{
	private readonly CustomWebAppFactory _factory;
	private readonly HttpClient _httpClient;
	private const string GtId = "afbee524-5972-4075-8800-7d1f9d7b0a0c";

	/// <summary>
	/// Initializes a new instance of the AuctionControllerTests class using the provided CustomWebAppFactory.
	/// </summary>
	/// <param name="factory">The factory used to create instances of the test server and client.</param>
	public AuctionControllerTests(CustomWebAppFactory factory)
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
		// arrange? 

		// act
		var response = await _httpClient.GetFromJsonAsync<List<AuctionDto>>("api/auctions");

		// assert
		Assert.Equal(3, response.Count);
	}

	/// <summary>
	/// Tests the GetAuctionById endpoint with a valid ID, expecting to successfully retrieve the auction DTO.
	/// </summary>
	[Fact]
	public async Task GetAuctionById_WithValidId_ShouldReturnAuction()
	{
		// arrange? 

		// act
		var response = await _httpClient.GetFromJsonAsync<AuctionDto>($"api/auctions/{GtId}");

		// assert
		Assert.Equal("GT", response.Model);
	}

	/// <summary>
	/// Tests the GetAuctionById endpoint with an invalid ID, expecting a 404 Not Found response.
	/// </summary>
	[Fact]
	public async Task GetAuctionById_WithInvalidId_ShouldReturn404()
	{
		// arrange? 

		// act
		var response = await _httpClient.GetAsync($"api/auctions/{Guid.NewGuid()}");

		// assert
		Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
	}

	/// <summary>
	/// Tests the GetAuctionById endpoint with an invalid GUID format, expecting a 400 Bad Request response.
	/// </summary>
	[Fact]
	public async Task GetAuctionById_WithInvalidGuid_ShouldReturn400()
	{
		// arrange? 

		// act
		var response = await _httpClient.GetAsync($"api/auctions/notaguid");

		// assert
		Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
	}

	/// <summary>
	/// Tests the CreateAuction endpoint without authentication, expecting a 401 Unauthorized response.
	/// </summary>
	[Fact]
	public async Task CreateAuction_WithNoAuth_ShouldReturn401()
	{
		// arrange? 
		var auction = new CreateAuctionDto {Make = "test"};

		// act
		var response = await _httpClient.PostAsJsonAsync($"api/auctions", auction);

		// assert
		Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
	}

	/// <summary>
	/// Tests the CreateAuction endpoint with authentication, expecting a 201 Created response and verifying the seller name matches.
	/// </summary>
	[Fact]
	public async Task CreateAuction_WithAuth_ShouldReturn201()
	{
		// arrange? 
		var auction = GetAuctionForCreate();
		_httpClient.SetFakeJwtBearerToken(AuthHelper.GetBearerForUser("bob"));

		// act
		var response = await _httpClient.PostAsJsonAsync($"api/auctions", auction);

		// assert
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
		// arrange? 
		var auction = GetAuctionForCreate();
		auction.Make = null;
		_httpClient.SetFakeJwtBearerToken(AuthHelper.GetBearerForUser("bob"));

		// act
		var response = await _httpClient.PostAsJsonAsync($"api/auctions", auction);

		// assert
		Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
	}

	/// <summary>
	/// Tests the UpdateAuction endpoint with a valid UpdateAuctionDto and user, expecting a 200 OK response.
	/// </summary>
	[Fact]
	public async Task UpdateAuction_WithValidUpdateDtoAndUser_ShouldReturn200()
	{
		// arrange? 
		var updateAuction = new UpdateAuctionDto {Make = "Updated"};
		_httpClient.SetFakeJwtBearerToken(AuthHelper.GetBearerForUser("bob"));

		// act
		var response = await _httpClient.PutAsJsonAsync($"api/auctions/{GtId}", updateAuction);

		// assert
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
	}

	/// <summary>
	/// Tests the UpdateAuction endpoint with an invalid UpdateAuctionDto, expecting a 400 Bad Request response.
	/// </summary>
	[Fact]
	public async Task UpdateAuction_WithValidUpdateDtoAndInvalidUser_ShouldReturn403()
	{
		// arrange? 
		var updateAuction = new UpdateAuctionDto {Make = "Updated"};
		_httpClient.SetFakeJwtBearerToken(AuthHelper.GetBearerForUser("notbob"));

		// act
		var response = await _httpClient.PutAsJsonAsync($"api/auctions/{GtId}", updateAuction);

		// assert
		Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
	}

	/// <summary>
	/// Placeholder for asynchronous initialization logic before each test method is executed.
	/// </summary>
	/// <returns>A completed task</returns>
	public Task InitializeAsync() => Task.CompletedTask;

	/// <summary>
	/// Cleans up resources after tests are executed, specifically reinitializing the database to a known clean state.
	/// </summary>
	/// <returns>A completed task</returns>
	public Task DisposeAsync()
	{
		using var scope = _factory.Services.CreateScope();
		var db = scope.ServiceProvider.GetRequiredService<AuctionDbContext>();
		DbHelper.ReinitDbForTests(db);
		return Task.CompletedTask;
	}

	/// <summary>
	/// Returns a new CreateAuctionDto object for use in tests.
	/// </summary>
	/// <returns>A new CreatedAuctionDto object</returns>
	private static CreateAuctionDto GetAuctionForCreate()
	{
		return new CreateAuctionDto
		{
			Make = "test",
			Model = "testModel",
			ImageUrl = "test",
			Color = "test",
			Mileage = 10,
			Year = 10,
			ReservePrice = 10
		};
	}
}