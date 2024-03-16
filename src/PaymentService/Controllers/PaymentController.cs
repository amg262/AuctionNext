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

/// <summary>
/// Handles payment-related requests, integrating Stripe for payment processing.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class PaymentController : ControllerBase
{
	private readonly IConfiguration _config;
	private readonly AppDbContext _db;
	private readonly IMapper _mapper;

	/// <summary>
	/// Initializes a new instance of the PaymentController class.
	/// </summary>
	/// <param name="config">Application configuration settings.</param>
	/// <param name="db">Database context for accessing payment data.</param>
	/// <param name="mapper">Automapper for DTO to entity mapping.</param>
	public PaymentController(IConfiguration config, AppDbContext db, IMapper mapper)
	{
		_config = config;
		_db = db;
		_mapper = mapper;
	}

	/// <summary>
	/// Retrieves a count of all payments in the system.
	/// </summary>
	/// <returns>A count of payments.</returns>
	[HttpGet]
	public async Task<IActionResult> Get()
	{
		var payments = await _db.Payments.ToListAsync();
		return Ok($"Payment Service {payments.Count}");
	}

	/// <summary>
	/// Creates a Stripe session for a payment request and saves the payment details in the database.
	/// </summary>
	/// <param name="stripeRequestDto">Data transfer object containing the payment request details.</param>
	/// <returns>The created Stripe session.</returns>
	[HttpPost("create")]
	public async Task<IActionResult> Create([FromBody] StripeRequestDto stripeRequestDto)
	{
		var options = new SessionCreateOptions
		{
			SuccessUrl = "https://app.auctionnext.com/payment/details/",
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
		session.SuccessUrl = $"https://app.auctionnext.com/payment/details/{session.Id}";

		var paymentIntentService = new PaymentIntentService();

		// Create a new payment intent
		var paymentIntentCreateOptions = new PaymentIntentCreateOptions
		{
			Amount = (long) stripeRequestDto.SoldAmount * 100, // Convert to cents
			Currency = "usd",
			PaymentMethodTypes = new List<string> {"card"},
		};

		PaymentIntent paymentIntent = await paymentIntentService.CreateAsync(paymentIntentCreateOptions);

		if (session == null) return BadRequest("Failed to create session");

		stripeRequestDto.StripeSessionUrl = session.Url;
		stripeRequestDto.StripeSessionId = session.Id;
		stripeRequestDto.PaymentIntentId = paymentIntent.Id;

		options.SuccessUrl = $"https://app.auctionnext.com/payment/details/{stripeRequestDto.StripeSessionId}";


		try
		{
			var payment = _mapper.Map<Payment>(stripeRequestDto);

			var paymentReferenceId = payment.Id.ToString();

			// Set a cookie with the payment reference ID
			var cookieOptions = new CookieOptions
			{
				HttpOnly = true, // Makes the cookie inaccessible to client-side scripts, enhancing security
				Secure = true, // Ensures the cookie is sent only over HTTPS
				SameSite = SameSiteMode.Strict, // Limits the context in which the cookie can be sent to the same site only
				Expires = DateTime.UtcNow.AddDays(1) // Sets the cookie expiration (adjust as necessary)
			};
			Response.Cookies.Append("PaymentReferenceId", paymentReferenceId, cookieOptions);
			
			Response.HttpContext.Response.Cookies.Append("PaymentReferenceId", paymentReferenceId, cookieOptions);

			HttpContext.Response.Cookies.Append("PaymentReferenceId", paymentReferenceId, cookieOptions);
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

	/// <summary>
	/// Validates a payment session and updates the payment status in the database.
	/// </summary>
	/// <param name="sessionId">Stripe SessionId from Create</param>
	/// <returns>The completed payment</returns>
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

	/// <summary>
	/// Handles webhook events sent by Stripe.
	/// </summary>
	/// <returns>A confirmation response.</returns>
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