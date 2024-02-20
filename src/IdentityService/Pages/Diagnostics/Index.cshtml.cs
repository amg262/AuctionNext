// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;

namespace IdentityService.Pages.Diagnostics;

[SecurityHeaders]
[Authorize]
public class Index : PageModel
{
	public ViewModel View { get; set; } = default!;

	public async Task<IActionResult> OnGet()
	{
		var localAddresses = new List<string?>
		{
			"::ffff:172.18.0.5", "::ffff:172.20.0.5", "127.0.0.1", "::1",
			HttpContext.Connection.LocalIpAddress?.ToString()
		};
		Console.WriteLine("wE ARE HeRe");
		if (HttpContext.Connection.LocalIpAddress != null)
		{
			localAddresses.Add(HttpContext.Connection.LocalIpAddress.ToString());
		}

		// if (!localAddresses.Contains(HttpContext.Connection.RemoteIpAddress?.ToString()))
		// {
		// 	return NotFound();
		// }

		View = new ViewModel(await HttpContext.AuthenticateAsync());

		return Page();
	}
}