using AutoMapper;
using Contracts;
using MassTransit;

namespace PostService.Consumers;

/// <summary>
/// Consumes 'AuctionCreated' messages from a message broker, using AutoMapper to map the message to an 'Item' entity,
/// and then saves the entity to a MongoDB database. This consumer specifically checks for a condition on the model name
/// before saving, demonstrating simple validation logic within a consumer.
/// </summary>
public class AuctionCreatedConsumer : IConsumer<AuctionCreated>
{
	private readonly IMapper _mapper;

	/// <summary>
	/// Initializes a new instance of the <see cref="AuctionCreatedConsumer"/> class with dependency injection for AutoMapper.
	/// </summary>
	/// <param name="mapper">The AutoMapper instance for mapping between the message contract and the MongoDB entity.</param>
	public AuctionCreatedConsumer(IMapper mapper)
	{
		_mapper = mapper;
	}

	/// <summary>
	/// Consumes an 'AuctionCreated' message, performing a mapping to the 'Item' entity and saving it to the database.
	/// If the item model is "Foo", an exception is thrown to demonstrate validation.
	/// </summary>
	/// <param name="context">The consume context providing access to the message and other properties related to the consume operation.</param>
	/// <returns>A <see cref="Task"/> representing the asynchronous operation of consuming the message and processing it.</returns>
	/// <remarks>
	/// This method logs the operation and demonstrates basic error handling by throwing an exception for a specific condition.
	/// Ensure proper error handling and logging mechanisms are in place in a real application to handle such exceptions gracefully.
	/// </remarks>
	public async Task Consume(ConsumeContext<AuctionCreated> context)
	{
		// Console.WriteLine("--> Consuming auction created: " + context.Message.Id);
		//
		// var item = _mapper.Map<Item>(context.Message);
		//
		// if (item.Model == "Foo") throw new ArgumentException("Cannot sell cars with name of Foo");
		//
		// await item.SaveAsync();
	}
}