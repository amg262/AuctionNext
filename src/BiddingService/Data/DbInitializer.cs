using BiddingService.Models;
using MongoDB.Driver;
using MongoDB.Entities;

namespace BiddingService.Data;

public static class DbInitializer
{
	public static async Task InitDb(WebApplication app)
	{
		await DB.InitAsync("BidDb", MongoClientSettings
			.FromConnectionString(app.Configuration.GetConnectionString("BidDbConnection")));

		await DB.Index<Bid>()
			.Key(x => x.Amount, KeyType.Ascending)
			.Key(x => x.AuctionId, KeyType.Ascending)
			.Key(x => x.BidStatus, KeyType.Ascending)
			.Key(x => x.BidTime, KeyType.Ascending)
			.Key(x => x.Bidder, KeyType.Text)
			.CreateAsync();

		// Check if the 'Post' collection is empty
		var count = await DB.CountAsync<Bid>();

		// If the 'Post' collection is empty, seed the database with example posts
		if (count == 0)
		{
			var seedBids = new List<Bid>
			{
				new()
				{
					AuctionId = "afbee524-5972-4075-8800-7d1f9d7b0a0c",
					Bidder = "alice",
					Amount = 21000,
					BidTime = DateTime.UtcNow.AddDays(-1),
					BidStatus = BidStatus.Accepted
				},
				new()
				{
					AuctionId = "c8c3ec17-01bf-49db-82aa-1ef80b833a9f",
					Bidder = "bob",
					Amount = 95000,
					BidTime = DateTime.UtcNow.AddDays(-2),
					BidStatus = BidStatus.Accepted
				},
				new()
				{
					AuctionId = "bbab4d5a-8565-48b1-9450-5ac2a5c4a654",
					Bidder = "tom",
					Amount = 66000,
					BidTime = DateTime.UtcNow.AddDays(-3),
					BidStatus = BidStatus.Accepted
				},
				new()
				{
					AuctionId = "155225c1-4448-4066-9886-6786536e05ea",
					Bidder = "alice",
					Amount = 51000,
					BidTime = DateTime.UtcNow.AddDays(-4),
					BidStatus = BidStatus.Accepted
				},
				new()
				{
					AuctionId = "466e4744-4dc5-4987-aae0-b621acfc5e39",
					Bidder = "bob",
					Amount = 25000,
					BidTime = DateTime.UtcNow.AddDays(-5),
					BidStatus = BidStatus.Accepted
				},
				new()
				{
					AuctionId = "dc1e4071-d19d-459b-b848-b5c3cd3d151f",
					Bidder = "tom",
					Amount = 55000,
					BidTime = DateTime.UtcNow.AddDays(-6),
					BidStatus = BidStatus.Accepted
				},
				new()
				{
					AuctionId = "47111973-d176-4feb-848d-0ea22641c31a",
					Bidder = "alice",
					Amount = 160000,
					BidTime = DateTime.UtcNow.AddDays(-7),
					BidStatus = BidStatus.Accepted
				},
				new()
				{
					AuctionId = "6a5011a1-fe1f-47df-9a32-b5346b289391",
					Bidder = "bob",
					Amount = 105000,
					BidTime = DateTime.UtcNow.AddDays(-8),
					BidStatus = BidStatus.Accepted
				},
				new()
				{
					AuctionId = "40490065-dac7-46b6-acc4-df507e0d6570",
					Bidder = "tom",
					Amount = 20500,
					BidTime = DateTime.UtcNow.AddDays(-9),
					BidStatus = BidStatus.Accepted
				},
				new()
				{
					AuctionId = "678650ee-bb8c-4adb-9f00-183ee05da8d2",
					Bidder = "alice",
					Amount = 21000,
					BidTime = DateTime.UtcNow.AddDays(-10),
					BidStatus = BidStatus.Accepted
				}
			};

			foreach (var bid in seedBids)
			{
				await bid.SaveAsync();
			}
		}
	}
}