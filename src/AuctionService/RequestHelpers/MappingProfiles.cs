using AuctionService.DTOs;
using AuctionService.Entities;
using AutoMapper;

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
		// Maps Auction to AuctionDto and includes Item entity members for detailed information.
		CreateMap<Auction, AuctionDto>().IncludeMembers(x => x.Item);
		// Maps Item directly to AuctionDto, used for operations where Item details are directly mapped to an AuctionDto.
		CreateMap<Item, AuctionDto>();
		// Maps CreateAuctionDto to Auction, with a custom mapping for the Item property to handle complex object mapping.
		CreateMap<CreateAuctionDto, Auction>()
			.ForMember(d => d.Item, o => o.MapFrom(s => s));
		CreateMap<CreateAuctionDto, Item>();
	}
}