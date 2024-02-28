FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Copy csproj and restore as distinct layers. Use of the same COPY command to copy the csproj file and the 
# rest of the files is a best practice to take advantage of Docker's layer caching. This way, Docker can 
# reuse the layers created during the restore operation if the csproj file has not changed.
COPY AuctionNext.sln AuctionNext.sln
COPY src/AuctionService/AuctionService.csproj src/AuctionService/AuctionService.csproj
COPY src/SearchService/SearchService.csproj src/SearchService/SearchService.csproj
COPY src/IdentityService/IdentityService.csproj src/IdentityService/IdentityService.csproj
COPY src/GatewayService/GatewayService.csproj src/GatewayService/GatewayService.csproj
COPY src/BiddingService/BiddingService.csproj src/BiddingService/BiddingService.csproj
COPY src/Contracts/Contracts.csproj src/Contracts/Contracts.csproj
COPY tests/AuctionService.UnitTests/AuctionService.UnitTests.csproj tests/AuctionService.UnitTests/AuctionService.UnitTests.csproj
COPY tests/AuctionService.IntegrationTests/AuctionService.IntegrationTests.csproj tests/AuctionService.IntegrationTests/AuctionService.IntegrationTests.csproj
COPY tests/SearchService.UnitTests/SearchService.UnitTests.csproj tests/SearchService.UnitTests/SearchService.UnitTests.csproj


# Restore the dependencies
RUN dotnet restore AuctionNext.sln

# Copy the app folders
COPY src/GatewayService src/GatewayService

# publish the app
WORKDIR /app/src/GatewayService
RUN dotnet publish -c Release -o /app/src/out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/src/out .
ENTRYPOINT ["dotnet", "GatewayService.dll"]