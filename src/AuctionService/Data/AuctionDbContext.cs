using AuctionService.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Data;

public class AuctionDbContext : DbContext
{
	protected AuctionDbContext()
	{
	}

	public AuctionDbContext(DbContextOptions options) : base(options)
	{
	}

	public DbSet<Auction> Auctions { get; set; }
}