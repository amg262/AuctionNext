using AutoMapper;
using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Consumers;

/// <summary>
/// Consumes 'AuctionUpdated' messages, updating corresponding item details in the database.
/// </summary>
public class AuctionUpdatedConsumer : IConsumer<AuctionUpdated>
{
	private readonly IMapper _mapper;

	/// <summary>
	/// Initializes a new instance of the <see cref="AuctionUpdatedConsumer"/> class.
	/// </summary>
	/// <param name="mapper">The AutoMapper instance used for mapping between DTOs and entity models.</param>
	public AuctionUpdatedConsumer(IMapper mapper)
	{
		_mapper = mapper;
	}

	/// <summary>
	/// Asynchronously consumes an 'AuctionUpdated' message, updating the corresponding item in the MongoDB database.
	/// </summary>
	/// <param name="context">The consume context providing access to the message and other message processing utilities.</param>
	/// <returns>A task representing the asynchronous operation of consuming and processing the message.</returns>
	/// <exception cref="MessageException">Thrown if the update operation is not acknowledged by MongoDB.</exception>
	/// <remarks>
	/// This method logs the consumption of an 'AuctionUpdated' message and attempts to update the corresponding item's details
	/// in the MongoDB database. It maps the received message to an <see cref="Item"/> entity and updates the item's properties.
	/// If the update operation is not acknowledged by MongoDB, a <see cref="MessageException"/> is thrown, indicating a problem with the update process.
	/// </remarks>
	public async Task Consume(ConsumeContext<AuctionUpdated> context)
	{
		Console.WriteLine("--> Consuming auction updated: " + context.Message.Id);

		var item = _mapper.Map<Item>(context.Message);

		var result = await DB.Update<Item>()
			.Match(a => a.ID == context.Message.Id)
			.ModifyOnly(x => new
			{
				x.Color,
				x.Make,
				x.Model,
				x.Year,
				x.Mileage
			}, item)
			.ExecuteAsync();

		if (!result.IsAcknowledged)
			throw new MessageException(typeof(AuctionUpdated), "Problem updating mongodb");
	}
}