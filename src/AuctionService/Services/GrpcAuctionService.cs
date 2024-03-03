using AuctionService.Data;
using Grpc.Core;

namespace AuctionService.Services;

/// <summary>
/// Implements the gRPC service for auction operations.
/// </summary>
public class GrpcAuctionService : GrpcAuction.GrpcAuctionBase
{
	private readonly AuctionDbContext _dbContext;

	/// <summary>
	/// Initializes a new instance of the <see cref="GrpcAuctionService"/> class.
	/// </summary>
	/// <param name="dbContext">The database context to access auctions.</param>
	public GrpcAuctionService(AuctionDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	/// <summary>
	/// Gets the details of an auction by its ID.
	/// </summary>
	/// <param name="request">The request containing the ID of the auction to retrieve.</param>
	/// <param name="context">The server call context.</param>
	/// <returns>A task that represents the asynchronous operation, containing the auction details.</returns>
	/// <exception cref="RpcException">Thrown when the auction with the specified ID is not found.</exception>
	public override async Task<GrpcAuctionResponse> GetAuction(GetAuctionRequest request,
		ServerCallContext context)
	{
		Console.WriteLine("==> Received Grpc request for auction");

		var auction = await _dbContext.Auctions.FindAsync(Guid.Parse(request.Id))
		              ?? throw new RpcException(new Status(StatusCode.NotFound, "Not found"));

		var response = new GrpcAuctionResponse
		{
			Auction = new GrpcAuctionModel
			{
				AuctionEnd = auction.AuctionEnd.ToString(),
				Id = auction.Id.ToString(),
				ReservePrice = auction.ReservePrice,
				Seller = auction.Seller
			}
		};

		return response;
	}
}