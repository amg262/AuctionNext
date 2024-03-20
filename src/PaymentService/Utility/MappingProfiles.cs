using AutoMapper;
using PaymentService.DTOs;
using PaymentService.Entities;

namespace PaymentService.Utility;

/// <summary>
/// Configures AutoMapper profiles for mapping between data transfer objects (DTOs) and entity models within the PaymentService.
/// This facilitates the conversion of data between the API layer and the database layer, improving the separation of concerns.
/// </summary>
public class MappingProfile : Profile
{
	/// <summary>
	/// Configures the mappings between StripeRequestDto and Payment entity.
	/// </summary>
	public MappingProfile()
	{
		CreateMap<StripeRequestDto, Payment>()
			.ForMember(dest => dest.Total, opt => opt.MapFrom(src => src.SoldAmount))
			.ForMember(dest => dest.StripeSessionId, opt => opt.MapFrom(src => src.StripeSessionId))
			.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Model))
			.ForMember(dest => dest.Total, opt => opt.MapFrom(src => src.SoldAmount))
			.ForMember(dest => dest.Name, opt => opt.MapFrom(src => $"{src.Year} {src.Color} {src.Make} {src.Model}"))
			.ForMember(dest => dest.Status, opt => opt.Ignore())
			.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Guid))
			.ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
			.ForMember(dest => dest.AuctionId, opt => opt.MapFrom(src => src.AuctionId))
			.ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Winner))
			.ReverseMap();

		CreateMap<CouponDto, Coupon>().ReverseMap();

		CreateMap<Stripe.Coupon, Coupon>()
			.ForMember(dest => dest.CouponCode, opt => opt.MapFrom(src => src.Name))
			.ForMember(dest => dest.DiscountAmount, opt => opt.MapFrom(src => src.AmountOff / 100.0))
			.ForMember(dest => dest.MinAmount, opt => opt.MapFrom(src => src.AmountOff / 100.0))
			.ReverseMap();
		// Add other mappings as needed
	}
}