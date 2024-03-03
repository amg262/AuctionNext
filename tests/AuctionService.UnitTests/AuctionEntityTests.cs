using AuctionService.Entities;

namespace AuctionService.UnitTests;

/// <summary>
/// Contains tests for verifying the behavior of the Auction entity, particularly focusing on the handling of reserve prices.
/// </summary>
public class AuctionEntityTests
{
	/// <summary>
	/// Tests whether the HasReservePrice method correctly identifies auctions with a reserve price greater than zero as having a reserve price.
	/// </summary>
	[Fact]
	public void HasReservePrice_ReservePriceGtZero_True()
	{
		// arrange
		var auction = new Auction {Id = Guid.NewGuid(), ReservePrice = 10};

		// act
		var result = auction.HasReservePrice();

		// assert
		Assert.True(result);
	}

	/// <summary>
	/// Tests whether the HasReservePrice method correctly identifies auctions with a reserve price of zero as not having a reserve price.
	/// </summary>
	[Fact]
	public void HasReservePrice_ReservePriceIsZero_False()
	{
		// arrange
		var auction = new Auction {Id = Guid.NewGuid(), ReservePrice = 0};

		// act
		var result = auction.HasReservePrice();

		// assert
		Assert.False(result);
	}
}