using AuctionService.DTOs;
using AuctionService.Entities;
using AutoMapper;
using Contracts;

namespace AuctionService.RequestHelpers;

/// <summary>
/// Configures AutoMapper profiles for mapping between entity models and DTOs within the AuctionService.
/// </summary>
public class MappingProfiles : Profile
{
	/// <summary>
	/// Initializes a new instance of the <see cref="MappingProfiles"/> class.
	/// </summary>
	/// <remarks>
	/// This constructor sets up the following mappings:
	/// - From <see cref="Auction"/> to <see cref="AuctionDto"/>, including members from the related <see cref="Item"/> entity.
	/// - Direct mapping from <see cref="Item"/> to <see cref="AuctionDto"/>.
	/// - From <see cref="CreateAuctionDto"/> to <see cref="Auction"/>, with custom mapping for the <see cref="Auction.Item"/> property.
	/// - Direct mapping from <see cref="CreateAuctionDto"/> to <see cref="Item"/>.
	/// These mappings are crucial for converting between the application's internal entities and the data transfer objects used in API requests and responses.
	/// </remarks>
	public MappingProfiles()
	{
		CreateMap<Auction, AuctionDto>().IncludeMembers(x => x.Item);
		CreateMap<Item, AuctionDto>();
		CreateMap<CreateAuctionDto, Auction>()
			.ForMember(d => d.Item, o => o.MapFrom(s => s));
		CreateMap<CreateAuctionDto, Item>();
		CreateMap<AuctionDto, AuctionCreated>();
		CreateMap<Auction, AuctionUpdated>().IncludeMembers(a => a.Item);
		CreateMap<Item, AuctionUpdated>();
	}
}