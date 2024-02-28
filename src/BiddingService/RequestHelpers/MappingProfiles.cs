using AutoMapper;
using BiddingService.DTOs;
using BiddingService.Models;
using Contracts;

namespace BiddingService.RequestHelpers;

/// <summary>
/// Defines mapping profiles for AutoMapper to convert between domain models and DTOs,
/// as well as to contracts used for messaging.
/// </summary>
public class MappingProfiles : Profile
{
	public MappingProfiles()
	{
		CreateMap<Bid, BidDto>();
		CreateMap<Bid, BidPlaced>();
	}
}