using System.Security.Claims;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IdentityModel;
using IdentityService.Models;
using Microsoft.AspNetCore.Identity;

namespace IdentityService.Services;

/// <summary>
/// Implements the profile service to integrate with ASP.NET Identity for user profile data.
/// </summary>
public class CustomProfileService : IProfileService
{
	private readonly UserManager<ApplicationUser> _userManager;

	/// <summary>
	/// Initializes a new instance of <see cref="CustomProfileService"/> with a user manager.
	/// </summary>
	/// <param name="userManager">The ASP.NET Identity UserManager for accessing user information.</param>
	public CustomProfileService(UserManager<ApplicationUser> userManager)
	{
		_userManager = userManager;
	}

	/// <summary>
	/// Asynchronously gets profile data for a user when requested by IdentityServer, adding custom claims to the issued claims.
	/// </summary>
	/// <param name="context">The context containing information about the profile data request.</param>
	/// <remarks>
	/// This method fetches the user from the database based on the subject identifier and adds specific claims to the
	/// issued claims collection in the profile data request context. It includes a default 'username' claim and attempts
	/// to include the 'name' claim if it exists among the user's claims.
	/// </remarks>
	public async Task GetProfileDataAsync(ProfileDataRequestContext context)
	{
		var user = await _userManager.GetUserAsync(context.Subject);

		IList<Claim?> existingClaims = await _userManager.GetClaimsAsync(user);


		var claims = new List<Claim>
		{
			new("username", user?.UserName),
		};

		context.IssuedClaims.AddRange(claims);
		context.IssuedClaims.Add(existingClaims.FirstOrDefault(x => x.Type == JwtClaimTypes.Name));
	}

	/// <summary>
	/// Checks if the user is active. Currently, this implementation always completes the task successfully without any checks.
	/// </summary>
	/// <param name="context">The context containing information about the activity check.</param>
	/// <remarks>
	/// This method is a placeholder for implementing user activity checks. It completes successfully by default,
	/// indicating that the user is considered active. Custom logic to determine the user's active status can be added as needed.
	/// </remarks>
	public Task IsActiveAsync(IsActiveContext context)
	{
		return Task.CompletedTask;
	}
}