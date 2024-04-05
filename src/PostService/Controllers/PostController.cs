using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;
using PostService.Models;

namespace PostService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostController : ControllerBase
{
	[HttpPost("create")]
	public async Task<IActionResult> Post([FromBody] Post post)
	{
		if (post == null)
		{
			return BadRequest("Post cannot be null.");
		}

		await post.SaveAsync(); // MongoDB.Entities simplifies the save operation

		return CreatedAtAction(nameof(Post), new {id = post.ID}, post);
	}

	[HttpGet("{id:length(24)}")]
	public async Task<IActionResult> Get(string id)
	{
		var post = await DB.Find<Post>().OneAsync(id);
		if (post == null)
		{
			return NotFound($"Post with ID {id} not found.");
		}

		return Ok(post);
	}
}