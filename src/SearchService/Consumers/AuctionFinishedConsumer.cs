using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Consumers;

/// <summary>
/// Consumes 'AuctionFinished' messages to update the status of auctions within the search service's data store.
/// </summary>
public class AuctionFinishedConsumer : IConsumer<AuctionFinished>
{
	/// <summary>
	/// Processes the 'AuctionFinished' message, updating the auction's status, winner, and sold amount if the item was sold.
	/// </summary>
	/// <param name="context">The consume context containing the 'AuctionFinished' message.</param>
	/// <returns>A task representing the asynchronous operation of processing the message.</returns>
	/// <remarks>
	/// Logs the processing of an 'AuctionFinished' message.
	/// Updates the auction's status to "Finished" and, if the item was sold, updates the winner and sold amount in the database.
	/// </remarks>
	public async Task Consume(ConsumeContext<AuctionFinished> context)
	{
		Console.WriteLine($"--> Search - AuctionFinishedConsumer: {context.Message.AuctionId}");
		var auction = DB.Find<Item>().OneAsync(context.Message.AuctionId).Result;

		if (context.Message.ItemSold)
		{
			auction.Winner = context.Message.Winner;
			auction.SoldAmount = (int) context.Message.Amount;
		}

		auction.Status = "Finished";
		await auction.SaveAsync();
	}
}