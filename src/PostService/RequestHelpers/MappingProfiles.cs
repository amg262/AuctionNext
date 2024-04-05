using AutoMapper;

namespace PostService.RequestHelpers;

/// <summary>
/// Defines AutoMapper mapping profiles for the SearchService.
/// This class is responsible for configuring the mappings between DTOs (Data Transfer Objects) and entity models.
/// </summary>
public class MappingProfiles : Profile
{
	/// <summary>
	/// Initializes a new instance of the <see cref="MappingProfiles"/> class, setting up the AutoMapper configurations necessary for the application.
	/// </summary>
	/// <remarks>
	/// Configures AutoMapper to map between the following:
	/// - <see cref="AuctionCreated"/> DTO and the <see cref="Item"/> entity, facilitating the conversion of auction creation data into the item model.
	/// - <see cref="AuctionUpdated"/> DTO and the <see cref="Item"/> entity, allowing updates from auction events to be applied to the item model.
	/// These mappings ensure that auction event data is correctly translated into the corresponding models used within the SearchService, 
	/// supporting operations like creation and updates of items in the service's context.
	/// </remarks>
	public MappingProfiles()
	{
		// Maps AuctionCreated DTO to Item entity, configuring how auction creation data should be translated to the Item model.
		// CreateMap<AuctionCreated, Item>();
		// // Maps AuctionUpdated DTO to Item entity, specifying how updates from auction events are applied to the existing Item model.
		// CreateMap<AuctionUpdated, Item>();
	}
}