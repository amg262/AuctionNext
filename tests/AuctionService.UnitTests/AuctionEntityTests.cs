using AuctionService.Entities;

namespace AuctionService.UnitTests;

public class AuctionEntityTests
{
	[Fact]
	public void HasReservePrice_ReservePriceGtZero_True()
	{
		// Arrange
		var auction = new Auction
		{
			Id = Guid.NewGuid(),
			ReservePrice = 10,
		};

		// Act
		var hasReservePrice = auction.HasReservePrice();

		// Assert
		Assert.True(hasReservePrice);
	}
	
	[Fact]
	public void HasReservePrice_ReservePriceIsZero_False()
	{
		// Arrange
		var auction = new Auction
		{
			Id = Guid.NewGuid(),
			ReservePrice = 0,
		};

		// Act
		var hasReservePrice = auction.HasReservePrice();

		// Assert
		Assert.False(hasReservePrice);
	}
}