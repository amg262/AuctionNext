using AutoMapper;
using BiddingService.DTOs;
using BiddingService.Models;
using Contracts;

namespace BiddingService.RequestHelpers;

/// <summary>
/// Defines AutoMapper mapping profiles for converting between entity models and data transfer objects (DTOs).
/// This class inherits from AutoMapper's Profile class, allowing it to configure mappings for the AutoMapper instance.
/// </summary>
public class MappingProfiles : Profile
{
	/// <summary>
	/// Configures the mappings between model entities and DTOs upon instantiation of the MappingProfiles class.
	/// </summary>
	public MappingProfiles()
	{
		CreateMap<Bid, BidDto>();
		CreateMap<Bid, BidPlaced>();
	}
}