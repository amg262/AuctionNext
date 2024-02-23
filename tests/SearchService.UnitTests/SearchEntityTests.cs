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
}