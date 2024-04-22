namespace PostService.DTOs;

public record CommentCreatedDto
{
    public string? PostId { get; init; }
    public string? Content { get; init; }
    public string? Author { get; init; }
    public string? UserId { get; init; }
    public DateTime? CreatedAt { get; init; }
}