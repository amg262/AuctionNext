using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PaymentService.Data;

namespace PaymentService.Controllers;

public class CouponController : ControllerBase
{
	private readonly IConfiguration _config;
	private readonly AppDbContext _db;
	private readonly IMapper _mapper;

	public CouponController(IConfiguration config, AppDbContext db, IMapper mapper)
	{
		_config = config;
		_db = db;
		_mapper = mapper;
	}
}