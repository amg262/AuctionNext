using Duende.IdentityServer;
using Duende.IdentityServer.Services;
using IdentityService.Data;
using IdentityService.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace IdentityService;

internal static class HostingExtensions
{
	public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
	{
		builder.Services.AddRazorPages();

		builder.Services.AddDbContext<ApplicationDbContext>(options =>
			options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

		builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
			.AddEntityFrameworkStores<ApplicationDbContext>()
			.AddDefaultTokenProviders();

		builder.Services
			.AddIdentityServer(options =>
			{
				options.Events.RaiseErrorEvents = true;
				options.Events.RaiseInformationEvents = true;
				options.Events.RaiseFailureEvents = true;
				options.Events.RaiseSuccessEvents = true;
				options.LicenseKey = builder.Configuration["IdentityServer:LicenseKey"];


				if (builder.Environment.IsEnvironment("Docker"))
				{
					options.IssuerUri = "identity-svc";
				}

				if (builder.Environment.IsProduction())
				{
					options.IssuerUri = "https://id.milwaukeesoftware.net";
				}

				// see https://docs.duendesoftware.com/identityserver/v6/fundamentals/resources/
				// options.EmitStaticAudienceClaim = true;
			})
			.AddInMemoryIdentityResources(Config.IdentityResources)
			.AddInMemoryApiScopes(Config.ApiScopes)
			.AddInMemoryClients(Config.Clients(builder.Configuration))
			.AddAspNetIdentity<ApplicationUser>()
			.AddProfileService<CustomProfileService>();

		builder.Services.ConfigureApplicationCookie(options => { options.Cookie.SameSite = SameSiteMode.Lax; });

		builder.Services.AddAuthentication()
			.AddMicrosoftAccount("Microsoft", options =>
			{
				options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
				options.ClientId = builder.Configuration["Microsoft:ClientId"];
				options.ClientSecret = builder.Configuration["Microsoft:ClientSecret"];
				options.CallbackPath = "/api/auth/callback/id-server";
			})
			.AddGoogle("Google", options =>
			{
				options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
				// options.ReturnUrlParameter = "/api/auth/callback/id-server";
				// Register your IdentityServer with Google at https://console.developers.google.com
				// Enable the Google+ API
				// Set the redirect URI to https://localhost:5001/signin-google
				options.CallbackPath = "/api/auth/callback/id-server";
				options.ClientId = builder.Configuration["Google:ClientId"];
				options.ClientSecret = builder.Configuration["Google:ClientSecret"];
			});
	
			
		return builder.Build();
	}

	public static WebApplication ConfigurePipeline(this WebApplication app)
	{
		app.UseSerilogRequestLogging();

		if (app.Environment.IsDevelopment())
		{
			app.UseDeveloperExceptionPage();
		}

		app.UseStaticFiles();
		app.UseRouting();

		if (app.Environment.IsProduction())
		{
			app.Use(async (ctx, next) =>
			{
				var serverUrls = ctx.RequestServices.GetRequiredService<IServerUrls>();
				serverUrls.Origin = serverUrls.Origin = "https://id.milwaukeesoftware.net";
				await next();
			});
		}

		app.UseIdentityServer();
		app.UseAuthentication();
		app.UseAuthorization();

		app.MapRazorPages()
			.RequireAuthorization();

		return app;
	}
}