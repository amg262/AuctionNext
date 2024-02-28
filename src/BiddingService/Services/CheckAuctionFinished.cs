using BiddingService.Models;
using Contracts;
using MassTransit;
using MongoDB.Entities;

namespace BiddingService.Services;

/// <summary>
/// A background service that periodically checks for auctions that have finished
/// and publishes an event for each auction that has concluded.
/// </summary>
public class CheckAuctionFinished : BackgroundService
{
	private readonly ILogger<CheckAuctionFinished> _logger;
	private readonly IServiceProvider _serviceProvider;

	/// <summary>
	/// Initializes a new instance of the <see cref="CheckAuctionFinished"/> class.
	/// </summary>
	/// <param name="logger">The logger used for logging information and errors.</param>
	/// <param name="serviceProvider">The service provider used for accessing application services.</param>
	public CheckAuctionFinished(ILogger<CheckAuctionFinished> logger, IServiceProvider serviceProvider)
	{
		_logger = logger;
		_serviceProvider = serviceProvider;
	}

	/// <summary>
	/// Executes the background task to check for finished auctions.
	/// This method runs in a loop until a stop request is received.
	/// </summary>
	/// <param name="stoppingToken">The token to monitor for stopping requests.</param>
	/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		_logger.LogInformation("CheckAuctionFinished is starting");

		stoppingToken.Register(() => _logger.LogInformation("CheckAuctionFinished is stopping"));

		while (!stoppingToken.IsCancellationRequested)
		{
			await CheckAuctions(stoppingToken);
			await Task.Delay(5000, stoppingToken);
		}
	}

	/// <summary>
	/// Checks the database for auctions that have finished but not yet been processed,
	/// marks them as finished, and publishes an event for each.
	/// </summary>
	/// <param name="stoppingToken">The token to monitor for stopping requests.</param>
	/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
	private async Task CheckAuctions(CancellationToken stoppingToken)
	{
		var finsihedAuctions = await DB.Find<Auction>()
			.Match(x => x.AuctionEnd <= DateTime.UtcNow)
			.Match(x => !x.Finished)
			.ExecuteAsync(stoppingToken);

		if (finsihedAuctions.Count == 0) return;

		_logger.LogInformation("==> Found {Count} auctions that have finished", finsihedAuctions.Count);

		using var scope = _serviceProvider.CreateScope();
		var endpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();

		foreach (var auction in finsihedAuctions)
		{
			auction.Finished = true;
			await auction.SaveAsync(null, stoppingToken);

			var winningBid = await DB.Find<Bid>()
				.Match(x => x.AuctionId == auction.ID)
				.Match(b => b.BidStatus == BidStatus.Accepted)
				.Sort(x => x.Descending(s => s.Amount))
				.ExecuteFirstAsync(stoppingToken);

			await endpoint.Publish(new AuctionFinished
			{
				ItemSold = winningBid != null,
				AuctionId = auction.ID,
				Winner = winningBid?.Bidder,
				Amount = winningBid?.Amount,
				Seller = auction.Seller
			}, stoppingToken);
		}
	}
}