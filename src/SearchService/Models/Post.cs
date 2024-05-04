using MongoDB.Entities;

namespace SearchService.Models;

/// <summary>
/// Represents an item entity in the database, used for mapping the Item collection in MongoDB.
/// Inheriting from <see cref="Entity"/> automatically provides the item with an Id property (of type string),
/// which serves as the primary key in the database and enables easy CRUD operations through MongoDB.Entities.
/// This class represents the Item entity in the database and is used to map the Item collection in the database.
/// </summary>
public class Post : Entity
{
    public string? Title { get; set; }
    public string? Content { get; set; }
    public string? Author { get; set; }
    public string? UserId { get; set; }
    public string? ImageUrl { get; set; }
    public string? Category { get; set; }
    public string? Status { get; set; }
    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
}