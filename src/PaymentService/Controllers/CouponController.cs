using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaymentService.Data;
using PaymentService.DTOs;
using Stripe;
using Coupon = PaymentService.Entities.Coupon;
using StripeCoupon = Stripe.Coupon;

namespace PaymentService.Controllers;

/// <summary>
/// Controller for managing coupons in both the application database and Stripe.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CouponController : ControllerBase
{
	private readonly IConfiguration _config;
	private readonly AppDbContext _db;
	private readonly IMapper _mapper;

	/// <summary>
	/// Initializes a new instance of the CouponController.
	/// </summary>
	/// <param name="config">Configuration for accessing application settings.</param>
	/// <param name="db">Database context for CRUD operations.</param>
	/// <param name="mapper">AutoMapper for object-to-object mapping.</param>
	public CouponController(IConfiguration config, AppDbContext db, IMapper mapper)
	{
		_config = config;
		_db = db;
		_mapper = mapper;
	}

	/// <summary>
	/// Retrieves all coupons from the database.
	/// </summary>
	/// <returns>A list of coupons.</returns>
	[HttpGet]
	public async Task<IActionResult> Get()
	{
		List<Coupon?> coupons = await _db.Coupons.ToListAsync();
		return Ok(coupons);
	}

	/// <summary>
	/// Placeholder method for coupon operations. Currently returns a simple string.
	/// </summary>
	/// <returns>A string indicating the coupon method has been hit.</returns>
	[HttpPost("coupon")]
	public async Task<IActionResult> Coupon()
	{
		return Ok("coupon");
	}

	/// <summary>
	/// Creates a new coupon in both the application database and Stripe.
	/// </summary>
	/// <param name="couponDto">Data transfer object containing coupon details.</param>
	/// <returns>A status code indicating success or failure, and the created coupon data.</returns>
	[HttpPost("create-coupon")]
	public async Task<IActionResult> CreateCoupon([FromBody] CouponDto? couponDto)
	{
		try
		{
			Coupon coupon = _mapper.Map<Coupon>(couponDto);
			_db.Coupons.Add(coupon);
			await _db.SaveChangesAsync();

			// Create the coupon in Stripe
			var options = new CouponCreateOptions
			{
				AmountOff = (long) (couponDto.DiscountAmount * 100),
				Name = couponDto.CouponCode,
				Currency = "usd",
				Id = couponDto.CouponCode,
			};
			var service = new CouponService();
			await service.CreateAsync(options);

			var result = _mapper.Map<CouponDto>(coupon);

			return StatusCode(201);
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

	/// <summary>
	/// Updates an existing coupon's details in both the application database and Stripe.
	/// Note: Stripe coupons are immutable; this method likely needs adjustment.
	/// </summary>
	/// <param name="couponDto">Data transfer object containing updated coupon details.</param>
	/// <returns>A status code indicating success or failure, and the updated coupon data.</returns>
	[HttpPut("update-coupon")]
	public async Task<IActionResult> UpdateCoupon([FromBody] CouponDto? couponDto)
	{
		try
		{
			Coupon coupon = _mapper.Map<Coupon>(couponDto);
			_db.Coupons.Update(coupon);
			await _db.SaveChangesAsync();

			// Update the coupon in Stripe
			var options = new CouponUpdateOptions
			{
				Name = couponDto.CouponCode,
			};
			var service = new CouponService();
			await service.UpdateAsync(couponDto.CouponCode, options);

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

	/// <summary>
	/// Synchronizes Stripe coupons with the local database. Fetches all coupons from Stripe,
	/// checks for new ones, and adds them to the database if they do not already exist.
	/// </summary>
	/// <returns>A task that represents the asynchronous operation. The task result contains
	/// a list of Stripe coupons.</returns>
	[HttpGet("stripe-coupons")]
	public async Task<StripeList<StripeCoupon>> SyncStripeCoupons()
	{
		try
		{
			var service = new CouponService();
			var options = new CouponListOptions();

			var stripeCoupons = await service.ListAsync(options);
			if (!stripeCoupons.Any()) return stripeCoupons;

			var hasNewCoupons = false;

			foreach (var stripeCoupon in stripeCoupons)
			{
				var exists = await _db.Coupons.AnyAsync(c => c.CouponCode == stripeCoupon.Name);
				if (!exists)
				{
					var newCoupon = _mapper.Map<Coupon>(stripeCoupon);
					_db.Coupons.Add(newCoupon);
					hasNewCoupons = true;
				}
			}

			if (hasNewCoupons) await _db.SaveChangesAsync();

			return stripeCoupons;
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
	}
}