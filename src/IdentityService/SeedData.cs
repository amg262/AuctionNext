using System.Security.Claims;
using IdentityModel;
using IdentityService.Data;
using IdentityService.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace IdentityService;

public class SeedData
{
    public static void EnsureSeedData(WebApplication app)
    {
        using var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
        context.Database.Migrate();

        var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        if (userMgr.Users.Any()) return;

        var alice = userMgr.FindByNameAsync("alice").Result;
        if (alice == null)
        {
            alice = new ApplicationUser
            {
                UserName = "alice",
                Email = "AliceSmith@email.com",
                EmailConfirmed = true,
            };
            var result = userMgr.CreateAsync(alice, "Pass123$").Result;
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            result = userMgr.AddClaimsAsync(alice, new Claim[]
            {
                new(JwtClaimTypes.Name, "Alice Smith"),
            }).Result;
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            Log.Debug("alice created");
        }
        else
        {
            Log.Debug("alice already exists");
        }

        var bob = userMgr.FindByNameAsync("bob").Result;
        if (bob == null)
        {
            bob = new ApplicationUser
            {
                UserName = "bob",
                Email = "BobSmith@email.com",
                EmailConfirmed = true
            };
            var result = userMgr.CreateAsync(bob, "Pass123$").Result;
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            result = userMgr.AddClaimsAsync(bob, new Claim[]
            {
                new(JwtClaimTypes.Name, "Bob Smith"),
            }).Result;
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            Log.Debug("bob created");
        }
        else
        {
            Log.Debug("bob already exists");
        }

        var tom = userMgr.FindByNameAsync("tom").Result;
        if (tom == null)
        {
            tom = new ApplicationUser
            {
                UserName = "tom",
                Email = "TomSmith@email.com",
                EmailConfirmed = true,
            };
            var result = userMgr.CreateAsync(tom, "Pass123$").Result;
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            result = userMgr.AddClaimsAsync(tom, new Claim[]
            {
                new(JwtClaimTypes.Name, "Tom Smith"),
            }).Result;
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            Log.Debug("tom created");
        }
        else
        {
            Log.Debug("tom already exists");
        }

        var andrew = userMgr.FindByNameAsync("andrew").Result;
        if (andrew == null)
        {
            andrew = new ApplicationUser
            {
                UserName = "andrew",
                Email = "andrewgunn31@gmail.com",
                EmailConfirmed = true
            };
            var result = userMgr.CreateAsync(andrew, "Pass123$").Result;
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            result = userMgr.AddClaimsAsync(andrew, new Claim[]
            {
                new(JwtClaimTypes.Name, "Andrew Gunn"),
                new(JwtClaimTypes.Email, "andrewgunn31@gmail.com"),
            }).Result;
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            Log.Debug("andrew created");
        }
        else
        {
            Log.Debug("andrew already exists");
        }
    }
}