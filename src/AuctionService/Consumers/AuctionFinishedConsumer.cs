using AuctionService.Data;
using AuctionService.Entities;
using Contracts;
using MassTransit;

namespace AuctionService.Consumers;

/// <summary>
/// Consumes <see cref="AuctionFinished"/> messages to update auction entities in the database
/// based on the auction completion details provided in the message.
/// </summary>
public class AuctionFinishedConsumer : IConsumer<AuctionFinished>
{
	private readonly AuctionDbContext _dbContext;

	/// <summary>
	/// Initializes a new instance of the <see cref="AuctionFinishedConsumer"/> class with a database context.
	/// </summary>
	/// <param name="dbContext">The database context used for data access and manipulation.</param>
	public AuctionFinishedConsumer(AuctionDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	/// <summary>
	/// Asynchronously handles an <see cref="AuctionFinished"/> message, updating the corresponding auction's status,
	/// winner, and sold amount in the database.
	/// </summary>
	/// <param name="context">The consume context providing access to the <see cref="AuctionFinished"/> message.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
	/// <remarks>
	/// Upon receiving an <see cref="AuctionFinished"/> message, this method updates the corresponding auction record in the database.
	/// If the item was sold, it records the winner's identity and the final sold amount. Then, it updates the auction's status
	/// based on whether the sold amount meets or exceeds the reserve price. Changes are saved to the database.
	/// </remarks>
	public async Task Consume(ConsumeContext<AuctionFinished> context)
	{
		Console.WriteLine("--> Consuming auction finished");

		var auction = await _dbContext.Auctions.FindAsync(Guid.Parse(context.Message.AuctionId));

		if (context.Message.ItemSold)
		{
			auction.Winner = context.Message.Winner;
			auction.SoldAmount = context.Message.Amount;
		}

		auction.Status = auction.SoldAmount > auction.ReservePrice
			? Status.Finished
			: Status.ReserveNotMet;

		await _dbContext.SaveChangesAsync();
	}
}