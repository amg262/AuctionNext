﻿using MongoDB.Driver;
using MongoDB.Entities;
using PostService.Models;

namespace PostService.Data;

/// <summary>
/// Provides functionality to initialize and seed the MongoDB database for the SearchService.
/// Handles database connection initialization, index creation, and initial data seeding with posts and comments.
/// </summary>
public static class DbInitializer
{
    private static readonly Random Random = new();

    /// <summary>
    /// Initializes the MongoDB database with necessary indexes and seeds it with initial data if empty.
    /// </summary>
    /// <param name="app">The application instance used to access configuration settings for database initialization.</param>
    /// <remarks>
    /// This method handles the initialization of the database connection and index creation for optimized search.
    /// It checks if the database is empty and seeds it with sample posts and associated comments.
    /// </remarks>
    public static async Task InitDb(WebApplication app)
    {
        await DB.InitAsync("PostDb", MongoClientSettings
            .FromConnectionString(app.Configuration.GetConnectionString("MongoDbConnection")));

        await DB.Index<Post>()
            .Key(x => x.ID, KeyType.Ascending)
            .Key(x => x.Title, KeyType.Text)
            .Key(x => x.Content, KeyType.Text)
            .Key(x => x.UserId, KeyType.Text)
            .Key(x => x.Category, KeyType.Text)
            .CreateAsync();


        await DB.Index<Comment>()
            .Key(c => c.PostId, KeyType.Ascending)
            .CreateAsync();

        // Check if the 'Post' collection is empty
        var count = await DB.CountAsync<Post>();

        // If the 'Post' collection is empty, seed the database with example posts
        if (count == 0)
        {
            var seedPosts = GetSeedPosts();

            foreach (var post in seedPosts)
            {
                await post.SaveAsync();
                var numberOfComments = Random.Next(1, 5);

                for (var i = 0; i < numberOfComments; i++)
                {
                    var comment = new Comment
                    {
                        PostId = post.ID,
                        Content = RandomComment(),
                        Author = post.Author,
                        UserId = RandomUser(),
                        CreatedAt = DateTime.UtcNow.AddDays(-Random.Next(0, 10)) // Randomize comment creation date
                    };

                    await DB.InsertAsync(comment);
                }
            }
        }
        else
        {
            Console.WriteLine("Database already seeded with posts.");
        }
    }

    /// <summary>
    /// Generates a random user ID from a predefined list.
    /// </summary>
    /// <returns>A randomly selected user ID from the predefined list.</returns>
    private static string RandomUser()
    {
        // List of predefined user IDs for seeding comments
        var predefinedUsers = new List<string> { "alice", "bob", "tom", "andrew" };
        var index = Random.Next(predefinedUsers.Count);
        return predefinedUsers[index];
    }

    /// <summary>
    /// Generates a random comment from a predefined list of comments.
    /// </summary>
    /// <returns>A random comment string selected from a list of predefined comments.</returns>
    private static string RandomComment()
    {
        var predefinedComments = new List<string>
        {
            "Great insight, thanks for sharing!",
            "Very informative post, learned a lot.",
            "I disagree with your point, but interesting read!",
            "This is exactly what I was looking for, perfect!",
            "Could you elaborate on this further?",
            "Fascinating approach, could you provide more details?",
            "I never thought about it this way, thanks for opening my eyes!",
            "This seems incorrect, can you verify your sources?",
            "Amazing explanation, very clear and concise.",
            "I'm not sure I understand this completely, could you help?",
            "Spot on! This aligns perfectly with my experience.",
            "This is a new perspective for me, quite intriguing!",
            "Your example helped clarify the concept, thank you.",
            "Is there a reference or source that supports this argument?",
            "I appreciate the simplicity of your explanation.",
            "Wow, this really challenges the conventional wisdom.",
            "Can this method be applied in other contexts as well?",
            "Your insight here is very valuable, keep posting!",
            "This contradicts what I've read elsewhere; interesting take.",
            "Could you break down the last point a bit more?"
        };
        var index = Random.Next(predefinedComments.Count);
        return predefinedComments[index];
    }

