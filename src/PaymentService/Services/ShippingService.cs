using EasyPost;
using EasyPost.Exceptions.API;
using EasyPost.Models.API;
using PaymentService.Data;
using PaymentService.Entities;

namespace PaymentService.Services;

public class ShippingService
{
	private readonly AppDbContext _db;
	private readonly IConfiguration _config;
	private readonly Client myClient;

	public ShippingService(AppDbContext db, IConfiguration config)
	{
		_db = db;
		_config = config;
		myClient = new Client(new ClientConfiguration(_config["EasyPost:ApiKey"]));
	}


	public async Task<Shipment> CompleteShipping(Payment payment, Address toAddress)
	{
		try
		{
			var to1 = new Address
			{
				Name = toAddress.Name,
				Street1 = toAddress.Street1,
				Street2 = toAddress.Street2,
				City = toAddress.City,
				State = toAddress.State,
				Zip = toAddress.Zip,
				Country = toAddress.Country,
				Phone = toAddress.Phone,
				Company = toAddress.Name,
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
				// You can add additional parameters as needed outside of the constructor
				Company = "AuctionNext"
			};

			var parcel = new Parcel
			{
				Length = 9,
				Width = 6,
				Height = 2,
				Weight = 10 // Assuming weight is in ounces
			};


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

			await _db.Shipping.AddAsync(new Shipping
			{
				PaymentId = payment.Id,
				Name = toAddress.Name,
				Company = toAddress.Street1,
				Street1 = toAddress.Street1,
				Street2 = toAddress.Street2,
				City = toAddress.City,
				State = toAddress.State,
				Zip = toAddress.Zip,
				Country = toAddress.Country,
				Email = toAddress.Email,
				UpdatedAt = DateTime.UtcNow,
				Carrier = myPurchasedShipment.Rates[0].Carrier,
				Rate = myPurchasedShipment.Rates[0].Price,
				TrackingCode = myPurchasedShipment.TrackingCode
			});

			await _db.SaveChangesAsync();

			return myPurchasedShipment;
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

	public async Task<Address> VerifyAddress(Address toVerify)
	{
		var verifiedAddress = await myClient.Address.CreateAndVerify(toVerify.AsDictionary());
		return verifiedAddress;
	}
}