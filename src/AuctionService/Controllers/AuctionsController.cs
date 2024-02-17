using AuctionService.Data;
using AuctionService.DTOs;
using AuctionService.Entities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Controllers;

/// <summary>
/// Initializes a new instance of the <see cref="AuctionsController"/> class.
/// </summary>
/// <param name="context">The database context for auction data.</param>
/// <param name="mapper">The AutoMapper instance for mapping between entities and DTOs.</param>
[ApiController]
[Route("api/auctions")]
public class AuctionsController(AuctionDbContext context, IMapper mapper) : ControllerBase
{
	/// <summary>
	/// Retrieves all auctions.
	/// </summary>
	/// <returns>A list of auction DTOs.</returns>
	[HttpGet]
	public async Task<ActionResult<List<AuctionDto>>> GetAllAuctions(string? date = null)
	{
		var query = context.Auctions.OrderBy(x => x.Item.Make).AsQueryable();

		if (!string.IsNullOrEmpty(date))
		{
			query = query.Where(x => x.UpdatedAt.CompareTo(DateTime.Parse(date).ToUniversalTime()) > 0);
		}

		return await query.ProjectTo<AuctionDto>(mapper.ConfigurationProvider).ToListAsync();
	}

	/// <summary>
	/// Retrieves a specific auction by ID.
	/// </summary>
	/// <param name="id">The unique identifier of the auction.</param>
	/// <returns>An auction DTO.</returns>
	[HttpGet("{id:guid}")]
	public async Task<ActionResult<AuctionDto>> GetAuctionById(Guid id)
	{
		var auction = await context.Auctions
			.Include(x => x.Item)
			.FirstOrDefaultAsync(x => x.Id == id);

		if (auction is null) return NotFound();

		return mapper.Map<AuctionDto>(auction);
	}

	/// <summary>
	/// Creates a new auction.
	/// </summary>
	/// <param name="auctionDto">The auction data transfer object containing the creation data.</param>
	/// <returns>The created auction DTO.</returns>
	[HttpPost]
	public async Task<ActionResult<AuctionDto>> CreateAuction(CreateAuctionDto auctionDto)
	{
		var auction = mapper.Map<Auction>(auctionDto);
		// Todo: Add the user id from the token
		auction.Seller = "Andrew";
		await context.Auctions.AddAsync(auction);

		// If the number of changes is greater than 0, then the operation was successful
		var result = await context.SaveChangesAsync() > 0;

		if (!result) return BadRequest("Failed to create auction in the database.");

		// This will return the auction that was created via GET request and parameters
		return CreatedAtAction(nameof(GetAuctionById), new {id = auction.Id}, mapper.Map<AuctionDto>(auction));
	}

	/// <summary>
	/// Updates an existing auction.
	/// </summary>
	/// <param name="id">The unique identifier of the auction to update.</param>
	/// <param name="updateAuctionDto">The auction data transfer object containing the update data.</param>
	/// <returns>A response indicating the outcome of the operation.</returns>
	[HttpPut("{id:guid}")]
	public async Task<ActionResult> UpdateAuction(Guid id, UpdateAuctionDto updateAuctionDto)
	{
		var auction = await context.Auctions.Include(x => x.Item).FirstOrDefaultAsync(x => x.Id == id);

		if (auction is null) return NotFound();


		// Todo: check if seller == username
		auction.Item.Make = updateAuctionDto.Make ?? auction.Item.Make;
		auction.Item.Model = updateAuctionDto.Model ?? auction.Item.Model;
		auction.Item.Color = updateAuctionDto.Color ?? auction.Item.Color;
		auction.Item.Mileage = updateAuctionDto.Mileage ?? auction.Item.Mileage;
		auction.Item.Year = updateAuctionDto.Year ?? auction.Item.Year;

		// mapper.Map(updateAuctionDto, auction);
		//
		// context.Auctions.Update(auction);

		var result = await context.SaveChangesAsync() > 0;

		if (result) return Ok();

		return BadRequest("Failed to update auction in the database.");
	}

	/// <summary>
	/// Deletes an auction.
	/// </summary>
	/// <param name="id">The unique identifier of the auction to delete.</param>
	/// <returns>A response indicating the outcome of the operation.</returns>
	[HttpDelete("{id:guid}")]
	public async Task<ActionResult> DeleteAuction(Guid id)
	{
		var auction = await context.Auctions.FindAsync(id);

		if (auction is null) return NotFound();

		context.Auctions.Remove(auction);

		var result = await context.SaveChangesAsync() > 0;

		if (!result) return BadRequest("Failed to delete auction in the database.");

		return Ok();
	}
}