using AuctionService.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Data;

/// <summary>
/// Provides functionality to initialize and seed the database at application startup.
/// </summary>
public static class DbInitializer
{
	/// <summary>
	/// Initializes and seeds the auction database. This method is called during the application startup.
	/// It ensures that the database is created, applies any pending migrations, and seeds the database with initial data if necessary.
	/// </summary>
	/// <param name="app">The instance of <see cref="WebApplication"/> to access application services.</param>
	public static void InitDb(WebApplication app)
	{
		using var scope = app.Services.CreateScope();

		SeedData(scope.ServiceProvider.GetService<AuctionDbContext>());
	}

	/// <summary>
	/// Seeds the database with initial data if it has not been seeded already.
	/// This includes creating predefined auction items with associated details.
	/// </summary>
	/// <param name="context">The database context instance for accessing the auctions database.</param>
	private static void SeedData(AuctionDbContext context)
	{
		context.Database.Migrate();

		if (context.Auctions.Any())
		{
			Console.WriteLine("Already have data - no need to seed");
			return;
		}

		var auctions = new List<Auction>()
		{
			// 1 Ford GT
			new()
			{
				Id = Guid.Parse("afbee524-5972-4075-8800-7d1f9d7b0a0c"),
				Status = Status.Live,
				ReservePrice = 20000,
				Seller = "bob",
				AuctionEnd = DateTime.UtcNow.AddMinutes(30),
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
			new()
			{
				Id = Guid.Parse("c8c3ec17-01bf-49db-82aa-1ef80b833a9f"),
				Status = Status.Live,
				ReservePrice = 90000,
				Seller = "alice",
				AuctionEnd = DateTime.UtcNow.AddSeconds(45),
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
			new()
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
			},
			// 4 Mercedes SLK
			new()
			{
				Id = Guid.Parse("155225c1-4448-4066-9886-6786536e05ea"),
				Status = Status.ReserveNotMet,
				ReservePrice = 50000,
				Seller = "tom",
				Winner = "bob",
				AuctionEnd = DateTime.UtcNow.AddDays(-10),
				Item = new Item
				{
					Make = "Mercedes",
					Model = "SLK",
					Color = "Silver",
					Mileage = 15001,
					Year = 2020,
					ImageUrl = "https://cdn.pixabay.com/photo/2016/04/17/22/10/mercedes-benz-1335674_960_720.png"
				}
			},
			// 5 BMW X1
			new()
			{
				Id = Guid.Parse("466e4744-4dc5-4987-aae0-b621acfc5e39"),
				Status = Status.Live,
				ReservePrice = 20000,
				Seller = "alice",
				AuctionEnd = DateTime.UtcNow.AddDays(30),
				Item = new Item
				{
					Make = "BMW",
					Model = "X1",
					Color = "White",
					Mileage = 90000,
					Year = 2017,
					ImageUrl = "https://cdn.pixabay.com/photo/2017/08/31/05/47/bmw-2699538_960_720.jpg"
				}
			},
			// 6 Ferrari spider
			new()
			{
				Id = Guid.Parse("dc1e4071-d19d-459b-b848-b5c3cd3d151f"),
				Status = Status.Live,
				ReservePrice = 20000,
				Seller = "bob",
				AuctionEnd = DateTime.UtcNow.AddDays(45),
				Item = new Item
				{
					Make = "Ferrari",
					Model = "Spider",
					Color = "Red",
					Mileage = 50000,
					Year = 2015,
					ImageUrl = "https://cdn.pixabay.com/photo/2017/11/09/01/49/ferrari-458-spider-2932191_960_720.jpg"
				}
			},
			// 7 Ferrari F-430
			new()
			{
				Id = Guid.Parse("47111973-d176-4feb-848d-0ea22641c31a"),
				Status = Status.Live,
				ReservePrice = 150000,
				Seller = "alice",
				AuctionEnd = DateTime.UtcNow.AddDays(13),
				Item = new Item
				{
					Make = "Ferrari",
					Model = "F-430",
					Color = "Red",
					Mileage = 5000,
					Year = 2022,
					ImageUrl = "https://cdn.pixabay.com/photo/2017/11/08/14/39/ferrari-f430-2930661_960_720.jpg"
				}
			},
			// 8 Audi R8
			new()
			{
				Id = Guid.Parse("6a5011a1-fe1f-47df-9a32-b5346b289391"),
				Status = Status.Live,
				Seller = "bob",
				AuctionEnd = DateTime.UtcNow.AddDays(19),
				Item = new Item
				{
					Make = "Audi",
					Model = "R8",
					Color = "White",
					Mileage = 10050,
					Year = 2021,
					ImageUrl = "https://cdn.pixabay.com/photo/2019/12/26/20/50/audi-r8-4721217_960_720.jpg"
				}
			},
			// 9 Audi TT
			new()
			{
				Id = Guid.Parse("40490065-dac7-46b6-acc4-df507e0d6570"),
				Status = Status.Live,
				ReservePrice = 20000,
				Seller = "tom",
				AuctionEnd = DateTime.UtcNow.AddDays(20),
				Item = new Item
				{
					Make = "Audi",
					Model = "TT",
					Color = "Black",
					Mileage = 25400,
					Year = 2020,
					ImageUrl = "https://cdn.pixabay.com/photo/2016/09/01/15/06/audi-1636320_960_720.jpg"
				}
			},
			// 10 Ford Model T
			new()
			{
				Id = Guid.Parse("678650ee-bb8c-4adb-9f00-183ee05da8d2"),
				Status = Status.Live,
				ReservePrice = 20000,
				Seller = "alice",
				AuctionEnd = DateTime.UtcNow.AddMinutes(1),
				Item = new Item
				{
					Make = "Toyota",
					Model = "Camry",
					Color = "Red",
					Mileage = 150150,
					Year = 2001,
					ImageUrl = "https://cdn.pixabay.com/photo/2014/05/18/19/13/toyota-347288_1280.jpg"
				}
			},
			new()
			{
				Id = Guid.Parse("3659ac24-29dd-407a-81f5-ecfe6f924b9b"),
				Status = Status.Live,
				ReservePrice = 20000,
				Seller = "tom",
				AuctionEnd = DateTime.UtcNow.AddMinutes(11),
				Item = new Item
				{
					Make = "Lexus",
					Model = "ES 350",
					Color = "Gray",
					Mileage = 88000,
					Year = 2016,
					ImageUrl = "https://cdn.pixabay.com/photo/2023/01/12/15/30/car-7714372_1280.jpg"
				}
			},
			new()
			{
				Id = Guid.Parse("24e79e07-5278-492b-ab60-122b8d5220f1"),
				Status = Status.Live,
				ReservePrice = 20000,
				Seller = "alice",
				AuctionEnd = DateTime.UtcNow.AddMinutes(11),
				Item = new Item
				{
					Make = "Dodge",
					Model = "Sprinter",
					Color = "White",
					Mileage = 150150,
					Year = 2009,
					ImageUrl = "https://cdn.pixabay.com/photo/2010/12/16/20/56/van-3676_1280.jpg"
				}
			},
			new Auction
			{
				Id = Guid.Parse("b1239818-2414-4844-be7c-e74e048cda7d"),
				Status = Status.Live,
				ReservePrice = 20000,
				Seller = "bob",
				AuctionEnd = DateTime.UtcNow.AddMinutes(11),
				Item = new Item
				{
					Make = "Volkswagen",
					Model = "Bus",
					Color = "Yellow",
					Mileage = 150150,
					Year = 1971,
					ImageUrl = "https://cdn.pixabay.com/photo/2022/07/31/20/32/volkswagen-7356817_1280.jpg"
				}
			},
			new Auction
			{
				Id = Guid.Parse("0bab0135-d91d-4120-a1b9-83e6da262957"),
				Status = Status.Finished,
				ReservePrice = 20000,
				Seller = "alice",
				Winner = "bob",
				SoldAmount = 21000,
				AuctionEnd = DateTime.UtcNow.AddMinutes(-11),
				Item = new Item
				{
					Make = "Ford",
					Model = "Model T",
					Color = "Rust",
					Mileage = 150150,
					Year = 1938,
					ImageUrl = "https://cdn.pixabay.com/photo/2014/07/13/19/45/edsel-ranger-392745_1280.jpg"
				}
			}
		};

		context.Auctions.AddRange(auctions);

		context.SaveChanges();
	}
}