using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Entities;
using PostService.DTOs;
using PostService.Models;

namespace PostService.Controllers;

/// <summary>
/// Handles HTTP requests related to Post entities.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class PostController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ILogger<PostController> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="PostController"/> class.
    /// </summary>
    /// <param name="mapper">AutoMapper interface to map between DTO and domain models.</param>
    /// <param name="logger">ILogger interface for logging</param>
    public PostController(IMapper mapper, ILogger<PostController> logger)
    {
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Retrieves all posts.
    /// </summary>
    /// <returns>A list of posts.</returns>
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var filter = Builders<Post>.Filter.Empty; // Defines an empty filter to select all documents
        var posts = await DB.Find<Post>().Match(filter).ExecuteAsync();
        return Ok(posts);
    }

    /// <summary>
    /// Creates a new post.
    /// Note: Currently uses DB.InsertAsync for demonstration. Swap to post.SaveAsync() for upsert functionality.
    /// </summary>
    /// <param name="postDto">The post Dto object passed in</param>
    /// <returns>The created post.</returns>
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] PostDto postDto)
    {
        if (postDto == null)
        {
            return BadRequest("Post cannot be null.");
        }

        var post = _mapper.Map<Post>(postDto);

        if (post == null)
        {
            return BadRequest("Post cannot be null.");
        }

        // Best For: Situations where you are certain the document is new and does not exist in the database.
        // It ensures that duplicates are not created inadvertently.
        await DB.InsertAsync(post);

        // Best For: Scenarios where you might be working with a document that could either be new or already exist,
        // and you want to upsert (update if exists, insert if not).
        // await post.SaveAsync();

        return CreatedAtAction(nameof(Post), new { id = post.ID }, post);
    }

    /// <summary>
    /// Retrieves a single post by ID.
    /// </summary>
    /// <param name="id">The ID of the post to retrieve.</param>
    /// <returns>The requested post if found; otherwise, NotFound.</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        var post = await DB.Find<Post>().OneAsync(id);
        if (post == null)
        {
            return NotFound($"Post with ID {id} not found.");
        }

        return Ok(post);
    }

    /// <summary>
    /// Updates a post with a specified ID.
    /// </summary>
    /// <param name="id">The ID of the post to update.</param>
    /// <param name="postDto">The updated post DTO object.</param>
    /// <returns>The updated post if the operation is successful; otherwise, NotFound.</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(string id, [FromBody] PostDto postDto)
    {
        if (postDto == null)
        {
            return BadRequest("Post cannot be null.");
        }


        var existingPost = await DB.Find<Post>().OneAsync(id);
        if (existingPost == null)
        {
            return NotFound($"Post with ID {id} not found.");
        }
        //
        // existingPost.Title = postDto.Title;
        // existingPost.Content = postDto.Content;

        // Instead of manually mapping properties, use AutoMapper to map the DTO to the existing post
        _mapper.Map(postDto, existingPost);

        // var post = _mapper.Map<Post>(postDto);

        await existingPost.SaveAsync();

        return Ok(existingPost);
    }

    /// <summary>
    /// Deletes a post by ID.
    /// </summary>
    /// <param name="id">The ID of the post to delete.</param>
    /// <returns>A NoContent result if the post is successfully deleted; otherwise, NotFound.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var post = await DB.Find<Post>().OneAsync(id);
        if (post == null)
        {
            return NotFound($"Post with ID {id} not found.");
        }

        await DB.DeleteAsync<Post>(id); // Correct usage to delete a document by ID
        // await post.DeleteAsync(); // Incorrect usage to delete a document by instance
        return NoContent();
    }


    /// <summary>
    /// Posts a new comment to a specific blog post.
    /// </summary>
    /// <param name="postId">The ID of the post to which the comment is being added.</param>
    /// <param name="commentDto">The comment DTO object containing the user's input.</param>
    /// <returns>An IActionResult that indicates whether the comment was successfully posted or if an error occurred.</returns>
    [HttpPost("{postId}/comment")]
    public async Task<IActionResult> Comment(string postId, [FromBody] CommentDto commentDto)
    {
        if (commentDto == null)
        {
            return BadRequest("Comment cannot be null.");
        }

        var post = await DB.Find<Post>().OneAsync(postId);

        if (post == null)
        {
            return NotFound($"Post with ID {postId} not found.");
        }

        commentDto.PostId = postId;
        commentDto.Author = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        var newComment = _mapper.Map<Comment>(commentDto);

        // var newCommentDto = new CommentDto
        // {
        //     Content = commentDto.Content,
        //     PostId = post.ID,
        //     UserId = commentDto.UserId,
        //     Author = User.Identity.Name
        // };

        await DB.InsertAsync(newComment);

        // await post.SaveAsync();

        return Ok(post);
    }

    /// <summary>
    /// Retrieves a specific comment by its ID associated with a given post.
    /// </summary>
    /// <param name="postId">The ID of the post to which the comment belongs.</param>
    /// <param name="commentId">The ID of the comment to retrieve.</param>
    /// <returns>An IActionResult containing the comment if found, otherwise NotFound.</returns>
    [HttpGet("{postId}/comment/{commentId}")]
    public async Task<IActionResult> GetComment(string postId, string commentId)
    {
        var post = await DB.Find<Post>().OneAsync(postId);
        if (post == null)
        {
            return NotFound($"Post with ID {postId} not found.");
        }

        var comment = await DB.Find<Comment>().OneAsync(commentId);
        if (comment == null)
        {
            return NotFound($"Comment with ID {commentId} not found.");
        }

        return Ok(comment);
    }

    /// <summary>
    /// Updates a specific comment by ID associated with a given post.
    /// </summary>
    /// <param name="postId">The ID of the post to which the comment belongs.</param>
    /// <param name="commentId">The ID of the comment to update.</param>
    /// <param name="commentDto">The updated comment data transferred via DTO.</param>
    /// <returns>An IActionResult indicating success or failure of the update.</returns>
    [HttpPut("{postId}/comment/{commentId}")]
    public async Task<IActionResult> PutComment(string postId, string commentId, [FromBody] CommentDto commentDto)
    {
        if (commentDto == null)
        {
            return BadRequest("Comment cannot be null.");
        }

        var post = await DB.Find<Post>().OneAsync(postId);
        if (post == null)
        {
            return NotFound($"Post with ID {postId} not found.");
        }

        var existingComment = await DB.Find<Comment>().OneAsync(commentId);
        if (existingComment == null)
        {
            return NotFound($"Comment with ID {commentId} not found.");
        }

        // Check if the current user is authorized to update the comment
        if (existingComment.UserId != User.FindFirst(ClaimTypes.NameIdentifier)?.Value)
        {
            return Unauthorized("You do not have permission to update this comment.");
        }


        // existingComment.Content = commentDto.Content;

        _mapper.Map(commentDto, existingComment);
        // var updatedComment = _mapper.Map<Comment>(commentDto);

        await existingComment.SaveAsync();

        return Ok(existingComment);
    }

    /// <summary>
    /// Deletes a specific comment by its ID from a given post.
    /// </summary>
    /// <param name="postId">The ID of the post from which the comment will be deleted.</param>
    /// <param name="commentId">The ID of the comment to delete.</param>
    /// <returns>An IActionResult indicating if the comment was successfully deleted or not.</returns>
    [HttpDelete("{postId}/comment/{commentId}")]
    public async Task<IActionResult> DeleteComment(string postId, string commentId)
    {
        var post = await DB.Find<Post>().OneAsync(postId);
        if (post == null)
        {
            return NotFound($"Post with ID {postId} not found.");
        }

        var comment = await DB.Find<Comment>().OneAsync(commentId);
        if (comment == null)
        {
            return NotFound($"Comment with ID {commentId} not found.");
        }

        await DB.DeleteAsync<Comment>(commentId);

        return NoContent();
    }

    /// <summary>
    /// Retrieves all comments associated with a given post.
    /// </summary>
    /// <param name="postId">The ID of the post for which comments are to be retrieved.</param>
    /// <returns>An IActionResult containing all comments for the specified post.</returns>
    [HttpGet("{postId}/comment")]
    public async Task<IActionResult> GetComments(string postId)
    {
        var post = await DB.Find<Post>().OneAsync(postId);
        if (post == null)
        {
            return NotFound($"Post with ID {postId} not found.");
        }

        var comments = await DB.Find<Comment>().ManyAsync(c => c.PostId == postId);
        return Ok(comments);
    }
}