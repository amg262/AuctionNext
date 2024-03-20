using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PaymentService.Data;

namespace PaymentService.Controllers;

public class RewardController : ControllerBase
{
	private readonly IConfiguration _config;
	private readonly AppDbContext _db;
	private readonly IMapper _mapper;

	public RewardController(IConfiguration config, AppDbContext db, IMapper mapper)
	{
		_config = config;
		_db = db;
		_mapper = mapper;
	}
}