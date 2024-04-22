namespace PostService.DTOs;

public record CommentDto
{
    public string? PostTitle { get; set; }
    public string? PostId { get; set; }
    public string? Content { get; set; }
    public string? Author { get; set; }
    public string? UserId { get; set; }
    public DateTime? CreatedAt { get; set; }
}