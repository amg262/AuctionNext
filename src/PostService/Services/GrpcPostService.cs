using MongoDB.Entities;
using PostService.Models;
using Grpc.Core;

namespace PostService.Services;

/// <summary>
/// Implements the gRPC service for managing posts.
/// This service allows for operations related to posts such as retrieving a specific post.
/// </summary>
public class GrpcPostService : GrpcPost.GrpcPostBase
{
    private readonly ILogger<GrpcPostService> _logger;

    /// <summary>
    /// Initializes a new instance of the GrpcPostService with dependency injection.
    /// </summary>
    /// <param name="logger">The logger used to log information and errors.</param>
    public GrpcPostService(ILogger<GrpcPostService> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Asynchronously retrieves a post by its ID.
    /// </summary>
    /// <param name="request">The request containing the ID of the post to retrieve.</param>
    /// <param name="context">Server call context that provides properties for gRPC calls, such as cancellation and metadata.</param>
    /// <returns>A task that upon completion returns the post details if found; otherwise, throws an RpcException when the post is not found.</returns>
    /// <exception cref="RpcException">Thrown when no post with the specified ID could be found.</exception>
    public async Task<GrpcPostResponse> GetPost(GetPostRequest request, ServerCallContext context)
    {
        Console.WriteLine("==> Received Grpc request for post");

        var post = await DB.Find<Post>().OneAsync(request.Id)
                   ?? throw new RpcException(new Status(StatusCode.NotFound, "Not found"));

        var response = new GrpcPostResponse
        {
            Post = new GrpcPostModel
            {
                Id = request.Id.ToString(),
                UserId = post.UserId,
                Content = post.Content,
                Title = post.Title
            }
        };

        return response;
    }
}