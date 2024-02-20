using System.Globalization;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Services;

/// <summary>
/// Represents a client service for fetching auction items from an external Auction Service.
/// Initializes a new instance of the <see cref="AuctionServiceHttpClient"/> class.
/// </summary>
/// <param name="httpClient">The <see cref="HttpClient"/> for making HTTP requests.</param>
/// <param name="config">The application's configuration to access settings like the Auction Service URL.</param>
public class AuctionServiceHttpClient(HttpClient httpClient, IConfiguration config)
{
	/// <summary>
	/// Asynchronously retrieves a list of <see cref="Item"/> objects from the Auction Service.
	/// </summary>
	/// <param name="date">The optional date parameter to filter items by their last updated timestamp. 
	/// If not provided, the method fetches the most recently updated item and uses its timestamp as the filter.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a list of <see cref="Item"/> objects.</returns>
	/// <remarks>
	/// This method fetches items from the Auction Service by making an HTTP GET request. 
	/// It constructs the request URL using the base URL provided in the application's configuration and appends a query parameter
	/// to filter items based on their last updated timestamp. The method uses MongoDB to find the most recent update timestamp
	/// if no specific date is provided.
	/// </remarks>
	public async Task<List<Item>> GetItemsForSearchDb(string date = null)
	{
		var lastUpdated = await DB.Find<Item, string>()
			.Sort(x => x.Descending(x => x.UpdatedAt))
			.Project(x => x.UpdatedAt.ToString())
			.ExecuteFirstAsync();

		return await httpClient.GetFromJsonAsync<List<Item>>(config["AuctionServiceUrl"] +
		                                                     $"/api/auctions?date={lastUpdated}");
	}
}