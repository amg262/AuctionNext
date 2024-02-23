using AuctionService.Data;
using AuctionService.DTOs;
using AuctionService.Entities;
using AutoMapper;
using Contracts;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace AuctionService.Controllers;

/// <summary>
/// Handles HTTP requests related to auctions, providing CRUD operations and more.
/// </summary>
[ApiController]
[Route("api/auctions")]
public class AuctionsController : ControllerBase
{
	private readonly IAuctionRepository _repo;
	private readonly IMapper _mapper;
	private readonly IPublishEndpoint _publishEndpoint;

	/// <summary>
	/// Initializes a new instance of the AuctionsController class.
	/// </summary>
	/// <param name="repo">The auction repository for database operations.</param>
	/// <param name="mapper">The AutoMapper instance for object transformation.</param>
	/// <param name="publishEndpoint">The MassTransit publish endpoint for messaging.</param>
	public AuctionsController(IAuctionRepository repo, IMapper mapper, IPublishEndpoint publishEndpoint)
	{
		_repo = repo;
		_mapper = mapper;
		_publishEndpoint = publishEndpoint;
	}


	/// <summary>
	/// Retrieves all auctions.
	/// </summary>
	/// <returns>A list of auction DTOs.</returns>
	[HttpGet]
	public async Task<ActionResult<List<AuctionDto>>> GetAllAuctions(string? date = null)
	{
		return await _repo.GetAuctionsAsync(date);
	}

	/// <summary>
	/// Retrieves a specific auction by ID.
	/// </summary>
	/// <param name="id">The unique identifier of the auction.</param>
	/// <returns>An auction DTO.</returns>
	[HttpGet("{id}")]
	// [HttpGet("{id:guid}")]
	public async Task<ActionResult<AuctionDto>> GetAuctionById(Guid id)
	{
		var auction = await _repo.GetAuctionByIdAsync(id);

		if (auction is null) return NotFound();

		return auction;
		return _mapper.Map<AuctionDto>(auction);
	}

	/// <summary>
	/// Creates a new auction.
	/// </summary>
	/// <param name="auctionDto">The auction data transfer object containing the creation data.</param>
	/// <returns>The created auction DTO.</returns>
	[HttpPost]
	[Authorize]
	public async Task<ActionResult<AuctionDto>> CreateAuction(CreateAuctionDto auctionDto)
	{
		var auction = _mapper.Map<Auction>(auctionDto);

		auction.Seller = User.Identity.Name ?? "unknown";

		_repo.AddAuction(auction);

		var newAuction = _mapper.Map<AuctionDto>(auction);

		await _publishEndpoint.Publish(_mapper.Map<AuctionCreated>(newAuction));

		// If the number of changes is greater than 0, then the operation was successful
		var result = await _repo.SaveChangesAsync();

		if (!result) return BadRequest("Failed to create auction in the database.");

		// This will return the auction that was created via GET request and parameters
		return CreatedAtAction(nameof(GetAuctionById), new {id = auction.Id}, newAuction);
	}

	/// <summary>
	/// Updates an existing auction.
	/// </summary>
	/// <param name="id">The unique identifier of the auction to update.</param>
	/// <param name="updateAuctionDto">The auction data transfer object containing the update data.</param>
	/// <returns>A response indicating the outcome of the operation.</returns>
	[HttpPut("{id:guid}")]
	[Authorize]
	public async Task<ActionResult> UpdateAuction(Guid id, UpdateAuctionDto updateAuctionDto)
	{
		var auction = await _repo.GetAuctionEntityById(id);

		if (auction is null) return NotFound();

		if (auction.Seller != User.Identity.Name) return Forbid(); // 403 Forbidden

		auction.Item.Make = updateAuctionDto.Make ?? auction.Item.Make;
		auction.Item.Model = updateAuctionDto.Model ?? auction.Item.Model;
		auction.Item.Color = updateAuctionDto.Color ?? auction.Item.Color;
		auction.Item.Mileage = updateAuctionDto.Mileage ?? auction.Item.Mileage;
		auction.Item.Year = updateAuctionDto.Year ?? auction.Item.Year;

		// _mapper.Map(updateAuctionDto, auction);
		//
		// context.Auctions.Update(auction);

		// We have to call the Publish method to send the message to the message broker before saving the changes to the database.
		await _publishEndpoint.Publish(_mapper.Map<AuctionUpdated>(auction));


		var result = await _repo.SaveChangesAsync();

		if (result) return Ok();

		return BadRequest("Failed to update auction in the database.");
	}

	/// <summary>
	/// Deletes an auction.
	/// </summary>
	/// <param name="id">The unique identifier of the auction to delete.</param>
	/// <returns>A response indicating the outcome of the operation.</returns>
	[HttpDelete("{id:guid}")]
	[Authorize]
	public async Task<ActionResult> DeleteAuction(Guid id)
	{
		var auction = await _repo.GetAuctionEntityById(id);

		if (auction is null) return NotFound();

		if (auction.Seller != User.Identity.Name) return Forbid(); // 403 Forbidden

		_repo.RemoveAuction(auction);

		await _publishEndpoint.Publish<AuctionDeleted>(new {Id = auction.Id.ToString()});

		var result = await _repo.SaveChangesAsync();

		if (!result) return BadRequest("Failed to delete auction in the database.");

		return Ok();
	}
}