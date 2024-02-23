using System.Security.Claims;

namespace AuctionService.UnitTests.Util;

/// <summary>
/// Provides utility methods and constants for unit testing, including user authentication helpers.
/// </summary>
public static class Helpers
{
	public const string TestUser = "test";
	public const string InvalidUser = "not-test";

	/// <summary>
	/// Creates a <see cref="ClaimsPrincipal"/> object for simulating user authentication in unit tests.
	/// </summary>
	/// <param name="name">The username to include in the claims principal. If null, the <see cref="TestUser"/> is used.</param>
	/// <returns>A <see cref="ClaimsPrincipal"/> with the specified user's name claim or the default test user if none is specified.</returns>
	/// <remarks>
	/// This method is particularly useful for testing components that require user context, such as
	/// authorization handlers or controllers with [Authorize] attributes. It allows unit tests to
	/// simulate authenticated user scenarios without needing to interact with the actual authentication system.
	/// </remarks>
	public static ClaimsPrincipal GetClaimsPrincipal(string? name)
	{
		var claims = new List<Claim>
		{
			new(ClaimTypes.Name, name ?? TestUser),
		};

		var identity = new ClaimsIdentity(claims, "TestAuthType");
		var claimsPrincipal = new ClaimsPrincipal(identity);

		return claimsPrincipal;
	}
}