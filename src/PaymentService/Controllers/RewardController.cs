using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaymentService.Data;
using PaymentService.DTOs;
using PaymentService.Entities;

namespace PaymentService.Controllers;

/// <summary>
/// Manages reward-related operations, including retrieving, adding, updating, and deleting rewards.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class RewardController : ControllerBase
{
	private readonly IConfiguration _config;
	private readonly AppDbContext _db;
	private readonly IMapper _mapper;

	/// <summary>
	/// Initializes a new instance of the <see cref="RewardController"/> class.
	/// </summary>
	/// <param name="config">The application configuration settings.</param>
	/// <param name="db">The database context used for accessing reward data.</param>
	/// <param name="mapper">The AutoMapper instance used for mapping between DTOs and entity models.</param>
	public RewardController(IConfiguration config, AppDbContext db, IMapper mapper)
	{
		_config = config;
		_db = db;
		_mapper = mapper;
	}

	/// <summary>
	/// Retrieves all rewards.
	/// </summary>
	/// <returns>An action result containing a list of all rewards.</returns>
	[HttpGet]
	public async Task<IActionResult> Get()
	{
		List<Reward?> rewards = await _db.Rewards.ToListAsync();
		return Ok(rewards);
	}

	/// <summary>
	/// Adds a new reward based on the provided reward data transfer object (DTO).
	/// </summary>
	/// <param name="rewardDto">The reward DTO containing data for the new reward.</param>
	/// <returns>An action result containing the added reward.</returns>
	[HttpPost("reward")]
	public async Task<IActionResult> Post([FromBody] RewardDto? rewardDto)
	{
		if (rewardDto == null) return BadRequest("Reward data is required.");

		var reward = _mapper.Map<Reward>(rewardDto);
		_db.Rewards.Add(reward);
		await _db.SaveChangesAsync();
		return Ok(reward);
	}

	/// <summary>
	/// Updates an existing reward identified by its ID with new data provided in the reward DTO.
	/// </summary>
	/// <param name="id">The ID of the reward to update.</param>
	/// <param name="rewardDto">The reward DTO containing the new data for the reward.</param>
	/// <returns>An action result containing the updated reward.</returns>
	[HttpPut("reward/{id}")]
	public async Task<IActionResult> Put(int id, [FromBody] RewardDto rewardDto)
	{
		var reward = await _db.Rewards.FindAsync(id);
		if (reward == null)
		{
			return NotFound();
		}

		var newReward = _mapper.Map<Reward>(rewardDto);
		_mapper.Map(newReward, reward);

		_db.Rewards.Update(newReward);
		await _db.SaveChangesAsync();

		return Ok(reward);
	}

	/// <summary>
	/// Deletes a reward identified by its ID.
	/// </summary>
	/// <param name="id">The ID of the reward to delete.</param>
	/// <returns>An action result confirming the deletion of the reward.</returns>
	[HttpDelete("reward/{id}")]
	public async Task<IActionResult> Delete(int id)
	{
		var reward = await _db.Rewards.FindAsync(id);
		if (reward == null)
		{
			return NotFound();
		}

		_db.Rewards.Remove(reward);
		await _db.SaveChangesAsync();

		return Ok(reward);
	}
}