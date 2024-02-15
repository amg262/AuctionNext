using AuctionService.Data;
using AuctionService.DTOs;
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
		var auctions = await context.Auctions.Include(x => x.Item).ToListAsync();
		var auctionsDto = mapper.Map<List<AuctionDto>>(auctions);
		return Ok(auctionsDto);
	}

	[HttpGet("{id:guid}")]
	public async Task<ActionResult<AuctionDto>> GetAuctionById(Guid id)
	{
		var auction = await context.Auctions.Include(x => x.Item).FirstOrDefaultAsync(x => x.Id == id);

		if (auction is null) return NotFound();

		return mapper.Map<AuctionDto>(auction);
	}
}