using AutoMapper;
using EasyPost;
using EasyPost.Exceptions.API;
using EasyPost.Models.API;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaymentService.Data;
using PaymentService.Entities;
using PaymentService.Services;

namespace PaymentService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShippingController : ControllerBase
{
	private readonly AppDbContext _db;
	private readonly ShippingService _shippingService;
	private readonly IMapper _mapper;
	private readonly IConfiguration _config;
	public Client myClient;


	public ShippingController(AppDbContext db, ShippingService shippingService, IMapper mapper, IConfiguration config)
	{
		_db = db;
		_shippingService = shippingService;
		_mapper = mapper;
		_config = config;
	}

	[HttpGet]
	public async Task<IActionResult> Get()
	{
		List<Shipping?> items = await _db.Shipping.ToListAsync();

		return Ok(items);
	}

	[HttpPost]
	public async Task<IActionResult> Post(Shipping shipping)
	{
		await _db.Shipping.AddAsync(shipping);
		await _db.SaveChangesAsync();
		return Ok(shipping);
	}

	[HttpPut]
	public async Task<IActionResult> Put(Shipping shipping)
	{
		_db.Shipping.Update(shipping);
		await _db.SaveChangesAsync();
		return Ok(shipping);
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> Delete(int id)
	{
		var item = await _db.Shipping.FirstOrDefaultAsync(c => c.ShippingId == id);
		_db.Shipping.Remove(item);
		await _db.SaveChangesAsync();
		return Ok();
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> GetShippingById(int id)
	{
		var item = await _db.Shipping.FirstOrDefaultAsync(c => c.ShippingId == id);
		return Ok(item);
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> GetShippingByPaymentId(Guid paymentId)
	{
		var item = await _db.Shipping.FirstOrDefaultAsync(c => c.PaymentId == paymentId);
		return Ok(item);
	}

	[HttpPost("complete/{paymentId}")]
	public async Task<IActionResult> CompleteShipping(Guid paymentId)
	{
		var payment = await _db.Payments.FirstOrDefaultAsync(c => c.Id == paymentId);
		var shipping = await _db.Shipping.FirstOrDefaultAsync(b => b.PaymentId == paymentId);
		if (payment == null) return NotFound();

		try
		{
			var to1 = new Address
			{
				Name = shipping.Name,
				Street1 = shipping.Street1,
				Street2 = shipping.Street2,
				City = shipping.City,
				State = shipping.State,
				Zip = shipping.Zip,
				Country = shipping.Country,
				Company = shipping.Name,
			};

			var from1 = new Address
			{
				Name = "AuctionNext",
				Street1 = "s75 w33075 Rolling Fields Drive",
				City = "Mukwonago",
				State = "WI",
				Zip = "53149",
				Country = "US",
				Phone = "262-363-9999",
				Company = "AuctionNext"
			};

			var parcel = new Parcel
			{
				Length = 9,
				Width = 6,
				Height = 2,
				Weight = 10 // Assuming weight is in ounces
			};

			myClient = new Client(new ClientConfiguration(_config["EasyPost:ApiKey"]));


			Shipment myShipment = await myClient.Shipment.Create(new Dictionary<string, object>
			{
				{"from_address", from1},
				{"to_address", to1},
				{
					"parcel", new Dictionary<string, object>
					{
						{"length", 9},
						{"width", 6},
						{"height", 2},
						{"weight", 10}
					}
				}
			});
			Shipment myPurchasedShipment = await myClient.Shipment.Buy(myShipment.Id, myShipment.LowestRate());
			// myShipment = await myClient.Shipment.Buy(myShipment.Id, myShipment.LowestRate());

			shipping.TrackingCode = myPurchasedShipment.TrackingCode;
			shipping.Rate = myPurchasedShipment.Rates[0].Price.ToString();
			shipping.Carrier = myPurchasedShipment.Rates[0].Carrier;
			shipping.UpdatedAt = DateTime.UtcNow;

			_db.Shipping.Update(shipping);
			await _db.SaveChangesAsync();

			return Ok(shipping);
		}
		catch (InvalidRequestError error)
		{
			Console.WriteLine($"Invalid Request Error: {error.Message}");
			throw;
		}
		catch (ApiError e)
		{
			// Handle any API exceptions (e.g., invalid address format)
			Console.WriteLine($"API Exception: {e.Message}");
			throw;
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			throw;
		}
	}

	public async Task<IActionResult> VerifyAddress(Address toVerify)
	{
		var verifiedAddress = await _shippingService.VerifyAddress(toVerify);
		return Ok(verifiedAddress);
	}
}