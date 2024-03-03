using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;
using SearchService.Models;
using SearchService.RequestHelpers;

namespace SearchService.Controllers;

/// <summary>
/// Provides functionality for searching items in the database.
/// </summary>
[ApiController, Route("api/search")]
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
		var query = DB.PagedSearch<Item, Item>();

		if (!string.IsNullOrEmpty(searchParams.SearchTerm))
		{
			query.Match(Search.Full, searchParams.SearchTerm).SortByTextScore();
		}

		query = searchParams.OrderBy switch
		{
			"make" => query.Sort(x => x.Ascending(a => a.Make))
				.Sort(x => x.Ascending(a => a.Model)),
			"new" => query.Sort(x => x.Descending(a => a.CreatedAt)),
			_ => query.Sort(x => x.Ascending(a => a.AuctionEnd))
		};

		query = searchParams.FilterBy switch
		{
			"finished" => query.Match(x => x.AuctionEnd < DateTime.UtcNow),
			"endingSoon" => query.Match(x => x.AuctionEnd < DateTime.UtcNow.AddHours(6)
			                                 && x.AuctionEnd > DateTime.UtcNow),
			_ => query.Match(x => x.AuctionEnd > DateTime.UtcNow)
		};

		if (!string.IsNullOrEmpty(searchParams.Seller))
		{
			query.Match(x => x.Seller == searchParams.Seller);
		}

		if (!string.IsNullOrEmpty(searchParams.Winner))
		{
			query.Match(x => x.Winner == searchParams.Winner);
		}

		query.PageNumber(searchParams.PageNumber);
		query.PageSize(searchParams.PageSize);

		var result = await query.ExecuteAsync();

		return Ok(new
		{
			results = result.Results,
			pageCount = result.PageCount,
			totalCount = result.TotalCount
		});
	}
}