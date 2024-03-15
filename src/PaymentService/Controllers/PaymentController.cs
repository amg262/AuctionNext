using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaymentService.Data;
using PaymentService.DTOs;
using PaymentService.Entities;
using Stripe;
using Stripe.Checkout;

namespace PaymentService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PaymentController : ControllerBase
{
	private readonly IConfiguration _config;
	private readonly AppDbContext _db;
	private readonly IMapper _mapper;

	public PaymentController(IConfiguration config, AppDbContext db, IMapper mapper)
	{
		_config = config;
		_db = db;
		_mapper = mapper;
		// StripeConfiguration.ApiKey = _config["Stripe:SecretKey"];
	}

	[HttpGet]
	public async Task<IActionResult> Get()
	{
		return Ok("Payment Service");
	}

	[HttpPost("create")]
	public async Task<IActionResult> Create([FromBody] StripeRequestDto request)
	{
		var options = new SessionCreateOptions
		{
			SuccessUrl = "https://app.auctionnext.com/success",
			CancelUrl = "https://app.auctionnext.com/cancel",
			PaymentMethodTypes = new List<string> {"card"}, // Force card payment collection
			Mode = "payment",
			// PaymentMethodTypes = new List<string> {"card",},
			LineItems = new List<SessionLineItemOptions>
			{
				new()
				{
					PriceData = new SessionLineItemPriceDataOptions
					{
						UnitAmount = (long) request.SoldAmount * 100, // Use the dynamic price from the request
						Currency = "usd",
						ProductData = new SessionLineItemPriceDataProductDataOptions
						{
							Name = request.Model, // Optionally, pass the product name dynamically
						},
					},
					Quantity = 1,
				},
			},
		};

		var service = new SessionService();
		Session session = await service.CreateAsync(options);

		if (session == null) return BadRequest("Failed to create session");


		request.StripeSessionUrl = session.Url;
		request.StripeSessionId = session.Id;
		// var b = request.AuctionId.ToString();

		try
		{
			var payment = _mapper.Map<Payment>(request);

			Console.WriteLine($"Payment {payment}");
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			throw;
		}


		Guid? auctionId = string.IsNullOrEmpty(request.AuctionId) ? (Guid?) null : Guid.Parse(request.AuctionId);

		var payment2 = new Payment
		{
			StripeSessionId = session.Id,
			AuctionId = auctionId,
			Name = request.Model,
			Total = request.SoldAmount,
		};

		// Console.WriteLine($"Payment {payment}");

		Console.WriteLine($"Payment2 {payment2}");

		try
		{
			_db.Payments.Add(payment2);

			await _db.SaveChangesAsync();
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			throw;
		}

		return Ok(new {session});
	}

	[HttpPost("validate")]
	public async Task<IActionResult> Validate()
	{
		return null;
	}

	[HttpPost("webhook")]
	public async Task<IActionResult> Webhook()
	{
		var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
		var stripeEvent =
			EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], _config["Stripe:WebhookSecret"]);

		switch (stripeEvent.Type)
		{
			case "checkout.session.completed":
				var session = stripeEvent.Data.Object as Session;
				// Fulfill the purchase...
				break;
			default:
				// Handle other event types...
				break;
		}

		return Ok();
	}
}