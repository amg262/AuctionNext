using AuctionService.Data;
using AuctionService.Entities;

namespace AuctionService.IntegrationTests.Util;

/// <summary>
/// Provides utility methods for initializing and reinitializing the Auction database with test data.
/// </summary>
public static class DbHelper
{
	/// <summary>
	/// Initializes the database with a predefined set of auctions for testing purposes.
	/// </summary>
	/// <param name="db">The instance of the AuctionDbContext to be initialized.</param>
	/// <remarks>
	/// This method populates the database with a fixed set of auction records to ensure a consistent state for integration tests.
	/// It's essential to call this method before executing tests that rely on known database content.
	/// </remarks>
	public static void InitDbForTests(AuctionDbContext db)
	{
		db.Auctions.AddRange(GetAuctionsForTest());
		db.SaveChanges();
	}

	/// <summary>
	/// Clears existing data and reinitializes the database with the predefined set of auctions.
	/// </summary>
	/// <param name="db">The instance of the AuctionDbContext to be reinitialized.</param>
	/// <remarks>
	/// Use this method to ensure the database is in a known state before each test, 
	/// especially useful when tests may modify database content and you need to reset the state between tests.
	/// </remarks>
	public static void ReinitDbForTests(AuctionDbContext db)
	{
		db.Auctions.RemoveRange(db.Auctions);
		db.SaveChanges();
		InitDbForTests(db);
	}

	/// <summary>
	/// Generates a collection of Auction entities to be used for testing.
	/// </summary>
	/// <returns>A collection of predefined Auction entities.</returns>
	/// <remarks>
	/// This private method provides a fixed list of Auction entities representing various scenarios for integration tests.
	/// The auctions cover a range of conditions, including different makes, models, and auction statuses.
	/// </remarks>
	private static IEnumerable<Auction> GetAuctionsForTest()
	{
		return
		[
			new Auction
			{
				Id = Guid.Parse("afbee524-5972-4075-8800-7d1f9d7b0a0c"),
				Status = Status.Live,
				ReservePrice = 20000,
				Seller = "bob",
				AuctionEnd = DateTime.UtcNow.AddDays(10),
				Item = new Item
				{
					Make = "Ford",
					Model = "GT",
					Color = "White",
					Mileage = 50000,
					Year = 2020,
					ImageUrl = "https://cdn.pixabay.com/photo/2016/05/06/16/32/car-1376190_960_720.jpg"
				}
			},
			// 2 Bugatti Veyron
			new Auction
			{
				Id = Guid.Parse("c8c3ec17-01bf-49db-82aa-1ef80b833a9f"),
				Status = Status.Live,
				ReservePrice = 90000,
				Seller = "alice",
				AuctionEnd = DateTime.UtcNow.AddDays(60),
				Item = new Item
				{
					Make = "Bugatti",
					Model = "Veyron",
					Color = "Black",
					Mileage = 15035,
					Year = 2018,
					ImageUrl = "https://cdn.pixabay.com/photo/2012/05/29/00/43/car-49278_960_720.jpg"
				}
			},
			// 3 Ford mustang

			new Auction
			{
				Id = Guid.Parse("bbab4d5a-8565-48b1-9450-5ac2a5c4a654"),
				Status = Status.Live,
				Seller = "bob",
				AuctionEnd = DateTime.UtcNow.AddDays(4),
				Item = new Item
				{
					Make = "Ford",
					Model = "Mustang",
					Color = "Black",
					Mileage = 65125,
					Year = 2023,
					ImageUrl = "https://cdn.pixabay.com/photo/2012/11/02/13/02/car-63930_960_720.jpg"
				}
			}
		];
	}
}