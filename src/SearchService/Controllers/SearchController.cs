using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;
using SearchService.Models;
using SearchService.RequestHelpers;

namespace SearchService.Controllers;

/// <summary>
/// Provides functionality for searching items in the database.
/// </summary>
[ApiController]
[Route("api/search")]
public class SearchController : ControllerBase
{
	/// <summary>
	/// Searches items based on the provided search parameters. Allows filtering, ordering, and pagination of search results.
	/// </summary>
	/// <param name="searchParams">The search parameters to filter and order items.</param>
	/// <returns>A list of items that match the search criteria along with pagination details.</returns>
	/// <remarks>
	/// Utilizes MongoDB's text search and filtering capabilities to find items matching the criteria specified in <paramref name="searchParams"/>.
	/// Supports ordering by 'make', 'new', or 'auction end', and filtering by status such as 'finished' or 'ending soon'.
	/// </remarks>
	[HttpGet]
	public async Task<ActionResult<List<Item>>> SearchItems([FromQuery] SearchParams searchParams)
	{
		// Initialize a query against the items collection.
		var query = DB.PagedSearch<Item, Item>();

		// Sort items by the 'Make' field in ascending order.
		query.Sort(x => x.Ascending(a => a.Make));

		// If a search term is provided, perform a full-text search and sort by text score.
		if (!string.IsNullOrEmpty(searchParams.SearchTerm))
		{
			query.Match(Search.Full, searchParams.SearchTerm).SortByTextScore();
		}

		// Apply ordering based on the 'OrderBy' parameter.
		query = searchParams.OrderBy switch
		{
			"make" => query.Sort(x => x.Ascending(a => a.Make))
				.Sort(x => x.Ascending(a => a.Model)),
			"new" => query.Sort(x => x.Descending(a => a.CreatedAt)),
			_ => query.Sort(x => x.Ascending(a => a.AuctionEnd))
		};

		// Apply filtering based on the 'FilterBy' parameter.
		query = searchParams.FilterBy switch
		{
			"finished" => query.Match(x => x.AuctionEnd < DateTime.UtcNow),
			"endingSoon" => query.Match(x =>
				x.AuctionEnd < DateTime.UtcNow.AddHours(6) && x.AuctionEnd > DateTime.UtcNow),
			_ => query.Match(x => x.AuctionEnd > DateTime.UtcNow)
		};

		// Apply additional filtering by seller or winner if provided.
		if (!string.IsNullOrEmpty(searchParams.Seller))
		{
			query.Match(x => x.Where(a => a.Seller == searchParams.Seller));
		}

		// Apply additional filtering by seller or winner if provided.
		if (!string.IsNullOrEmpty(searchParams.Winner))
		{
			query.Match(x => x.Where(a => a.Winner == searchParams.Winner));
		}

		// Set the pagination parameters.
		query.PageNumber(searchParams.PageNumber);
		query.PageSize(searchParams.PageSize);

		var result = await query.ExecuteAsync();

		// Construct and return the response with search results and pagination details.
		return Ok(new
		{
			results = result.Results,
			pageCount = result.PageCount,
			totalCount = result.TotalCount
		});
	}
}