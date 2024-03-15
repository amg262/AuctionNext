using BiddingService.Models;
using MongoDB.Driver;

namespace BiddingService.Data;

public static class DbInitializer
{
	public static void InitDb(IMongoDatabase db)
	{
		var bids = new List<Bid>
		{
			new Bid
			{
				Amount = 99999999,
				AuctionId = "0bab0135-d91d-4120-a1b9-83e6da262957",
				BidStatus = 0,
				BidTime = DateTime.Parse("2024-02-14T23:07:29.041Z"),
				Bidder = "bob"
			},
			new Bid
			{
				Amount = 100,
				AuctionId = "40490065-dac7-46b6-acc4-df507e0d6570",
				BidStatus = 0,
				BidTime = DateTime.Parse("2024-03-14T23:08:45.535Z"),
				Bidder = "bob"
			},
			new Bid
			{
				Amount = 100000,
				AuctionId = "3659ac24-29dd-407a-81f5-ecfe6f924b9b",
				BidStatus = 0,
				BidTime = DateTime.Parse("2024-03-14T23:15:54.003Z"),
				Bidder = "bob"
			}
		};

		var collection = db.GetCollection<Bid>("Bids");
		collection.InsertMany(bids);
	}
}