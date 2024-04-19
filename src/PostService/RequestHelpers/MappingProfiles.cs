using AutoMapper;
using Contracts;
using PostService.DTOs;
using PostService.Models;

namespace PostService.RequestHelpers;

/// <summary>
/// Defines AutoMapper mapping profiles for the SearchService.
/// This class is responsible for configuring the mappings between DTOs (Data Transfer Objects) and entity models.
/// </summary>
public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Post, PostDto>().ReverseMap();
        CreateMap<Comment, CommentDto>().ReverseMap();
        CreateMap<Post, PostCreated>().ReverseMap();
    }
}