namespace AuctionService.IntegrationTests.Fixtures;

/// <summary>
/// Defines a shared test collection for AuctionService integration tests.
/// This class leverages Xunit's CollectionDefinition attribute to create a shared context,
/// utilizing the CustomWebAppFactory to initialize and dispose resources common to all tests in the collection.
/// </summary>
/// <remarks>
/// The SharedFixture class acts as a container for shared test context setup, ensuring that the CustomWebAppFactory
/// instance is created once and reused across all tests within the "Shared collection". This approach helps to
/// minimize the overhead of repeatedly setting up and tearing down the test environment, promoting faster test execution
/// and consistency in the testing environment.
/// 
/// Usage:
/// To include a test class in this collection, simply annotate the test class with the
/// [Collection("Shared collection")] attribute. Xunit will automatically recognize this configuration and handle
/// the lifecycle of the shared context as defined by the CustomWebAppFactory.
/// </remarks>
[CollectionDefinition("Shared collection")]
public class SharedFixture : ICollectionFixture<CustomWebAppFactory>
{
}