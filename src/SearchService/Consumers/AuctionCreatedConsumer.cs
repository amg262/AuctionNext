using AutoMapper;
using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Consumers;

/// <summary>
/// A consumer that handles 'AuctionCreated' messages, mapping them to the 'Item' entity and saving them to the database.
/// Initializes a new instance of the <see cref="AuctionCreatedConsumer"/> with the necessary mapping configuration.
/// </summary>
/// <param name="mapper">The AutoMapper instance used for mapping between DTOs and entities.</param>
public class AuctionCreatedConsumer(IMapper mapper) : IConsumer<AuctionCreated>
{
	/// <summary>
	/// Consumes an 'AuctionCreated' message, mapping it to an 'Item' entity and saving it to the MongoDB database.
	/// </summary>
	/// <param name="context">The consume context containing the 'AuctionCreated' message.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
	/// <remarks>
	/// This method is automatically called by MassTransit when an 'AuctionCreated' message is received.
	/// It logs the message consumption, maps the received 'AuctionCreated' message to an 'Item' entity,
	/// and saves the entity asynchronously to the MongoDB database.
	/// </remarks>
	public async Task Consume(ConsumeContext<AuctionCreated> context)
	{
		Console.WriteLine($"--> Consuming auction created: {context.Message.Id}");
		var item = mapper.Map<Item>(context.Message);
		await item.SaveAsync();
	}
}