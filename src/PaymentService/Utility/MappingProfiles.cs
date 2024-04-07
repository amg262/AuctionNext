using AutoMapper;
using Contracts;
using PaymentService.DTOs;
using PaymentService.Entities;
using StripeCoupon = Stripe.Coupon;
using StripeAddress = Stripe.Address;
using Coupon = PaymentService.Entities.Coupon;
using EasyPost.Models.API;

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
			.ForMember(dest => dest.CouponCode, opt => opt.MapFrom(src => src.CouponCode))
			.ForMember(dest => dest.Length, opt => opt.MapFrom(src => src.Length))
			.ForMember(dest => dest.Width, opt => opt.MapFrom(src => src.Width))
			.ForMember(dest => dest.Height, opt => opt.MapFrom(src => src.Height))
			.ForMember(dest => dest.Weight, opt => opt.MapFrom(src => src.Weight))
			.ReverseMap();

		CreateMap<CouponDto, Coupon>().ReverseMap();

		CreateMap<StripeCoupon, CouponDto>()
			.ForMember(dest => dest.CouponCode, opt => opt.MapFrom(src => src.Name))
			.ForMember(dest => dest.DiscountAmount, opt => opt.MapFrom(src => src.AmountOff / 100.0))
			.ForMember(dest => dest.MinAmount, opt => opt.MapFrom(src => src.AmountOff / 100.0))
			.ReverseMap();

		CreateMap<StripeCoupon, Coupon>()
			.ForMember(dest => dest.CouponCode, opt => opt.MapFrom(src => src.Name))
			.ForMember(dest => dest.DiscountAmount, opt => opt.MapFrom(src => src.AmountOff / 100.0))
			.ForMember(dest => dest.MinAmount, opt => opt.MapFrom(src => src.AmountOff / 100.0))
			.ReverseMap();

		CreateMap<RewardDto, Reward>().ReverseMap();

		CreateMap<Payment, PaymentMade>().ReverseMap();

		CreateMap<StripeAddress, Address>()
			.ForMember(dest => dest.Street1, opt => opt.MapFrom(src => src.Line1))
			.ForMember(dest => dest.Street2, opt => opt.MapFrom(src => src.Line2))
			.ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
			.ForMember(dest => dest.State, opt => opt.MapFrom(src => src.State))
			.ForMember(dest => dest.Zip, opt => opt.MapFrom(src => src.PostalCode))
			.ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country))
			.ReverseMap();
	}
}