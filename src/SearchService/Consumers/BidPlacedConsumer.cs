using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Consumers;

/// <summary>
/// Consumes 'BidPlaced' messages to update the current high bid for auctions within the search service's data store.
/// </summary>
public class BidPlacedConsumer : IConsumer<BidPlaced>
{
	/// <summary>
	/// Processes the 'BidPlaced' message, updating the auction's current high bid if the bid is accepted and higher than the current high bid.
	/// </summary>
	/// <param name="context">The consume context containing the 'BidPlaced' message.</param>
	/// <returns>A task representing the asynchronous operation of processing the message.</returns>
	/// <remarks>
	/// Logs the processing of a 'BidPlaced' message and checks if the bid is accepted and exceeds the current high bid.
	/// If conditions are met, updates the current high bid for the auction in the database.
	/// </remarks>
	public async Task Consume(ConsumeContext<BidPlaced> context)
	{
		Console.WriteLine("--> Consuming bid placed");

		var auction = await DB.Find<Item>().OneAsync(context.Message.AuctionId);

		if (context.Message.BidStatus.Contains("Accepted")
		    && context.Message.Amount > auction.CurrentHighBid)
		{
			auction.CurrentHighBid = context.Message.Amount;
			await auction.SaveAsync();
		}
	}
}