using AuctionService.Data;
using AuctionService.Entities;
using Contracts;
using MassTransit;

namespace AuctionService.Consumers;

/// <summary>
/// A consumer that handles 'BidPlaced' messages, updating the current high bid of an auction based on the bid information provided.
/// </summary>
public class BidPlacedConsumer : IConsumer<BidPlaced>
{
	private readonly AuctionDbContext _dbContext;

	/// <summary>
	/// Initializes a new instance of the <see cref="BidPlacedConsumer"/> class with a database context.
	/// </summary>
	/// <param name="dbContext">The database context used for accessing and updating auction data.</param>
	public BidPlacedConsumer(AuctionDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	/// <summary>
	/// Consumes a 'BidPlaced' message, potentially updating the current high bid for the associated auction.
	/// </summary>
	/// <param name="context">The consume context providing access to the message data and other utilities for message processing.</param>
	/// <returns>A task representing the asynchronous operation of consuming and processing the message.</returns>
	/// <remarks>
	/// This method logs the reception of a 'BidPlaced' message and checks if the bid is accepted and higher than the current high bid.
	/// If so, it updates the auction's current high bid with the amount from the message and saves the changes to the database.
	/// </remarks>
	public async Task Consume(ConsumeContext<BidPlaced> context)
	{
		Console.WriteLine(
			$"--> Consuming BidPlaced: {context.Message.AuctionId} - {context.Message.Bidder} - {context.Message.Amount}");
		
		var auction = await _dbContext.Auctions.FindAsync(Guid.Parse(context.Message.AuctionId));

		if (auction.CurrentHighBid == null || context.Message.BidStatus.Contains("Accepted") &&
		    context.Message.Amount > auction.CurrentHighBid)
		{
			auction.CurrentHighBid = context.Message.Amount;
			await _dbContext.SaveChangesAsync();
		}
	}
}