    /// <summary>
    /// Retrieves a predefined list of seed posts to be used for initial database seeding.
    /// </summary>
    /// <returns>An enumerable of <see cref="Post"/> objects to be used for seeding.</returns>
    private static IEnumerable<Post> GetSeedPosts()
    {
        IList<Post> seedPosts = new List<Post>
        {
            new()
            {
                Title = "The Rise of Serverless Architecture",
                Content =
                    "Serverless architecture allows developers to build and run applications and services without " +
                    "having to manage infrastructure. This article delves into the benefits, challenges, and future " +
                    "of serverless computing. " +
                    "<br/>" +
                    "Serverless architecture allows developers to build and run applications " +
                    "and services without having to manage infrastructure. This article delves into the benefits, " +
                    "challenges, and future of serverless computing.",
                Author = "Jane Doe",
                UserId = "bob",
                ImageUrl = "https://source.unsplash.com/featured/?serverless", // Example Unsplash URL
                Category = "Technology",
                Status = "Published",
                CreatedAt = DateTime.UtcNow.AddDays(-10),
                UpdatedAt = DateTime.UtcNow.AddDays(-2)
            },
            new()
            {
                Title = "Understanding Async/Await in C#",
                Content =
                    "Asynchronous programming is essential for developing scalable and responsive applications. " +
                    "This article covers the basics of async/await in C#, including best practices and common pitfalls." +
                    "<br/>" +
                    "Asynchronous programming is essential for developing scalable and responsive applications. " +
                    "This article covers the basics of async/await in C#, including best practices and common pitfalls.",
                Author = "John Smith",
                UserId = "bob",
                ImageUrl = "https://source.unsplash.com/featured/?coding", // Example Unsplash URL
                Category = "Programming",
                Status = "Published",
                CreatedAt = DateTime.UtcNow.AddDays(-20),
                UpdatedAt = DateTime.UtcNow.AddDays(-5)
            },
            new()
            {
                Title = "A Beginner's Guide to Microservices",
                Content =
                    "Microservices architecture is an approach in which a single application is composed of many " +
                    "loosely coupled and independently deployable smaller services. This guide covers the basics, " +
                    "advantages, and how to get started." +
                    "<br/>" +
                    "Microservices architecture is an approach in which a single application is composed of many " +
                    "loosely coupled and independently deployable smaller services. This guide covers the basics, " +
                    "advantages, and how to get started.",
                Author = "Alice Johnson",
                UserId = "bob",
                ImageUrl = "https://source.unsplash.com/featured/?microservices", // Example Unsplash URL
                Category = "Architecture",
                Status = "Published",
                CreatedAt = DateTime.UtcNow.AddDays(-30),
                UpdatedAt = DateTime.UtcNow.AddDays(-10)
            },
            new()
            {
                Title = "Exploring Blazor for Web Development",
                Content =
                    "Blazor is a free and open-source web framework that enables developers to create web apps using " +
                    "C# and HTML. This article explores how Blazor works, its various modes (Server and WebAssembly), " +
                    "and its advantages over traditional JavaScript frameworks." +
                    "<br/>" +
                    "Blazor is a free and open-source web framework that enables developers to create web apps using " +
                    "C# and HTML. This article explores how Blazor works, its various modes (Server and WebAssembly), " +
                    "and its advantages over traditional JavaScript frameworks.",
                Author = "Emily White",
                UserId = "tom",
                ImageUrl =
                    "https://source.unsplash.com/featured/?blazor", // Placeholder, specific Blazor images may be hard to find
                Category = "Web Development",
                Status = "Published",
                CreatedAt = DateTime.UtcNow.AddDays(-5),
                UpdatedAt = DateTime.UtcNow.AddDays(-1)
            },
            new()
            {
                Title = "Modern Security Practices for .NET Applications",
                Content =
                    "With cyber threats on the rise, securing your .NET applications has never been more important. " +
                    "This article covers modern security practices, including secure coding principles, data " +
                    "protection, and using ASP.NET Core's built-in security features. " +
                    "<br/>" +
                    "With cyber threats on the rise, securing your .NET applications has never been more important. " +
                    "This article covers modern security practices, including secure coding principles, data " +
                    "protection, and using ASP.NET Core's built-in security features. ",
                Author = "David Brown",
                UserId = "tom",
                ImageUrl = "https://source.unsplash.com/featured/?cybersecurity", // Example Unsplash URL
                Category = "Security",
                Status = "Published",
                CreatedAt = DateTime.UtcNow.AddDays(-15),
                UpdatedAt = DateTime.UtcNow.AddDays(-3)
            },
            new()
            {
                Title = "Getting Started with Docker for .NET Developers",
                Content =
                    "Docker simplifies the process of deploying .NET applications across different environments." +
                    " This guide explains Docker concepts, how to Dockerize a .NET application, and best practices " +
                    "for working with Docker containers." +
                    "<br/>" +
                    "Docker simplifies the process of deploying .NET applications across different environments." +
                    " This guide explains Docker concepts, how to Dockerize a .NET application, and best practices " +
                    "for working with Docker containers.",
                Author = "Michael Lee",
                UserId = "tom",
                ImageUrl = "https://source.unsplash.com/featured/?docker", // Example Unsplash URL
                Category = "DevOps",
                Status = "Published",
                CreatedAt = DateTime.UtcNow.AddDays(-25),
                UpdatedAt = DateTime.UtcNow.AddDays(-7)
            },
            new()
            {
                Title = "The Evolution of C#",
                Content =
                    "Since its introduction in 2000, C# has become one of the most popular and versatile " +
                    "programming languages. This article explores the history of C#, its major milestones, " +
                    "and how it has evolved over the years to meet modern development needs." +
                    "<br/>" +
                    "Since its introduction in 2000, C# has become one of the most popular and versatile " +
                    "programming languages. This article explores the history of C#, its major milestones, " +
                    "and how it has evolved over the years to meet modern development needs.",
                Author = "Sarah Green",
                UserId = "alice",
                ImageUrl = "https://source.unsplash.com/featured/?csharp", // Example Unsplash URL
                Category = "Programming Languages",
                Status = "Published",
                CreatedAt = DateTime.UtcNow.AddDays(-35),
                UpdatedAt = DateTime.UtcNow.AddDays(-12)
            },
            new()
            {
                Title = "Building RESTful APIs with ASP.NET Core",
                Content =
                    "RESTful APIs are the backbone of modern web services and applications. This article provides " +
                    "a step-by-step guide on how to design, develop, and deploy RESTful APIs using ASP.NET Core, " +
                    "highlighting best practices and common pitfalls. RESTful APIs are the backbone of modern web " +
                    "services and applications. This article provides a step-by-step guide on how to design, develop, " +
                    "and deploy RESTful APIs using ASP.NET Core, highlighting best practices and common pitfalls.",
                Author = "Alex Turner",
                UserId = "alice",
                ImageUrl = "https://source.unsplash.com/featured/?api", // Example Unsplash URL
                Category = "Web Development",
                Status = "Published",
                CreatedAt = DateTime.UtcNow.AddDays(-45),
                UpdatedAt = DateTime.UtcNow.AddDays(-14)
            },
        };
        return seedPosts;
    }
}