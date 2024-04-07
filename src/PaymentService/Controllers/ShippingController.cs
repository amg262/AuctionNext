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

/// <summary>
/// Handles shipping-related actions, including listing, creating, updating, and deleting shipping records,
/// as well as completing shipping actions by integrating with the EasyPost API.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ShippingController : ControllerBase
{
	private readonly AppDbContext _db;
	private readonly ShippingService _shippingService;
	private readonly IMapper _mapper;
	private readonly IConfiguration _config;
	public Client myClient;

	/// <summary>
	/// Initializes a new instance of the ShippingController class.
	/// </summary>
	/// <param name="db">Database context for accessing shipping records.</param>
	/// <param name="shippingService">Service for handling shipping logic and external API integration.</param>
	/// <param name="mapper">Automapper instance for model mapping.</param>
	/// <param name="config">Configuration for accessing application settings.</param>
	public ShippingController(AppDbContext db, ShippingService shippingService, IMapper mapper, IConfiguration config)
	{
		_db = db;
		_shippingService = shippingService;
		_mapper = mapper;
		_config = config;
	}

	/// <summary>
	/// Retrieves all shipping records from the database.
	/// </summary>
	/// <returns>A list of shipping records.</returns>
	[HttpGet]
	public async Task<IActionResult> Get()
	{
		List<Shipping?> items = await _db.Shipping.ToListAsync();

		return Ok(items);
	}

	/// <summary>
	/// Adds a new shipping record to the database.
	/// </summary>
	/// <param name="shipping">The shipping record to add.</param>
	/// <returns>The added shipping record.</returns>
	[HttpPost]
	public async Task<IActionResult> Post(Shipping shipping)
	{
		await _db.Shipping.AddAsync(shipping);
		await _db.SaveChangesAsync();
		return Ok(shipping);
	}

	/// <summary>
	/// Updates an existing shipping record in the database.
	/// </summary>
	/// <param name="shipping">The shipping record with updated information.</param>
	/// <returns>The updated shipping record.</returns>
	[HttpPut]
	public async Task<IActionResult> Put(Shipping shipping)
	{
		_db.Shipping.Update(shipping);
		await _db.SaveChangesAsync();
		return Ok(shipping);
	}

	/// <summary>
	/// Deletes a shipping record from the database.
	/// </summary>
	/// <param name="id">The ID of the shipping record to delete.</param>
	/// <returns>An IActionResult indicating the outcome of the operation.</returns>
	[HttpDelete("{id}")]
	public async Task<IActionResult> Delete(int id)
	{
		var item = await _db.Shipping.FirstOrDefaultAsync(c => c.ShippingId == id);
		_db.Shipping.Remove(item);
		await _db.SaveChangesAsync();
		return Ok();
	}

	/// <summary>
	/// Retrieves a shipping record by its ID.
	/// </summary>
	/// <param name="id">The ID of the shipping record to retrieve.</param>
	/// <returns>The requested shipping record.</returns>
	[HttpGet("{id}")]
	public async Task<IActionResult> GetShippingById(int id)
	{
		var item = await _db.Shipping.FirstOrDefaultAsync(c => c.ShippingId == id);
		return Ok(item);
	}

	/// <summary>
	/// Completes the shipping process for a given payment, creating a shipment with EasyPost and updating the shipping record.
	/// </summary>
	/// <param name="paymentId">The ID of the payment associated with the shipping.</param>
	/// <returns>The updated shipping record with tracking information.</returns>
	/// <exception cref="InvalidRequestError">Thrown when the request to EasyPost API is invalid.</exception>
	/// <exception cref="ApiError">Thrown when there's an error in the EasyPost API request.</exception>
	/// <exception cref="Exception">Thrown when an unexpected error occurs.</exception>
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
						{"length", 10},
						{"width", 20},
						{"height", 30},
						{"weight", 200},
						// {"length", 9},
						// {"width", 6},
						// {"height", 2},
						// {"weight", 10}

						// {"width", 6},
						// {"height", 2},
						// {"weight", 10}
					}
				}
			});
			Shipment myPurchasedShipment = await myClient.Shipment.Buy(myShipment.Id, myShipment.LowestRate());
			// myShipment = await myClient.Shipment.Buy(myShipment.Id, myShipment.LowestRate());

			shipping.TrackingCode = myPurchasedShipment.TrackingCode;
			shipping.TrackingUrl = myPurchasedShipment.Tracker.PublicUrl;
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

	/// <summary>
	/// Verifies an address using the shipping service.
	/// </summary>
	/// <param name="toVerify">The address to verify.</param>
	/// <returns>The verified address.</returns>
	public async Task<IActionResult> VerifyAddress(Address toVerify)
	{
		var verifiedAddress = await _shippingService.VerifyAddress(toVerify);
		return Ok(verifiedAddress);
	}
}