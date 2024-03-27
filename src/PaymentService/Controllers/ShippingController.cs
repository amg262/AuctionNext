using AutoMapper;
using EasyPost.Models.API;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaymentService.Data;
using PaymentService.Entities;
using PaymentService.Services;

namespace PaymentService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShippingController : ControllerBase
{
	private readonly AppDbContext _db;
	private readonly ShippingService _shippingService;
	private readonly IMapper _mapper;

	public ShippingController(AppDbContext db, ShippingService shippingService, IMapper mapper)
	{
		_db = db;
		_shippingService = shippingService;
		_mapper = mapper;
	}

	[HttpGet]
	public async Task<IActionResult> Get()
	{
		List<Shipping?> items = await _db.Shipping.ToListAsync();

		return Ok(items);
	}

	[HttpPost]
	public async Task<IActionResult> Post(Shipping shipping)
	{
		await _db.Shipping.AddAsync(shipping);
		await _db.SaveChangesAsync();
		return Ok(shipping);
	}

	[HttpPut]
	public async Task<IActionResult> Put(Shipping shipping)
	{
		_db.Shipping.Update(shipping);
		await _db.SaveChangesAsync();
		return Ok(shipping);
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> Delete(int id)
	{
		var item = await _db.Shipping.FirstOrDefaultAsync(c => c.ShippingId == id);
		_db.Shipping.Remove(item);
		await _db.SaveChangesAsync();
		return Ok();
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> GetShippingById(int id)
	{
		var item = await _db.Shipping.FirstOrDefaultAsync(c => c.ShippingId == id);
		return Ok(item);
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> GetShippingByPaymentId(Guid paymentId)
	{
		var item = await _db.Shipping.FirstOrDefaultAsync(c => c.PaymentId == paymentId);
		return Ok(item);
	}

	[HttpPost("{paymentId}")]
	public async Task<IActionResult> CompleteShipping(Guid paymentId, Address toAddress)
	{
		var payment = await _db.Payments.FirstOrDefaultAsync(c => c.Id == paymentId);
		if (payment == null) return NotFound();
		var shipment = await _shippingService.CompleteShipping(payment, toAddress);
		return Ok(shipment);
	}

	public async Task<IActionResult> VerifyAddress(Address toVerify)
	{
		var verifiedAddress = await _shippingService.VerifyAddress(toVerify);
		return Ok(verifiedAddress);
	}
}