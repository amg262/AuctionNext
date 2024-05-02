using MongoDB.Entities;

namespace PostService.Services;

using Grpc.Core;
using PostService;

public class GrpcPostService
{
    public GrpcPostService()
    {
    }

    public async Task<GrpcPostResponse> GetPost(GetPostRequest request, ServerCallContext context)
    {
        Console.WriteLine("==> Received Grpc request for post");

        var post = await DB.Find<Post>().OneAsync(Guid.Parse((ReadOnlySpan<char>)request.Id))
                   ?? throw new RpcException(new Status(StatusCode.NotFound, "Not found"));

        var response = new GrpcPostResponse
        {
            Post = new GrpcPostModel
            {
                Id = post.Id.ToString(),
                UserId = post.UserId,
                Content = post.Content,
                Title = post.Title
            }
        };

        return response;
    }
}