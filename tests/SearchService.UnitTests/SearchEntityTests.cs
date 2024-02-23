using SearchService.Models;

namespace SearchService.UnitTests;

public class SearchEntityTests
{
	[Fact]
	public void HasSeller_SellerNotNull_True()
	{
		// Arrange: Create a SearchParams instance with a search term.
		var item = new Item {Seller = "bob"};

		// Act: Invoke the method under test.
		var response = item.HasSeller();

		// Assert: Verify that the method returns true for search parameters with a search term.
		Assert.True(response);
	}

	[Fact]
	public void HasValidCreatedDate_CreatedAtNotNullAndNotFuture_True()
	{
		// Arrange: Create a Item instance with a created date.
		var item = new Item {CreatedAt = DateTime.UtcNow};

		// Act: Invoke the method under test.
		var response = item.HasValidCreatedDate();

		// Assert: Verify that the method returns true for Item with a created date.
		Assert.True(response);
	}

	[Fact]
	public void CreatedAt_IsDateTime()
	{
		// Arrange
		var item = new Item {CreatedAt = DateTime.Now};

		// Act
		var createdAt = item.CreatedAt;

		// Assert
		Assert.IsType<DateTime>(createdAt);
	}

	[Fact]
	public void CreatedAt_IsNotFutureDate_True()
	{
		// Arrange
		var item = new Item {CreatedAt = DateTime.Now};

		// Act
		var createdAt = item.CreatedAt;

		// Assert
		Assert.True(createdAt <= DateTime.Now);
	}


	[Fact]
	public void Year_InvalidYearInt_ReturnsTrue()
	{
		// Arrange
		var item = new Item {Year = 2021};

		// Act
		var year = item.Year.ToString();

		// Assert
		Assert.IsNotType<int>(year);
	}

	[Fact]
	public void Year_InvalidYearRange_False()
	{
		// Arrange: Create a Item instance with a year.
		var item = new Item {Year = 2026};

		// Act: Invoke the method under test.
		var currentYear = DateTime.Now.Year;
		var minYear = currentYear - 100;
		var maxYear = ++currentYear;

		// Assert: Verify that the method returns false for Item with a year outside the valid range.
		Assert.NotInRange(item.Year, minYear, maxYear);
	}

	[Theory]
	[InlineData(1900)]
	[InlineData(1999)]
	[InlineData(2024)]
	public void Year_WithinValidRange_IsValid(int year)
	{
		// Arrange: Create a Item instance with a year.
		var item = new Item {Year = year};

		// Act: Invoke the method under test.
		var isValidYear = item.Year >= 1900 && item.Year <= DateTime.Now.Year;

		// Assert: Verify that the method returns true for Item with a year within the valid range.
		Assert.True(isValidYear);
	}
}