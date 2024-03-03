using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Services;

/// <summary>
/// Provides functionality to interact with the Auction service via HTTP, specifically for retrieving items to update the search database.
/// </summary>
public class AuctionSvcHttpClient
{
	private readonly HttpClient _httpClient;
	private readonly IConfiguration _config;

	/// <summary>
	/// Initializes a new instance of the <see cref="AuctionSvcHttpClient"/> class with a pre-configured HttpClient and application configuration.
	/// </summary>
	/// <param name="httpClient">The HttpClient used for making HTTP requests to the Auction service.</param>
	/// <param name="config">The application configuration where the Auction service URL is specified.</param>
	public AuctionSvcHttpClient(HttpClient httpClient, IConfiguration config)
	{
		_httpClient = httpClient;
		_config = config;
	}

	/// <summary>
	/// Retrieves a list of items from the Auction service that need to be updated in the search database.
	/// </summary>
	/// <returns>A task that represents the asynchronous operation and contains a list of items to be updated.</returns>
	/// <remarks>
	/// This method first queries the local MongoDB to find the last updated timestamp among the stored items.
	/// It then uses this timestamp to request from the Auction service only those items that have been updated since.
	/// This efficient approach minimizes the volume of data transferred and processed, focusing only on updates.
	/// </remarks>
	public async Task<List<Item>> GetItemsForSearchDb()
	{
		// Query the local MongoDB for the most recent update timestamp among the items.
		var lastUpdated = await DB.Find<Item, string>()
			.Sort(x => x.Descending(x => x.UpdatedAt))
			.Project(x => x.UpdatedAt.ToString())
			.ExecuteFirstAsync();

		// Use the lastUpdated timestamp to request updated items from the Auction service.
		return await _httpClient.GetFromJsonAsync<List<Item>>(_config["AuctionServiceUrl"]
		                                                      + "/api/auctions?date=" + lastUpdated);
	}
}