using AutoMapper;

namespace PostService.RequestHelpers;

/// <summary>
/// Defines AutoMapper mapping profiles for the SearchService.
/// This class is responsible for configuring the mappings between DTOs (Data Transfer Objects) and entity models.
/// </summary>
public class MappingProfiles : Profile
{

	public MappingProfiles()
	{
		// Maps AuctionCreated DTO to Item entity, configuring how auction creation data should be translated to the Item model.
		// CreateMap<AuctionCreated, Item>();
		// // Maps AuctionUpdated DTO to Item entity, specifying how updates from auction events are applied to the existing Item model.
		// CreateMap<AuctionUpdated, Item>();
	}
}