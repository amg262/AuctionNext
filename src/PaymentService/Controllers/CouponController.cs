using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaymentService.Data;
using PaymentService.DTOs;
using Stripe;
using Coupon = PaymentService.Entities.Coupon;

namespace PaymentService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CouponController : ControllerBase
{
	private readonly IConfiguration _config;
	private readonly AppDbContext _db;
	private readonly IMapper _mapper;

	public CouponController(IConfiguration config, AppDbContext db, IMapper mapper)
	{
		_config = config;
		_db = db;
		_mapper = mapper;
	}

	[HttpPost("coupon")]
	public async Task<IActionResult> Coupon()
	{
		return Ok("coupon");
	}

	[HttpPost("create-coupon")]
	public async Task<IActionResult> CreateCoupon([FromBody] CouponDto? couponDto)
	{
		try
		{
			Coupon coupon = _mapper.Map<Coupon>(couponDto);
			_db.Coupons.Add(coupon);
			await _db.SaveChangesAsync();

			// Create the coupon in Stripe
			var options = new Stripe.CouponCreateOptions
			{
				AmountOff = (long) (couponDto.DiscountAmount * 100),
				Name = couponDto.CouponCode,
				Currency = "usd",
				Id = couponDto.CouponCode,
			};
			var service = new Stripe.CouponService();
			await service.CreateAsync(options);

			var result = _mapper.Map<CouponDto>(coupon);

			return Ok(result);
		}
		catch (StripeException se)
		{
			Console.WriteLine(se);
			throw;
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			throw;
		}

		return BadRequest();
	}

	[HttpGet("get-coupons")]
	public async Task<StripeList<Stripe.Coupon>> GetStripeCoupons()
	{
		var service = new CouponService();
		var options = new CouponListOptions
		{
			Limit = 100 // Adjust based on your needs
		};

		var coupons = await service.ListAsync(options);

		foreach (var coupon in coupons)
		{
			var newCoupon = _mapper.Map<Coupon>(coupon);

			var existingCoupon = await _db.Coupons.FirstOrDefaultAsync(c => c.CouponCode == coupon.Name);

			if (existingCoupon == null)
			{
				// _db.Coupons.Add(new Coupon
				// {
				// 	CouponCode = coupon.Name,
				// 	DiscountAmount = coupon.AmountOff / 100.0
				// });
				_db.Coupons.Add(newCoupon);
				await _db.SaveChangesAsync();
			}
		}

		return coupons;
	}
}