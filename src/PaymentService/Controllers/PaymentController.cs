using Microsoft.AspNetCore.Mvc;
using PaymentService.DTOs;
using Stripe;
using Stripe.Checkout;

namespace PaymentService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PaymentController : ControllerBase
{
	private readonly IConfiguration _config;

	public PaymentController(IConfiguration config)
	{
		_config = config;
		StripeConfiguration.ApiKey = _config["Stripe:SecretKey"];
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
						UnitAmount = request.SoldAmount, // Use the dynamic price from the request
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

		return Ok(new {session});
	}
}