using AutoMapper;
using EasyPost;
using EasyPost.Exceptions.API;
using EasyPost.Models.API;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaymentService.Data;
using PaymentService.DTOs;
using PaymentService.Entities;
using PaymentService.Services;

namespace PaymentService.Controllers;

/// <summary>
/// Handles shipping-related actions, including listing, creating, updating, and deleting shipping records,
/// as well as completing shipping actions by integrating with the EasyPost API.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ShippingController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly ShippingService _shippingService;
    private readonly IMapper _mapper;
    private readonly IConfiguration _config;
    public Client myClient;

    /// <summary>
    /// Initializes a new instance of the ShippingController class.
    /// </summary>
    /// <param name="db">Database context for accessing shipping records.</param>
    /// <param name="shippingService">Service for handling shipping logic and external API integration.</param>
    /// <param name="mapper">Automapper instance for model mapping.</param>
    /// <param name="config">Configuration for accessing application settings.</param>
    /// <param name="myClient">Shipping Client object for EastPost</param>
    public ShippingController(AppDbContext db, ShippingService shippingService, IMapper mapper, IConfiguration config)
    {
        _db = db;
        _shippingService = shippingService;
        _mapper = mapper;
        _config = config;
    }

    /// <summary>
    /// Retrieves all shipping records from the database.
    /// </summary>
    /// <returns>A list of shipping records.</returns>
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        List<Shipping?> items = await _db.Shipping.ToListAsync();

        return Ok(items);
    }

    /// <summary>
    /// Adds a new shipping record to the database.
    /// </summary>
    /// <param name="shippingDto">DTO for transferring Shipping data</param>
    /// <returns>The added shipping record.</returns>
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] ShippingDto shippingDto)
    {
        if (shippingDto == null) return BadRequest("Shipping data is required.");

        var shipping = _mapper.Map<Shipping>(shippingDto);
        await _db.Shipping.AddAsync(shipping);
        await _db.SaveChangesAsync();
        return Ok(shipping);
    }

    /// <summary>
    /// Updates an existing shipping record in the database.
    /// </summary>
    /// <param name="id">ID of the Shipping record to update</param>
    /// <param name="shippingDto">DTO to transfer Shipping data</param>
    /// <returns>The updated shipping record.</returns>
    [HttpPut]
    public async Task<IActionResult> Put(string id, [FromBody] ShippingDto shippingDto)
    {
        if (shippingDto == null) return BadRequest("Shipping data is required.");

        var shipping = _mapper.Map<Shipping>(shippingDto);
        _db.Shipping.Update(shipping);
        await _db.SaveChangesAsync();
        return Ok(shipping);
    }

    /// <summary>
    /// Deletes a shipping record from the database.
    /// </summary>
    /// <param name="id">The ID of the shipping record to delete.</param>
    /// <returns>An IActionResult indicating the outcome of the operation.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _db.Shipping.FirstOrDefaultAsync(c => c.ShippingId == id);
        _db.Shipping.Remove(item);
        await _db.SaveChangesAsync();
        return Ok();
    }

    /// <summary>
    /// Retrieves a shipping record by its ID.
    /// </summary>
    /// <param name="id">The ID of the shipping record to retrieve.</param>
    /// <returns>The requested shipping record.</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetShippingById(int id)
    {
        var item = await _db.Shipping.FirstOrDefaultAsync(c => c.ShippingId == id);
        return Ok(item);
    }

    /// <summary>
    /// Completes the shipping process for a given payment, creating a shipment with EasyPost and updating the shipping record.
    /// </summary>
    /// <param name="paymentId">The ID of the payment associated with the shipping.</param>
    /// <returns>The updated shipping record with tracking information.</returns>
    /// <exception cref="InvalidRequestError">Thrown when the request to EasyPost API is invalid.</exception>
    /// <exception cref="ApiError">Thrown when there's an error in the EasyPost API request.</exception>
    /// <exception cref="Exception">Thrown when an unexpected error occurs.</exception>
    [HttpGet("{paymentId}")]
    public async Task<IActionResult> GetShippingByPaymentId(Guid paymentId)
    {
        var item = await _db.Shipping.FirstOrDefaultAsync(c => c.PaymentId == paymentId);
        return Ok(item);
    }

    /// <summary>
    /// Completes the shipping process for a specified payment.
    /// </summary>
    /// <param name="paymentId">The unique identifier of the payment for which to complete the shipping.</param>
    /// <returns>Returns an <see cref="IActionResult"/> indicating the outcome of the operation. 
    /// If the payment is not found, returns <see cref="NotFound"/>. 
    /// If the shipping process is completed successfully, returns the updated shipping details with <see cref="Ok"/>.
    /// In case of errors, various exceptions may be thrown, which are handled and rethrown to be caught by global error handlers.</returns>
    /// <remarks>
    /// This method retrieves payment and shipping details from the database, creates shipment details, sends them to an external shipping service provider (using EasyPost API), and updates the shipping information in the database.
    /// </remarks>
    /// <exception cref="InvalidRequestError">Thrown when there is an invalid request to the shipping service API.</exception>
    /// <exception cref="ApiError">Thrown when there is a general API error from the shipping service.</exception>
    /// <exception cref="Exception">General exception for any other errors that occur during the process.</exception>
    [HttpPost("complete/{paymentId}")]
    public async Task<IActionResult> CompleteShipping(Guid paymentId)
    {
        var payment = await _db.Payments.FirstOrDefaultAsync(c => c.Id == paymentId);
        var shipping = await _db.Shipping.FirstOrDefaultAsync(b => b.PaymentId == paymentId);
        if (payment == null) return NotFound();

        try
        {
            var to1 = new Address
            {
                Name = shipping.Name,
                Street1 = shipping.Street1,
                Street2 = shipping.Street2,
                City = shipping.City,
                State = shipping.State,
                Zip = shipping.Zip,
                Country = shipping.Country,
                Company = shipping.Name,
            };

            var from1 = new Address
            {
                Name = "AuctionNext",
                Street1 = "s75 w33075 Rolling Fields Drive",
                City = "Mukwonago",
                State = "WI",
                Zip = "53149",
                Country = "US",
                Phone = "262-363-9999",
                Company = "AuctionNext"
            };

            var parcel = new Parcel
            {
                Length = 9,
                Width = 6,
                Height = 2,
                Weight = 10 // Assuming weight is in ounces
            };

            myClient = new Client(new ClientConfiguration(_config["EasyPost:ApiKey"]));


            Shipment myShipment = await myClient.Shipment.Create(new Dictionary<string, object>
            {
                { "from_address", from1 },
                { "to_address", to1 },
                {
                    "parcel", new Dictionary<string, object>
                    {
                        { "length", 10 },
                        { "width", 20 },
                        { "height", 30 },
                        { "weight", 200 },
                        // {"length", 9},
                        // {"width", 6},
                        // {"height", 2},
                        // {"weight", 10}

                        // {"width", 6},
                        // {"height", 2},
                        // {"weight", 10}
                    }
                }
            });
            Shipment myPurchasedShipment = await myClient.Shipment.Buy(myShipment.Id, myShipment.LowestRate());
            // myShipment = await myClient.Shipment.Buy(myShipment.Id, myShipment.LowestRate());

            shipping.TrackingCode = myPurchasedShipment.TrackingCode;
            shipping.TrackingUrl = myPurchasedShipment.Tracker.PublicUrl;
            shipping.Rate = myPurchasedShipment.Rates[0].Price.ToString();
            shipping.Carrier = myPurchasedShipment.Rates[0].Carrier;
            shipping.UpdatedAt = DateTime.UtcNow;

            _db.Shipping.Update(shipping);
            await _db.SaveChangesAsync();

            return Ok(shipping);
        }
        catch (InvalidRequestError error)
        {
            Console.WriteLine($"Invalid Request Error: {error.Message}");
            throw;
        }
        catch (ApiError e)
        {
            // Handle any API exceptions (e.g., invalid address format)
            Console.WriteLine($"API Exception: {e.Message}");
            throw;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    /// <summary>
    /// Verifies an address using the shipping service.
    /// </summary>
    /// <param name="toVerify">The address to verify.</param>
    /// <returns>The verified address.</returns>
    public async Task<IActionResult> VerifyAddress(Address toVerify)
    {
        var verifiedAddress = await _shippingService.VerifyAddress(toVerify);
        return Ok(verifiedAddress);
    }
}