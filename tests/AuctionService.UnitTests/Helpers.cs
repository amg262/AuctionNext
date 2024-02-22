using System.Security.Claims;

namespace AuctionService.UnitTests;

public static class Helpers
{
	public const string TestUser = "test";
	public const string InvalidUser = "not-test";
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