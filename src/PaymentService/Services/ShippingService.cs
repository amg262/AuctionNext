using EasyPost;
using EasyPost.Exceptions.API;
using EasyPost.Models.API;
using PaymentService.Data;
using EasyPost.Parameters;

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


	public async Task<Shipment> CompleteShipping(Address toAddress)
	{
		try
		{
			var to = new EasyPost.Parameters.Address.Create
			{
				Name = toAddress.Name,
				Street1 = toAddress.Street1,
				Street2 = toAddress.Street2,
				City = toAddress.City,
				State = toAddress.State,
				Zip = toAddress.Zip,
				Country = toAddress.Country,
				Phone = toAddress.Phone,
				// You can add additional parameters as needed outside of the constructor
				Company = "My Company",
				Verify = false,
				VerifyStrict = false
			};

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
				// You can add additional parameters as needed outside of the constructor
				Company = "My Company",
			};


			// var from = new EasyPost.Parameters.Address.Create
			// {
			// 	Name = "AuctionNext",
			// 	Street1 = "s75 w33075 Rolling Fields Drive",
			// 	City = "Mukwonago",
			// 	State = "WI",
			// 	Zip = "53149",
			// 	Country = "US",
			// 	Phone = "262-363-9999",
			// 	// You can add additional parameters as needed outside of the constructor
			// 	Company = "AuctionNext",
			// 	Verify = false,
			// 	VerifyStrict = false
			// };

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

			// var toVerify = await myClient.Address.Create(to.ToDictionary());
			// var fromVerify = await myClient.Address.Create(from.ToDictionary());
			//

			// var address1 = await myClient.Address.Create(addressCreateParameters.ToDictionary());
			// var address2 = await myClient.Address.Create(addressCreateParametersSender.ToDictionary());

			// Shipment shipment = await myClient.Shipment.Create(new Shipment
			// {
			// 	ToAddress = addr1,
			// 	FromAddress = addr2,
			// 	Parcel = parcel
			// });

			// Shipment shipment1 = new Shipment();
			// shipment1.ToAddress = addr1;
			// shipment1.FromAddress = addr2;
			// shipment1.Parcel = parcel;
			// // shipment1 = await myClient.Shipment.Create();
			//
			//
			// // Then convert the object to a dictionary
			// // This step will validate the data and throw an exception if there are any errors (i.e. missing required parameters)
			// // var addressCreateDictionary = addressCreateParameters.ToDictionary();
			// //
			// // // Pass the dictionary into the address creation method as normal
			// // var address = await myClient.Address.Create(addressCreateDictionary);
			// //
			// // return address;
			// var address = await myClient.Address.Create(addressCreateParameters.ToDictionary());
			// var fromAddress = await myClient.Address.Create(addressCreateParameters.ToDictionary());
			// // var verifiedAddress = await myClient.Address.Verify(address.Id);
			// var verifiedFromAddress = await myClient.Address.Verify(fromAddress.Id);

			// Address verifiedToAddress = await VerifyAddress(toAddress);
			// Address verifiedFromAddress = await VerifyAddress(address2);

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