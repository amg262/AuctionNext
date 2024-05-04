using MongoDB.Entities;
using PostService.Models;

namespace PostService.Services;

using Grpc.Core;

public class GrpcPostService
{
    public GrpcPostService()
    {
    }

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