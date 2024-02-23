using AuctionService.Controllers;
using AuctionService.Data;
using AuctionService.DTOs;
using AuctionService.Entities;
using AuctionService.RequestHelpers;
using AuctionService.UnitTests.Util;
using AutoFixture;
using AutoMapper;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace AuctionService.UnitTests;

/// <summary>
/// Contains unit tests for the AuctionsController to ensure it behaves as expected under various scenarios.
/// </summary>
public class AuctionControllerTests
{
	// Mocking dependencies and setting up common test utilities.
	private readonly Mock<IAuctionRepository> _auctionRepo;
	private readonly Mock<IPublishEndpoint> _publishEndpoint;
	private readonly Fixture _fixture; // AutoFixture is used for generating test data automatically.
	private readonly AuctionsController _controller;
	private readonly IMapper _mapper;

	/// <summary>
	/// Initializes new instances for the mock repository, publish endpoint, AutoFixture, AutoMapper, and the AuctionsController with a mock HttpContext to simulate user authentication.
	/// </summary>
	public AuctionControllerTests()
	{
		_fixture = new Fixture();
		_auctionRepo = new Mock<IAuctionRepository>();
		_publishEndpoint = new Mock<IPublishEndpoint>();

		var mockMapper = new MapperConfiguration(mc => { mc.AddMaps(typeof(MappingProfiles).Assembly); }).CreateMapper()
			.ConfigurationProvider;

		_mapper = new Mapper(mockMapper);

		// Injecting mocked dependencies into the controller and simulating a logged-in user.
		_controller = new AuctionsController(_auctionRepo.Object, _mapper, _publishEndpoint.Object)
		{
			ControllerContext = new ControllerContext
			{
				HttpContext = new DefaultHttpContext {User = Helpers.GetClaimsPrincipal(Helpers.TestUser)}
			}
		};
	}

	/// <summary>
	/// Verifies that GetAllAuctions returns 10 auctions when no specific parameters are provided.
	/// </summary>
	[Fact]
	public async Task GetAuctions_WithNoParams_Returns10Auctions()
	{
		// Arrange: Create 10 dummy AuctionDto objects to simulate a list of auctions returned from the repository.
		var auctions = _fixture.CreateMany<AuctionDto>(10).ToList();
		_auctionRepo.Setup(x => x.GetAuctionsAsync(null)).ReturnsAsync(auctions);

		// Act: Invoke the GetAllAuctions method on the controller.
		var result = await _controller.GetAllAuctions(null);

		// Assert: Verify the action result returns exactly 10 auctions.
		Assert.Equal(10, result?.Value?.Count);
		Assert.IsType<ActionResult<List<AuctionDto>>>(result);
	}

	/// <summary>
	/// Ensures GetAuctionById returns the correct auction when provided with a valid GUID.
	/// </summary>
	[Fact]
	public async Task GetAuctionById_WithValidGuid_ReturnsAuction()
	{
		// Arrange: Create a dummy AuctionDto object and setup the repository to return it when GetAuctionByIdAsync is called.
		var auction = _fixture.Create<AuctionDto>();
		_auctionRepo.Setup(repo => repo.GetAuctionByIdAsync(It.IsAny<Guid>())).ReturnsAsync(auction);

		// Act: Invoke the GetAuctionById method on the controller with a valid GUID.
		var result = await _controller.GetAuctionById(auction.Id);

		// Assert: Verify the action result returns the correct auction.
		Assert.Equal(auction.Make, result.Value.Make);
		Assert.IsType<ActionResult<AuctionDto>>(result);
	}

