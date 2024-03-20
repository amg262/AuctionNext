using MassTransit;
using Microsoft.EntityFrameworkCore;
using PaymentService.Entities;

namespace PaymentService.Data;

/// <summary>
/// Application database context that extends <see cref="DbContext"/>.
/// It is responsible for configuring the model and relationships between the tables, 
/// and providing a DbSet for each entity within the application.
/// </summary>
public class AppDbContext : DbContext
{
	/// <summary>
	/// Initializes a new instance of <see cref="AppDbContext"/> with the specified options.
	/// The options configure things like the database provider to use and the connection string.
	/// </summary>
	/// <param name="options">The options for this context.</param>
	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
	{
	}

	/// <summary>
	/// Gets or sets a DbSet for the Payment entity.
	/// This enables CRUD operations on the payments table mapped by the Payment entity.
	/// </summary>
	public DbSet<Payment> Payments { get; set; }

	public DbSet<Coupon> Coupons { get; set; }

	public DbSet<Reward> Rewards { get; set; }

	/// <summary>
	/// Configures the model that was discovered by convention from the entity types
	/// exposed in DbSet properties on your derived context. The resulting model may be cached
	/// and re-used for subsequent instances of your derived context.
	/// </summary>
	/// <param name="modelBuilder">The builder being used to construct the model for this context.</param>
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		// This method would include any model configuration not discovered by conventions.
		// For example, you could use this method to configure the primary key, foreign key, and index properties of each entity type.
		base.OnModelCreating(modelBuilder);

		// Configures the entity model for the inbox feature of MassTransit, used to ensure reliable message consumption.
		modelBuilder.AddInboxStateEntity();

		// Configures the entity model for the outbox message feature of MassTransit, enabling reliable messaging by storing messages before they are published.
		modelBuilder.AddOutboxMessageEntity();

		// Configures the entity model for the outbox state feature of MassTransit, used to track the state of messages in the outbox.
		modelBuilder.AddOutboxStateEntity();
	}
}