using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PaymentService.Data;
using Stripe;
using StripeCoupon = Stripe.Coupon;
using Coupon = PaymentService.Entities.Coupon;

namespace PaymentService.Services;

/// <summary>
/// Provides services for synchronizing Stripe coupons with the local application database.
/// Implements IDisposable and IAsyncDisposable for proper resource management.
/// </summary>
public class StripeService
{
	private readonly IConfiguration _config;
	private readonly AppDbContext _db;
	private readonly IMapper _mapper;

	/// <summary>
	/// Initializes a new instance of the StripeService with necessary dependencies.
	/// </summary>
	/// <param name="config">Configuration interface for accessing application settings.</param>
	/// <param name="db">The application's database context for data access operations.</param>
	/// <param name="mapper">AutoMapper instance for object-to-object mapping.</param>
	public StripeService(IConfiguration config, AppDbContext db, IMapper mapper)
	{
		_config = config;
		_db = db;
		_mapper = mapper;
	}

	/// <summary>
	/// Synchronizes Stripe coupons with the local database. Fetches all coupons from Stripe,
	/// checks for new ones, and adds them to the database if they do not already exist.
	/// </summary>
	/// <returns>A task that represents the asynchronous operation. The task result contains
	/// a list of Stripe coupons.</returns>
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