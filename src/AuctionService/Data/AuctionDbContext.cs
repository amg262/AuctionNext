using AuctionService.Entities;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Data;

/// <summary>
/// Represents the database context for the Auction service, providing access to the auctions stored in the database.
/// It includes configuration for entities and their relationships as well as integration with MassTransit for event-driven architecture components.
/// </summary>
public class AuctionDbContext : DbContext
{
	/// <summary>
	/// Initializes a new instance of the <see cref="AuctionDbContext"/> with the specified options.
	/// The options include configurations such as the database provider (e.g., SQL Server, SQLite) and connection details.
	/// </summary>
	/// <param name="options">The options to be used by the DbContext.</param>
	public AuctionDbContext(DbContextOptions options) : base(options)
	{
	}

	public DbSet<Auction> Auctions { get; set; }

	/// <summary>
	/// Configures the model for the context being created. This method includes configuration for the Auction entities
	/// and setups for MassTransit's inbox and outbox patterns to support reliable messaging and event-driven architectures.
	/// </summary>
	/// <param name="modelBuilder">The builder being used to construct the model for this context.</param>
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		// Configures the entity model for the inbox feature of MassTransit, used to ensure reliable message consumption.
		modelBuilder.AddInboxStateEntity();

		// Configures the entity model for the outbox message feature of MassTransit, enabling reliable messaging by storing messages before they are published.
		modelBuilder.AddOutboxMessageEntity();

		// Configures the entity model for the outbox state feature of MassTransit, used to track the state of messages in the outbox.
		modelBuilder.AddOutboxStateEntity();
	}
}