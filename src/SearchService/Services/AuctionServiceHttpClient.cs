using System.Globalization;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Services;

public class AuctionServiceHttpClient(
	HttpClient httpClient,
	IHttpClientFactory clientFactory,
	IConfiguration configuration)
{
	private readonly HttpClient _httpClient = httpClient;
	private readonly IConfiguration _configuration = configuration;

	public async Task<List<Item>> GetItemsForSearchDb(string date = null)
	{
		var client = clientFactory.CreateClient("AuctionService");

		var lastUpdated = await DB.Find<Item, string>()
			.Sort(x => x.Descending(x => x.UpdatedAt))
			.Project(x => x.UpdatedAt.ToString())
			.ExecuteFirstAsync();

		return await _httpClient.GetFromJsonAsync<List<Item>>(_configuration["AuctionServiceUrl"] +
		                                                      $"api/auctions?date={lastUpdated}");

		// var client = httpClientFactory.CreateClient("AuctionService");
		// var response = await client.GetAsync($"api/auctions?date={date}");
		// if (response.IsSuccessStatusCode)
		// {
		// 	var auctions = await response.Content.ReadFromJsonAsync<List<AuctionDto>>();
		// 	return auctions;
		// }
		// else
		// {
		// 	logger.LogError("Failed to retrieve auctions from AuctionService. Status code: {StatusCode}", response.StatusCode);
		// 	return null;
		// }
	}
}