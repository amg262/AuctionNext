using MongoDB.Driver;
using MongoDB.Entities;
using PostService.Models;

namespace PostService.Data;

/// <summary>
/// Provides functionality to initialize and seed the MongoDB database for the SearchService.
/// </summary>
public static class DbInitializer
{
    private static readonly Random Random = new();

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
                // new()
                // {
                //     Title = "Introduction to Machine Learning with .NET",
                //     Description = "Getting started with machine learning in .NET using ML.NET.",
                //     Content =
                //         "ML.NET is a powerful, open-source machine learning framework for .NET developers. This article " +
                //         "introduces the basics of machine learning, how to get started with ML.NET, and walks through a " +
                //         "simple project to predict user behavior." +
                //         "<br/>" +
                //         "ML.NET is a powerful, open-source machine learning framework for .NET developers. This article " +
                //         "introduces the basics of machine learning, how to get started with ML.NET, and walks through a " +
                //         "simple project to predict user behavior.",
                //     Author = "Chris Johnson",
                //     UserId = "alice",
                //     ImageUrl = "https://source.unsplash.com/featured/?machinelearning", // Example Unsplash URL
                //     Category = "Machine Learning",
                //     Status = "Published",
                //     CreatedAt = DateTime.UtcNow.AddDays(-55),
                //     UpdatedAt = DateTime.UtcNow.AddDays(-18)
                // }
                // Add more example posts here
            };

            foreach (var post in seedPosts)
            {
                await post.SaveAsync();

                var comment = new Comment
                {
                    PostId = post.ID,
                    Content = RandomComment(),
                    Author = post.Author,
                    // UserId = user,
                    UserId = post.UserId switch
                    {
                        "bob" => "alice",
                        "alice" => "bob",
                        "tom" => "alice",
                        _ => string.Empty
                    },
                };

                await DB.InsertAsync(comment);
            }
        }

        // await DB.Index<Item>()
        // 	.Key(x => x.Make, KeyType.Text)
        // 	.Key(x => x.Model, KeyType.Text)
        // 	.Key(x => x.Color, KeyType.Text)
        // 	.CreateAsync();
        //
        // var count = await DB.CountAsync<Item>();
        //
        // using var scope = app.Services.CreateScope();
        //
        // var httpClient = scope.ServiceProvider.GetRequiredService<AuctionSvcHttpClient>();
        //
        // var items = await httpClient.GetItemsForSearchDb();
        //
        // Console.WriteLine(items.Count + " returned from the auction service");
        //
        // if (items.Count > 0) await DB.SaveAsync(items);
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
            "I'm not sure I understand this completely, could you help?"
        };
        var index = Random.Next(predefinedComments.Count);
        return predefinedComments[index];
    }
}