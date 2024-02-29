using AutoMapper;
using BiddingService.DTOs;
using BiddingService.Models;
using BiddingService.Services;
using Contracts;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;

namespace BiddingService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BidsController : ControllerBase
{
	private readonly IMapper _mapper;
	private readonly IPublishEndpoint _publishEndpoint;
	private readonly GrpcAuctionClient _grpcClient;

	public BidsController(IMapper mapper, IPublishEndpoint publishEndpoint, GrpcAuctionClient grpcClient)
	{
		_mapper = mapper;
		_publishEndpoint = publishEndpoint;
		_grpcClient = grpcClient;
	}

	[Authorize, HttpPost]
	public async Task<ActionResult<BidDto>> PlaceBid(string auctionId, int amount)
	{
		var auction = await DB.Find<Auction>().OneAsync(auctionId);

		if (auction == null)
		{
			// todo check with auction service if it has an auction
			auction = _grpcClient.GetAuction(auctionId);

			if (auction == null) return BadRequest("Cannot accept bids for auction that does not exist");
		}

		if (auction.Seller == User.Identity.Name)
		{
			return BadRequest("You cannot bid on your own auction");
		}

		var bid = new Bid
		{
			AuctionId = auctionId,
			Bidder = User?.Identity?.Name,
			Amount = amount
		};

		if (auction.AuctionEnd < DateTime.UtcNow)
		{
			bid.BidStatus = BidStatus.Finished;
		}
		else
		{
			var highBid = await DB.Find<Bid>()
				.Match(b => b.AuctionId == auctionId)
				.Sort(b => b.Amount, Order.Descending)
				.ExecuteFirstAsync();

			if (highBid != null && amount > highBid.Amount || highBid == null)
			{
				bid.BidStatus = amount > auction.ReservePrice ? BidStatus.Accepted : BidStatus.AcceptedBelowReserve;
			}

			if (highBid != null && amount <= highBid.Amount)
			{
				bid.BidStatus = BidStatus.TooLow;
			}
		}

		await DB.SaveAsync(bid);

		await _publishEndpoint.Publish(_mapper.Map<BidPlaced>(bid));

		return Ok(_mapper.Map<BidDto>(bid));
	}

	[HttpGet("{auctionId}")]
	public async Task<ActionResult<List<BidDto>>> GetBidsForAuction(string auctionId)
	{
		var bids = await DB.Find<Bid>()
			.Match(a => a.AuctionId == auctionId)
			.Sort(b => b.Descending(a => a.BidTime))
			.ExecuteAsync();

		return bids.Select(_mapper.Map<BidDto>).ToList();
	}
}