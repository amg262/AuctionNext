﻿using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Entities;
using PostService.Models;

namespace PostService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostController : ControllerBase
{
	
	
	[HttpGet]
	public async Task<IActionResult> Get()
	{
		var filter = Builders<Post>.Filter.Empty; // Defines an empty filter to select all documents
		var posts = await DB.Find<Post>().Match(filter).ExecuteAsync();
		return Ok(posts);
	}

	[HttpPost("create")]
	public async Task<IActionResult> Post([FromBody] Post post)
	{
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

	[HttpPut("{id:length(24)}")]
	public async Task<IActionResult> Put(string id, [FromBody] Post post)
	{
		if (post == null)
		{
			return BadRequest("Post cannot be null.");
		}

		var existingPost = await DB.Find<Post>().OneAsync(id);
		if (existingPost == null)
		{
			return NotFound($"Post with ID {id} not found.");
		}

		existingPost.Title = post.Title;
		existingPost.Content = post.Content;
		await existingPost.SaveAsync();

		return Ok(existingPost);
	}

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
}