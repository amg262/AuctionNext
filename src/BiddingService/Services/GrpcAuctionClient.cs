using AuctionService;
using BiddingService.Models;
using Grpc.Net.Client;

namespace BiddingService.Services;

/// <summary>
/// Provides functionality to communicate with the Auction service via gRPC for retrieving auction details.
/// This client encapsulates the complexity of setting up a gRPC channel and making requests to the gRPC server.
/// </summary>
public class GrpcAuctionClient
{
	private readonly ILogger<GrpcAuctionClient> _logger;
	private readonly IConfiguration _config;

	/// <summary>
	/// Initializes a new instance of the <see cref="GrpcAuctionClient"/> class.
	/// </summary>
	/// <param name="logger">The logger used for logging information and errors.</param>
	/// <param name="config">The configuration provider for accessing application settings.</param>
	public GrpcAuctionClient(ILogger<GrpcAuctionClient> logger, IConfiguration config)
	{
		_logger = logger;
		_config = config;
	}

	/// <summary>
	/// Retrieves auction details by its identifier using a gRPC call.
	/// </summary>
	/// <param name="id">The unique identifier of the auction to retrieve.</param>
	/// <returns>An instance of <see cref="Auction"/> containing the auction details if the call is successful; otherwise, null.</returns>
	/// <remarks>
	/// This method logs the initiation of the call and errors if the call fails.
	/// It demonstrates a synchronous gRPC call pattern within a .NET application.
	/// In a production scenario, consider using asynchronous methods for better scalability.
	/// </remarks>
	public Auction GetAuction(string id)
	{
		_logger.LogInformation("Calling GRPC Service");
		var channel = GrpcChannel.ForAddress(_config["GrpcAuction"]);
		var client = new GrpcAuction.GrpcAuctionClient(channel);
		var request = new GetAuctionRequest {Id = id};

		try
		{
			var reply = client.GetAuction(request);
			var auction = new Auction
			{
				ID = reply.Auction.Id,
				AuctionEnd = DateTime.Parse(reply.Auction.AuctionEnd),
				Seller = reply.Auction.Seller,
				ReservePrice = reply.Auction.ReservePrice
			};

			return auction;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Could not call GRPC Server");
			return null;
		}
	}
}