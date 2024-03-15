using AutoMapper;
using PaymentService.DTOs;
using PaymentService.Entities;

namespace PaymentService.Utility;

/// <summary>
/// Configures AutoMapper profiles for mapping between entity models and DTOs within the AuctionService.
/// </summary>
public class MappingProfile : Profile
{
	public MappingProfile()
	{
		CreateMap<StripeRequestDto, Payment>()
			.ForMember(dest => dest.Total, opt => opt.MapFrom(src => src.SoldAmount))
			.ForMember(dest => dest.StripeSessionId, opt => opt.MapFrom(src => src.StripeSessionId))
			.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Model))
			.ForMember(dest => dest.AuctionId, opt => opt.MapFrom(src => src.AuctionId));
		// Add other mappings as needed
	}
}