using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Controllers;

/// <summary>
/// Provides functionality for searching items in the database.
/// </summary>
[ApiController]
[Route("api/search")]
public class SearchController : ControllerBase
{
	/// <summary>
	/// Searches items based on the provided search term. If no search term is provided, it returns all items sorted by make.
	/// </summary>
	/// <param name="searchTerm">The search term to filter items. Optional.</param>
	/// <returns>A list of items that match the search criteria.</returns>
	/// <remarks>
	/// This method performs a text search if a search term is provided, leveraging MongoDB's full-text search capabilities.
	/// It returns items sorted by their make in ascending order, and if a search term is provided, results are further sorted by text score to prioritize more relevant matches.
	/// </remarks>
	[HttpGet]
	public async Task<ActionResult<List<Item>>> SearchItems(string? searchTerm)
	{
		// Initialize a query against the items collection.
		var query = DB.Find<Item>();
		// Sort items by the 'Make' field in ascending order.
		query.Sort(x => x.Ascending(a => a.Make));

		// If a search term is provided, perform a full-text search and sort by text score.
		if (!string.IsNullOrEmpty(searchTerm))
		{
			query.Match(Search.Full, searchTerm).SortByTextScore();
		}

		var result = await query.ExecuteAsync();
		// Return the result as an HTTP response.
		return result;
	}
}