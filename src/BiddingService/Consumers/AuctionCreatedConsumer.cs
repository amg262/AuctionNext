using BiddingService.Models;
using Contracts;
using MassTransit;
using MongoDB.Entities;

namespace BiddingService.Consumers;

/// <summary>
/// Consumes "AuctionCreated" messages, creating and saving a new auction entity
/// to the database whenever an auction creation event is received.
/// </summary>
/// <remarks>
/// This consumer listens for "AuctionCreated" events published on the message bus.
/// Upon receiving an event, it initiates the creation of a new Auction record
/// in the database with the details provided in the message. This is a critical
/// part of maintaining the auction state, ensuring that newly created auctions
/// are immediately available for bidding and other operations within the system.
/// 
/// It's important to handle failures gracefully in a production environment,
/// including retrying operations or logging failures, to ensure system resilience.
/// </remarks>
public class AuctionCreatedConsumer : IConsumer<AuctionCreated>
{
	/// <summary>
	/// Handles the consumption of an "AuctionCreated" message.
	/// </summary>
	/// <param name="context">The consume context containing the message data.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
	/// <remarks>
	/// The method extracts auction details from the received message and creates
	/// a new Auction object. It then asynchronously saves this object to the database.
	/// 
	/// Attention should be given to the asynchronous nature of the SaveAsync method.
	/// Depending on the database load and network conditions, the operation might
	/// have varying completion times. Proper error handling and possibly a retry
	/// mechanism should be considered to handle scenarios where the database operation
	/// fails.
	/// 
	/// The ID of the auction is directly taken from the message and converted to a string,
	/// assuming the message ID format is compatible with the database's requirements.
	/// This direct use implies trust in the format and uniqueness of the incoming message ID.
	/// </remarks>
	public async Task Consume(ConsumeContext<AuctionCreated> context)
	{
		var auction = new Auction
		{
			ID = context.Message.Id.ToString(),
			Seller = context.Message.Seller,
			AuctionEnd = context.Message.AuctionEnd,
			ReservePrice = context.Message.ReservePrice
		};

		await auction.SaveAsync();
	}
}