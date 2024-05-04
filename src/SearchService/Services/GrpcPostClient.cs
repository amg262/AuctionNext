using System.Reflection.Metadata;
using Grpc.Net.Client;
using SearchService.Models;
using PostService;

namespace SearchService.Services;

/// <summary>
/// Provides functionality to interact with the GRPC PostService to fetch posts.
/// </summary>
public class GrpcPostClient
{
    private readonly ILogger<GrpcPostClient> _logger;
    private readonly IConfiguration _config;

    /// <summary>
    /// Initializes a new instance of the GrpcPostClient class.
    /// </summary>
    /// <param name="logger">The logger for logging messages within the client.</param>
    /// <param name="config">The configuration handler to access application settings.</param>
    public GrpcPostClient(ILogger<GrpcPostClient> logger, IConfiguration config)
    {
        _logger = logger;
        _config = config;
    }

    /// <summary>
    /// Retrieves a post by its ID using a GRPC service call.
    /// </summary>
    /// <param name="id">The unique identifier of the post to retrieve.</param>
    /// <returns>A <see cref="Post"/> object containing the retrieved post details if successful, null otherwise.</returns>
    /// <remarks>
    /// This method logs the initiation of the GRPC service call and handles any exceptions by logging them and returning null.
    /// This method does not handle any retry logic or extended error handling beyond logging and null returning. 
    /// Consumers should handle null results appropriately.
    /// </remarks>
    public Post GetPost(string id)
    {
        _logger.LogInformation("Calling GRPC Service");
        var channel = GrpcChannel.ForAddress(_config["GrpcPost"]);
        var client = new GrpcPost.GrpcPostClient(channel);
        var request = new GetPostRequest() { Id = id };

        try
        {
            var reply = client.GetPost(request);
            var post = new Post
            {
                ID = reply.Post.Id,
                Title = reply.Post.Title,
                Category = reply.Post.Content,
                UserId = reply.Post.UserId,
            };

            return post;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Could not call GRPC Server");
            return null;
        }
    }
}