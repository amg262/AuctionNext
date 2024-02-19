using AutoMapper;
using Contracts;
using SearchService.Models;

namespace SearchService.RequestHelpers;

/// <summary>
/// Defines AutoMapper mapping profiles for the SearchService.
/// This class is responsible for configuring the mappings between DTOs (Data Transfer Objects) and entity models.
/// </summary>
public class MappingProfiles : Profile
{
	/// <summary>
	/// Initializes a new instance of the <see cref="MappingProfiles"/> class.
	/// Sets up the mapping configurations for the application.
	/// </summary>
	/// <remarks>
	/// This constructor configures the AutoMapper mappings that are needed for the application.
	/// Specifically, it maps between the <see cref="AuctionCreated"/> DTO and the <see cref="Item"/> entity.
	/// This mapping facilitates the conversion of auction creation event data into the item model format used within the SearchService.
	/// </remarks>
	public MappingProfiles()
	{
		CreateMap<AuctionCreated, Item>();
		CreateMap<AuctionUpdated, Item>();
	}
}