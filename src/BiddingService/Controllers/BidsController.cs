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

/// <summary>
/// Handles bid-related actions, allowing users to place bids on auctions and retrieve bids for specific auctions.
/// Utilizes gRPC for auction validation, AutoMapper for DTO mapping, and MassTransit for event publishing.
/// </summary>
[ApiController, Route("api/[controller]")]
public class BidsController : ControllerBase
{
	private readonly IMapper _mapper;
	private readonly IPublishEndpoint _publishEndpoint;
	private readonly GrpcAuctionClient _grpcClient;

	/// <summary>
	/// Initializes a new instance of the <see cref="BidsController"/> class.
	/// </summary>
	/// <param name="mapper">The AutoMapper instance for mapping between models and DTOs.</param>
	/// <param name="publishEndpoint">The MassTransit publish endpoint for publishing events.</param>
	/// <param name="grpcClient">The gRPC client for auction service communication.</param>
	public BidsController(IMapper mapper, IPublishEndpoint publishEndpoint,
		GrpcAuctionClient grpcClient)
	{
		_mapper = mapper;
		_publishEndpoint = publishEndpoint;
		_grpcClient = grpcClient;
	}

	/// <summary>
	/// Places a bid on an auction, verifying the auction's existence and eligibility for bidding through gRPC and MongoDB.
	/// Publishes a bid placed event if the bid is successful.
	/// </summary>
	/// <param name="auctionId">The ID of the auction to bid on.</param>
	/// <param name="amount">The bid amount.</param>
	/// <returns>The placed bid's details or an error message if the bid cannot be placed.</returns>
	[Authorize, HttpPost]
	public async Task<ActionResult<BidDto>> PlaceBid(string auctionId, int amount)
	{
		var auction = await DB.Find<Auction>().OneAsync(auctionId);

		if (auction == null)
		{
			auction = _grpcClient.GetAuction(auctionId);

			if (auction == null) return BadRequest("Cannot accept bids on this auction at this time");
		}

		if (auction.Seller == User.Identity.Name)
		{
			return BadRequest("You cannot bid on your own auction");
		}

		var bid = new Bid
		{
			Amount = amount,
			AuctionId = auctionId,
			Bidder = User.Identity.Name
		};

		if (auction.AuctionEnd < DateTime.UtcNow)
		{
			bid.BidStatus = BidStatus.Finished;
		}
		else
		{
			var highBid = await DB.Find<Bid>()
				.Match(a => a.AuctionId == auctionId)
				.Sort(b => b.Descending(x => x.Amount))
				.ExecuteFirstAsync();

			if (highBid != null && amount > highBid.Amount || highBid == null)
			{
				bid.BidStatus = amount > auction.ReservePrice
					? BidStatus.Accepted
					: BidStatus.AcceptedBelowReserve;
			}

			if (highBid != null && bid.Amount <= highBid.Amount)
			{
				bid.BidStatus = BidStatus.TooLow;
			}
		}

		await DB.SaveAsync(bid);

		await _publishEndpoint.Publish(_mapper.Map<BidPlaced>(bid));

		return Ok(_mapper.Map<BidDto>(bid));
	}

	/// <summary>
	/// Retrieves all bids for a given auction, sorted by bid date in descending order.
	/// </summary>
	/// <param name="auctionId">The ID of the auction for which to retrieve bids.</param>
	/// <returns>A list of bids for the auction.</returns>
	[HttpGet("{auctionId}")]
	public async Task<ActionResult<List<BidDto>>> GetBidsForAuction(string auctionId)
	{
		var bids = await DB.Find<Bid>()
			.Match(a => a.AuctionId == auctionId)
			.Sort(b => b.Descending(a => a.BidDate))
			.ExecuteAsync();

		return bids.Select(_mapper.Map<BidDto>).ToList();
	}
}