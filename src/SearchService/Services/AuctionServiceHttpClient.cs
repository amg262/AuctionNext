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

	public async Task<List<Item>> GetItemsForSearchDb(string? date)
	{
		var client = clientFactory.CreateClient("AuctionService");

		var lastUpdate = await DB.Find<Item, string>()
			.Sort(x => x.Descending(item => item.UpdatedAt))
			.Project(x => x.UpdatedAt.ToString(CultureInfo.CurrentCulture))
			.ExecuteFirstAsync();

		return await _httpClient.GetFromJsonAsync<List<Item>>(_configuration["AuctionService:Url"] +
		                                                      $"api/auctions?date={lastUpdate}");

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