	/// <summary>
	/// Confirms GetAuctionById returns a NotFound result when an invalid GUID is provided.
	/// </summary>
	[Fact]
	public async Task GetAuctionById_WithInValidGuid_ReturnsNotFound()
	{
		// Arrange: Setup the repository to return null when an invalid GUID is passed to GetAuctionByIdAsync.
		_auctionRepo.Setup(repo => repo.GetAuctionByIdAsync(It.IsAny<Guid>())).ReturnsAsync(value: null);

		// Act: Invoke the GetAuctionById method on the controller with an invalid GUID.
		var result = await _controller.GetAuctionById(Guid.NewGuid());

		// Assert: Verify the action result is NotFound.
		Assert.IsType<NotFoundResult>(result.Result);
	}

	/// <summary>
	/// Tests that CreateAuction successfully returns a CreatedAtActionResult with the correct action name and DTO when provided with valid auction creation data.
	/// </summary>
	[Fact]
	public async Task CreateAuction_WithValidCreateAuctionDto_ReturnsCreatedAtAction()
	{
		// Arrange: Generate a dummy CreateAuctionDto object and configure the repository to acknowledge the addition of an Auction.
		var auctionDto = _fixture.Create<CreateAuctionDto>();
		_auctionRepo.Setup(repo => repo.AddAuction(It.IsAny<Auction>()));
		_auctionRepo.Setup(repo => repo.SaveChangesAsync()).ReturnsAsync(true);

		// Act: Invoke the CreateAuction method on the controller with the dummy DTO.
		var result = await _controller.CreateAuction(auctionDto);
		var createdResult = result.Result as CreatedAtActionResult;

		// Assert: Verify that the result is a CreatedAtActionResult pointing to the GetAuctionById action, indicating successful creation.
		Assert.NotNull(createdResult);
		Assert.Equal("GetAuctionById", createdResult.ActionName);
		Assert.IsType<AuctionDto>(createdResult.Value);
	}

	/// <summary>
	/// Validates that CreateAuction returns a BadRequest result when saving the auction to the database fails.
	/// </summary>
	[Fact]
	public async Task CreateAuction_FailedSave_Returns400BadRequest()
	{
		// Arrange: Setup the repository to simulate a failure in saving the Auction to the database.
		var auctionDto = _fixture.Create<CreateAuctionDto>();
		_auctionRepo.Setup(repo => repo.AddAuction(It.IsAny<Auction>()));
		_auctionRepo.Setup(repo => repo.SaveChangesAsync()).ReturnsAsync(false);

		// Act: Invoke the CreateAuction method on the controller with the dummy DTO.
		var result = await _controller.CreateAuction(auctionDto);

		// Assert: Verify that the result is a BadRequest, indicating the save operation failed.
		Assert.IsType<BadRequestObjectResult>(result.Result);
	}

	/// <summary>
	/// Checks that UpdateAuction returns an Ok response when an auction is successfully updated.
	/// </summary>
	[Fact]
	public async Task UpdateAuction_WithUpdateAuctionDto_ReturnsOkResponse()
	{
		// arrange
		var auction = _fixture.Build<Auction>().Without(x => x.Item).Create(); // create auction without item
		auction.Item = _fixture.Build<Item>().Without(x => x.Auction).Create(); // create item without auction
		auction.Seller = Helpers.TestUser;
		var updateDto = _fixture.Create<UpdateAuctionDto>();
		_auctionRepo.Setup(repo => repo.GetAuctionEntityById(It.IsAny<Guid>())).ReturnsAsync(auction);
		_auctionRepo.Setup(repo => repo.SaveChangesAsync()).ReturnsAsync(true);

		// act
		var result = await _controller.UpdateAuction(auction.Id, updateDto);

		// assert
		Assert.IsType<OkResult>(result);
	}

	/// <summary>
	/// Verifies UpdateAuction returns a Forbid result when the current user is not the seller of the auction.
	/// </summary>
	[Fact]
	public async Task UpdateAuction_WithInvalidUser_Returns403Forbid()
	{
		var auction = _fixture.Build<Auction>().Without(x => x.Item).Create();
		auction.Seller = Helpers.InvalidUser;
		var updateDto = _fixture.Create<UpdateAuctionDto>();
		_auctionRepo.Setup(repo => repo.GetAuctionEntityById(It.IsAny<Guid>())).ReturnsAsync(auction);

		// act
		var result = await _controller.UpdateAuction(auction.Id, updateDto);

		// assert
		Assert.IsType<ForbidResult>(result);
	}

