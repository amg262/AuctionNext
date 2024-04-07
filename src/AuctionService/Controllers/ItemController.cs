using AuctionService.Data;
using AutoMapper;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Controllers;

public class ItemController
{
	private readonly AuctionDbContext _db;
	private readonly IMapper _mapper;
	private readonly IPublishEndpoint _publishEndpoint;
	
	// I need to 
}