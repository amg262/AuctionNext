﻿using MongoDB.Driver;
using MongoDB.Entities;
using SearchService.Models;
using SearchService.Services;

namespace SearchService.Data;

/// <summary>
/// Provides functionality to initialize and seed the MongoDB database for the SearchService.
/// </summary>
public static class DbInitializer
{
	/// <summary>
	/// Initializes the MongoDB database with necessary indexes and seeds it with initial data if empty.
	/// </summary>
	/// <param name="app">The application instance used to access configuration settings for database initialization.</param>
	/// <remarks>
	/// This method performs the following operations:
	/// - Initializes the MongoDB connection using settings from the application configuration.
	/// - Creates text indexes on the 'Item' collection to enhance search capabilities on 'Make', 'Model', and 'Color' fields.
	/// - Checks if the 'Item' collection is empty, and if so, seeds the database with initial data from a JSON file.
	/// 
	/// This initialization process is essential for setting up the database to support the search functionality
	/// provided by the SearchService. It ensures that the database is ready to use immediately after the application starts.
	/// </remarks>
	public static async Task InitDb(WebApplication app)
	{
		await DB.InitAsync("SearchDb", MongoClientSettings
			.FromConnectionString(app.Configuration.GetConnectionString("MongoDbConnection")));

		await DB.Index<Item>()
			.Key(x => x.Make, KeyType.Text)
			.Key(x => x.Model, KeyType.Text)
			.Key(x => x.Color, KeyType.Text)
			.CreateAsync();

		var count = await DB.CountAsync<Item>();

		using var scope = app.Services.CreateScope();

		var httpClient = scope.ServiceProvider.GetRequiredService<AuctionSvcHttpClient>();

		var items = await httpClient.GetItemsForSearchDb();

		Console.WriteLine(items.Count + " returned from the auction service");

		if (items.Count > 0) await DB.SaveAsync(items);
	}
}