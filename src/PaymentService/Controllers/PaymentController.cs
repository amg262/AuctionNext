using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaymentService.Data;
using PaymentService.DTOs;
using PaymentService.Entities;
using PaymentService.Utility;
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
		var payments = await _db.Payments.ToListAsync();
		return Ok($"Payment Service {payments.Count}");
	}

	[HttpPost("create")]
	public async Task<IActionResult> Create([FromBody] StripeRequestDto stripeRequestDto)
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
						UnitAmount = (long) stripeRequestDto.SoldAmount * 100, 
						Currency = "usd",
						ProductData = new SessionLineItemPriceDataProductDataOptions
						{
							Name = stripeRequestDto.Model, // Optionally, pass the product name dynamically
						},
					},
					Quantity = 1,
				},
			},
		};

		var service = new SessionService();
		Session session = await service.CreateAsync(options);
		
		var paymentIntentService = new PaymentIntentService();

		// Create a new payment intent
		var paymentIntentCreateOptions = new PaymentIntentCreateOptions
		{
			Amount = (long) stripeRequestDto.SoldAmount * 100, // Convert to cents
			Currency = "usd",
			PaymentMethodTypes = new List<string> { "card" },
		};

		PaymentIntent paymentIntent = await paymentIntentService.CreateAsync(paymentIntentCreateOptions);

		if (session == null) return BadRequest("Failed to create session");

		stripeRequestDto.StripeSessionUrl = session.Url;
		stripeRequestDto.StripeSessionId = session.Id;
		stripeRequestDto.PaymentIntentId = paymentIntent.Id;

		try
		{
			var payment = _mapper.Map<Payment>(stripeRequestDto);

			_db.Payments.Add(payment);

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
	public async Task<IActionResult> Validate(string? sessionId)
	{
		try
		{
			Payment payment = await _db.Payments.FirstOrDefaultAsync(x => x.StripeSessionId == sessionId);
			var service = new SessionService();
			Session session = await service.GetAsync(payment.StripeSessionId);

			var paymentIntentService = new PaymentIntentService();
			
			PaymentIntent paymentIntent = await paymentIntentService.GetAsync(payment.PaymentIntentId);

			if (paymentIntent.Status == PaymentHelper.StatusSucceeded.ToLower())
			{
				//then payment was successful
				payment.PaymentIntentId = paymentIntent.Id;
				payment.Status = PaymentHelper.StatusApproved;
				await _db.SaveChangesAsync();
			}
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			throw;
		}

		return Ok("Hi");
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