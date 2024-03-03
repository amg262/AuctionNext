using System.Security.Claims;

namespace AuctionService.UnitTests.Utils;

/// <summary>
/// Provides utility methods and constants for unit testing, including user authentication helpers.
/// </summary>
public static class Helpers
{
	/// <summary>
	/// Creates a <see cref="ClaimsPrincipal"/> object for simulating user authentication in unit tests.
	/// </summary>
	/// <returns>A <see cref="ClaimsPrincipal"/> with the specified user's name claim or the default test user if none is specified.</returns>
	/// <remarks>
	/// This method is particularly useful for testing components that require user context, such as
	/// authorization handlers or controllers with [Authorize] attributes. It allows unit tests to
	/// simulate authenticated user scenarios without needing to interact with the actual authentication system.
	/// </remarks>
	public static ClaimsPrincipal GetClaimsPrincipal()
	{
		var claims = new List<Claim> {new Claim(ClaimTypes.Name, "test")};
		var identity = new ClaimsIdentity(claims, "testing");
		return new ClaimsPrincipal(identity);
	}
}