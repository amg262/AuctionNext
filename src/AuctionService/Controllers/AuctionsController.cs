using AuctionService.Data;
using AuctionService.DTOs;
using AuctionService.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Controllers;

[ApiController]
[Route("api/auctions")]
public class AuctionsController(AuctionDbContext context, IMapper mapper) : ControllerBase
{
	[HttpGet]
	public async Task<ActionResult<List<AuctionDto>>> GetAllAuctions()
	{
		var auctions = await context.Auctions
			.Include(x => x.Item)
			.OrderBy(x => x.Item.Make)
			.ToListAsync();

		var auctionsDto = mapper.Map<List<AuctionDto>>(auctions);
		return Ok(auctionsDto);
	}

	[HttpGet("{id:guid}")]
	public async Task<ActionResult<AuctionDto>> GetAuctionById(Guid id)
	{
		var auction = await context.Auctions
			.Include(x => x.Item)
			.FirstOrDefaultAsync(x => x.Id == id);

		if (auction is null) return NotFound();

		return mapper.Map<AuctionDto>(auction);
	}

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

	[HttpPut("{id:guid}")]
	public async Task<ActionResult> UpdateAuction(Guid id, UpdateAuctionDto updateAuctionDto)
	{
		var auction = await context.Auctions
			.Include(x => x.Item)
			.FirstOrDefaultAsync(x => x.Id == id);

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
}