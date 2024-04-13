using MongoDB.Entities;

namespace PostService.Models;

/// <summary>
/// Represents an item entity in the database, used for mapping the Item collection in MongoDB.
/// Inheriting from <see cref="Entity"/> automatically provides the item with an Id property (of type string),
/// which serves as the primary key in the database and enables easy CRUD operations through MongoDB.Entities.
/// This class represents the Item entity in the database and is used to map the Item collection in the database.
/// </summary>
public class Comment : Entity
{
    public string? PostId { get; set; }
    public string? Content { get; set; }
    public string? Author { get; set; }
    public string? UserId { get; set; }
    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
}