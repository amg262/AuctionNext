// using Grpc.Net.Client;
// using SearchService.Models;
//
// namespace SearchService.Services;
//
// public class Grpc
// {
//     private readonly ILogger<Grpc> _logger;
//     private readonly IConfiguration _config;
//
//     public Grpc(IConfiguration config, ILogger<Grpc> logger)
//     {
//         _config = config;
//         _logger = logger;
//     }
//
//     public Post GetPost(string id)
//     {
//         var channel = GrpcChannel.ForAddress(_config["GrpcPost"]);
//         var client = new GrpcPost.GrpcPostClient(channel);
//     }
// }