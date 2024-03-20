using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaymentService.Data;
using PaymentService.DTOs;
using PaymentService.Entities;
using PaymentService.Utility;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using Stripe;
using Stripe.Checkout;
using Coupon = PaymentService.Entities.Coupon;

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
		return Ok(payments);
	}

	/// <summary>
	/// Creates a Stripe session for a payment request and saves the payment details in the database.
	/// </summary>
	/// <param name="stripeRequestDto">Data transfer object containing the payment request details.</param>
	/// <returns>The created Stripe session.</returns>
	[HttpPost("create")]
	public async Task<IActionResult> Create([FromBody] StripeRequestDto stripeRequestDto)
	{
		try
		{
			var payment = new Payment
			{
				Id = stripeRequestDto.Guid
			};


			var options = new SessionCreateOptions
			{
				SuccessUrl = $"https://app.auctionnext.com/payment/details/{stripeRequestDto.Guid}",
				CancelUrl = "https://app.auctionnext.com/",
				PaymentMethodTypes = new List<string> {"card"}, // Force card payment collection
				Mode = "payment",
				Discounts = new List<SessionDiscountOptions>()
				{
					new()
					{
						Coupon = stripeRequestDto.CouponCode ?? "10OFF"
					}
				},
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

			// var paymentIntentService = new PaymentIntentService();
			//
			// // Create a new payment intent
			// var paymentIntentCreateOptions = new PaymentIntentCreateOptions
			// {
			// 	Amount = (long) stripeRequestDto.SoldAmount * 100, // Convert to cents
			// 	Currency = "usd",
			// 	PaymentMethodTypes = new List<string> {"card"},
			// };
			//
			// PaymentIntent paymentIntent = await paymentIntentService.CreateAsync(paymentIntentCreateOptions);


			if (session == null) return BadRequest("Failed to create session");

			// session.PaymentIntent = paymentIntent;

			stripeRequestDto.StripeSessionUrl = session.Url;
			stripeRequestDto.StripeSessionId = session.Id;
			// stripeRequestDto.PaymentIntentId = paymentIntent.Id;


			payment = _mapper.Map<Payment>(stripeRequestDto);

			_db.Payments.Add(payment);

			await _db.SaveChangesAsync();

			return Ok(new {payment, session});
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			throw;
		}
	}

	/// <summary>
	/// Validates a payment session and updates the payment status in the database.
	/// </summary>
	/// <param name="paymentId">Stripe </param>
	/// <returns>The completed payment</returns>
	[HttpPost("validate")]
	public async Task<IActionResult> Validate(Guid? paymentId)
	{
		try
		{
			Payment payment = await _db.Payments.FirstOrDefaultAsync(x => x.Id == paymentId);


			var service = new SessionService();
			Session session = await service.GetAsync(payment.StripeSessionId);

			// session.PaymentIntent = await new PaymentIntentService().GetAsync(session.PaymentIntentId);


			// // Create a new payment intent
			// var paymentIntentCreateOptions = new PaymentIntentCreateOptions
			// {
			// 	Amount = (long) payment.Total * 100, // Convert to cents
			// 	Currency = "usd",
			// 	PaymentMethodTypes = new List<string> {"card"},
			// };
			// var paymentIntentService = new PaymentIntentService();
			//
			// PaymentIntent paymentIntent = await paymentIntentService.CreateAsync(paymentIntentCreateOptions);
			//
			//
			//
			// payment.PaymentIntentId = paymentIntent.Id;

			payment.PaymentIntentId = session.PaymentIntentId;
			_db.Payments.Update(payment);
			await _db.SaveChangesAsync();

			// PaymentIntent paymentIntent = await paymentIntentService.GetAsync(payment.PaymentIntentId);

			// var id = paymentIntent.Id;
			//
			// if (paymentIntent.Status == PaymentHelper.StatusSucceeded.ToLower() ||
			//     paymentIntent.Status == PaymentHelper.RequiresPaymentMethod.ToLower())
			// {
			// 	//then payment was successful
			// 	// payment.PaymentIntentId = paymentIntent.Id;
			// 	payment.Status = PaymentHelper.StatusApproved;
			// 	payment.UpdatedAt = DateTime.UtcNow;
			// 	await _db.SaveChangesAsync();
			// }

			return Ok(payment);
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			throw;
		}

		return NotFound();
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

	/// <summary>
	/// Gets a payment by its ID.
	/// </summary>
	/// <param name="id">Payment ID field</param>
	/// <returns>Payment object</returns>
	[HttpGet("{id}")]
	public async Task<IActionResult> GetPaymentById(string? id)
	{
		var payment = await _db.Payments.FirstOrDefaultAsync(x => x.Id.ToString() == id);

		if (payment == null) return NotFound();

		return Ok(payment);
	}

	/// <summary>
	/// Deletes a payment from the database.
	/// </summary>
	/// <param name="id">Payment ID</param>
	/// <returns>200 status code</returns>
	[HttpDelete("{id}")]
	public async Task<IActionResult> DeletePayment(string? id)
	{
		var payment = await _db.Payments.FirstOrDefaultAsync(x => x.Id.ToString() == id);
		if (payment == null) return NotFound();
		_db.Payments.Remove(payment);
		await _db.SaveChangesAsync();
		return Ok();
	}

	[HttpGet("receipt/{id}")]
	public async Task<IActionResult> GenerateReceipt(Guid id)
	{
		var payment = await _db.Payments.FirstOrDefaultAsync(x => x.Id == id);
		if (payment == null)
		{
			return NotFound("Payment not found.");
		}

		// Create a new PDF document
		PdfDocument document = new PdfDocument();
		document.Info.Title = $"Payment Receipt - {payment.Id}";

		// Create an empty page
		PdfPage page = document.AddPage();

		// Get an XGraphics object for drawing
		XGraphics gfx = XGraphics.FromPdfPage(page);

		// Create a font
		XFont font = new XFont("Verdana", 20, XFontStyleEx.Bold);

		// Draw the text
		gfx.DrawString("Payment Receipt", font, XBrushes.Black, new XRect(0, 20, page.Width, page.Height),
			XStringFormats.TopCenter);

		// Draw more details about the payment
		XFont detailsFont = new XFont("Verdana", 12, XFontStyleEx.Regular);
		gfx.DrawString($"Payment ID: {payment.Id}", detailsFont, XBrushes.Black, new XPoint(40, 100));
		gfx.DrawString($"User ID: {payment.UserId}", detailsFont, XBrushes.Black, new XPoint(40, 130));
		gfx.DrawString($"Total: ${payment.Total}", detailsFont, XBrushes.Black, new XPoint(40, 160));
		gfx.DrawString($"Status: {payment.Status}", detailsFont, XBrushes.Black, new XPoint(40, 190));
		gfx.DrawString($"Date: {payment.UpdatedAt:yyyy-MM-dd HH:mm:ss}", detailsFont, XBrushes.Black,
			new XPoint(40, 220));

		// Save the document into a memory stream
		using MemoryStream stream = new MemoryStream();
		document.Save(stream, false);
		return File(stream.ToArray(), "application/pdf", $"PaymentReceipt-{payment.Id}.pdf");
	}


	[HttpPost("coupon")]
	public async Task<IActionResult> Coupon()
	{
		return Ok("coupon");
	}

	[HttpPost("create-coupon")]
	public async Task<IActionResult> CreateCoupon([FromBody] CouponDto? couponDto)
	{
		var b = couponDto;
		Console.WriteLine(b);
		try
		{
			Coupon coupon = _mapper.Map<Coupon>(couponDto);
			_db.Coupons.Add(coupon);
			await _db.SaveChangesAsync();

			// Create the coupon in Stripe
			var options = new Stripe.CouponCreateOptions
			{
				AmountOff = (long) (couponDto.DiscountAmount * 100),
				Name = couponDto.CouponCode,
				Currency = "usd",
				Id = couponDto.CouponCode,
			};
			var service = new Stripe.CouponService();
			await service.CreateAsync(options);

			var result = _mapper.Map<CouponDto>(coupon);

			return Ok(result);
		}
		catch (StripeException se)
		{
			Console.WriteLine(se);
			throw;
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			throw;
		}

		return BadRequest();
	}
}