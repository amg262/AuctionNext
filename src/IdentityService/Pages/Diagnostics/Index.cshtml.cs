using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;

namespace IdentityService.Pages.Diagnostics;

[SecurityHeaders]
[Authorize]
public class Index : PageModel
{
	public ViewModel View { get; set; }

	public async Task<IActionResult> OnGet()
	{
		var localAddresses = new string[]
		{
			"::ffff:172.18.0.1", "127.0.0.1", "::1", "146.190.34.72", "::ffff:10.5.0.200", "::ffff:10.5.0.7",
			"::ffff:172.19.0.4:80", "::ffff:172.19.0.2", "::ffff:172.22.0.4", "::ffff:172.22.0.2",
			HttpContext.Connection.LocalIpAddress.ToString()
		};

		Console.WriteLine($"Local IP: {HttpContext.Connection.LocalIpAddress}");
		Console.WriteLine($"Remote IP: {HttpContext.Connection.RemoteIpAddress}");
		
		if (!localAddresses.Contains(HttpContext.Connection.RemoteIpAddress.ToString()))
		{
			// return NotFound();
		}

		View = new ViewModel(await HttpContext.AuthenticateAsync());

		return Page();
	}
}