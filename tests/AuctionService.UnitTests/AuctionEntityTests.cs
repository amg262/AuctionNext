using AuctionService.Entities;
using AuctionService.UnitTests.Util;

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
		// Arrange: Create an Auction instance with a reserve price greater than zero.
		var auction = new Auction
		{
			Id = Guid.NewGuid(),
			ReservePrice = 10, // Set a reserve price greater than zero to test the positive scenario.
		};

		// Act: Invoke the method under test.
		var hasReservePrice = auction.HasReservePrice();

		// Assert: Verify that the method returns true for auctions with a reserve price greater than zero.
		Assert.True(hasReservePrice);
	}

	/// <summary>
	/// Tests whether the HasReservePrice method correctly identifies auctions with a reserve price of zero as not having a reserve price.
	/// </summary>
	[Fact]
	public void HasReservePrice_ReservePriceIsZero_False()
	{
		// Arrange: Create an Auction instance with a reserve price of zero.
		var auction = new Auction
		{
			Id = Guid.NewGuid(), // Assign a unique identifier.
			ReservePrice = 0, // Set the reserve price to zero to test the negative scenario.
		};

		// Act: Invoke the method under test.
		var hasReservePrice = auction.HasReservePrice();

		// Assert: Verify that the method returns false for auctions without a reserve price (i.e., reserve price is zero).
		Assert.False(hasReservePrice);
	}
}