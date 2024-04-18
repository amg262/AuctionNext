namespace Contracts;

public class PostCreated
{
    public string? Id { get; init; }
    public string? Title { get; init; }
    public string? Content { get; init; }
    public string? Author { get; init; }
    public string? ImageUrl { get; init; }
    public string? Category { get; init; }
    public string? UserId { get; init; }
    public DateTime? CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
}