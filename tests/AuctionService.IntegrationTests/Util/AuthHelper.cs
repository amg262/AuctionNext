using System.Security.Claims;

namespace AuctionService.IntegrationTests.Util;

/// <summary>
/// Provides helper methods for authentication related tasks in integration tests.
/// </summary>
public static class AuthHelper
{
	/// <summary>
	/// Generates a dictionary representing a fake JWT token payload for a specified user.
	/// </summary>
	/// <param name="username">The username to include in the token payload.</param>
	/// <returns>A dictionary with the user's name claim.</returns>
	/// <remarks>
	/// This method is useful for creating mock authentication tokens in tests where
	/// the actual authentication process is bypassed, and the focus is on authorization and functionality.
	/// </remarks>
	public static Dictionary<string, object> GetBearerForUser(string username)
	{
		return new Dictionary<string, object> {{ClaimTypes.Name, username}};
	}
}