using AutoMapper;
using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Consumers;

/// <summary>
/// A consumer responsible for handling 'AuctionDeleted' messages. 
/// When an auction is deleted, this consumer removes the corresponding item from the MongoDB database.
/// </summary>
public class AuctionDeletedConsumer : IConsumer<AuctionDeleted>
{
	/// <summary>
	/// Asynchronously consumes an 'AuctionDeleted' message, removing the corresponding item from the database.
	/// </summary>
	/// <param name="context">The consume context providing access to the message and other utilities for message processing.</param>
	/// <returns>A task representing the asynchronous operation of consuming and processing the message.</returns>
	/// <exception cref="MessageException">Thrown if the deletion operation is not acknowledged by MongoDB, 
	/// indicating a problem with the deletion process.</exception>
	/// <remarks>
	/// This method logs the consumption of an 'AuctionDeleted' message and attempts to delete the corresponding item 
	/// from the MongoDB database based on the auction ID provided in the message. If the deletion operation is not 
	/// acknowledged by MongoDB, it throws a <see cref="MessageException"/>, signaling an issue with the deletion process.
	/// </remarks>
	public async Task Consume(ConsumeContext<AuctionDeleted> context)
	{
		Console.WriteLine("--> Consuming AuctionDeleted: " + context.Message.Id);

		var result = await DB.DeleteAsync<Item>(context.Message.Id);

		if (!result.IsAcknowledged)
			throw new MessageException(typeof(AuctionDeleted), "Problem deleting auction");
	}
}