	/// <summary>
	/// Confirms UpdateAuction returns a NotFound result when provided with an invalid auction GUID.
	/// </summary>
	[Fact]
	public async Task UpdateAuction_WithInvalidGuid_ReturnsNotFound()
	{
		// Arrange: Setup the repository to return null when an invalid GUID is passed to GetAuctionEntityById.
		var auction = _fixture.Build<Auction>().Without(x => x.Item).Create();
		auction.Seller = Helpers.InvalidUser;
		var updateDto = _fixture.Create<UpdateAuctionDto>();
		_auctionRepo.Setup(repo => repo.GetAuctionEntityById(It.IsAny<Guid>())).ReturnsAsync(value: null);

		// Act: Invoke the UpdateAuction method on the controller with an invalid GUID.
		var result = await _controller.UpdateAuction(auction.Id, updateDto);

		// Assert: Verify the action result is NotFound.
		Assert.IsType<NotFoundResult>(result);
	}

	/// <summary>
	/// Ensures DeleteAuction returns an Ok response when an auction is successfully deleted.
	/// </summary>
	[Fact]
	public async Task DeleteAuction_WithValidUser_ReturnsOkResponse()
	{
		// Arrange: Create a dummy auction and configure the repository to return it when GetAuctionEntityById is called.
		var auction = _fixture.Build<Auction>().Without(x => x.Item).Create();
		auction.Seller = Helpers.TestUser;
		_auctionRepo.Setup(repo => repo.GetAuctionEntityById(It.IsAny<Guid>()))
			.ReturnsAsync(auction);
		_auctionRepo.Setup(repo => repo.SaveChangesAsync()).ReturnsAsync(true);

		// Act: Invoke the DeleteAuction method on the controller with the dummy auction's GUID.
		var result = await _controller.DeleteAuction(auction.Id);

		// Assert: Verify the action result is Ok, indicating the auction was successfully deleted.
		Assert.IsType<OkResult>(result);
	}

	/// <summary>
	/// Tests that DeleteAuction returns a NotFound response when provided with an invalid auction GUID.
	/// </summary>
	[Fact]
	public async Task DeleteAuction_WithInvalidGuid_Returns404Response()
	{
		// Arrange: Setup the repository to return null when an invalid GUID is passed to GetAuctionEntityById.
		var auction = _fixture.Build<Auction>().Without(x => x.Item).Create();
		_auctionRepo.Setup(repo => repo.GetAuctionEntityById(It.IsAny<Guid>()))
			.ReturnsAsync(value: null);

		// Act: Invoke the DeleteAuction method on the controller with an invalid GUID.
		var result = await _controller.DeleteAuction(auction.Id);

		// Assert: Verify the action result is NotFound.
		Assert.IsType<NotFoundResult>(result);
	}

	/// <summary>
	/// Validates that DeleteAuction returns a Forbid result when the current user is not the seller of the auction.
	/// </summary>
	[Fact]
	public async Task DeleteAuction_WithInvalidUser_Returns403Response()
	{
		// Arrange: Create a dummy auction and configure the repository to return it when GetAuctionEntityById is called.
		var auction = _fixture.Build<Auction>().Without(x => x.Item).Create();
		auction.Seller = Helpers.InvalidUser;
		_auctionRepo.Setup(repo => repo.GetAuctionEntityById(It.IsAny<Guid>()))
			.ReturnsAsync(auction);

		// Act: Invoke the DeleteAuction method on the controller with the dummy auction's GUID.
		var result = await _controller.DeleteAuction(auction.Id);

		// Assert: Verify the action result is Forbid, indicating the user is not authorized to delete the auction.
		Assert.IsType<ForbidResult>(result);
	}
}