# AuctionNext

AuctionNext is an online auction service that allows users to buy and sell items through an auction process.

[![wakatime](https://wakatime.com/badge/github/amg262/AuctionNext.svg)](https://wakatime.com/badge/github/amg262/AuctionNext)

## How to run the project

1. Clone the repository: `git clone <repository-url>`
2. Navigate to the project directory:

```bash
cd AuctionNext
```

3. Build the services locally on your computer by running

```bash
docker compose build
```

4. Run the services by running

```bash
docker compose up -d
````

5. To see the app working you will need to provide it with an SSL certificate. To do this please install 'mkcert' onto
   your computer which you can get from [here](https://github.com/FiloSottile/mkcert). Once you have this you will need
   to install the local Certificate
   Authority by using:

```bash
mkcert -install
```

6. Once you have the local Certificate Authority installed you can then generate the SSL certificate by running:

```bash
cd devcerts
mkcert -key-file auctionnext.com.key -cert-file auctionnext.com.crt app.auctionnext.com api.auctionnext.com id.auctionnext.com
```

7. Add entry to your hosts file for domain name resolution:

```bash
127.0.0.1 id.auctionnext.com app.auctionnext.com api.auctionnext.com
```

## Table of Contents

- [Introduction](#introduction)
- [Technologies](#technologies)
- [Project Structure](#project-structure)
- [Getting Started](#getting-started)
- [Usage](#usage)
- [Contributing](#contributing)
- [License](#license)

## Introduction

AuctionNext is a web application built using ASP.NET Core and Entity Framework Core. It provides a platform for users to
create auctions, bid on items, and monitor auction status. The application uses a PostgreSQL database to store auction
and item data.

## Technologies

The following technologies and libraries are used in this project:

- ASP.NET Core: A cross-platform framework for building web applications.
- Entity Framework Core: An object-relational mapping (ORM) framework for .NET.
- PostgreSQL: A powerful, open-source relational database management system.
- Swagger: A tool for documenting and testing APIs.
- Newtonsoft.Json: A popular JSON framework for .NET.
- Npgsql.EntityFrameworkCore.PostgreSQL: A PostgreSQL provider for Entity Framework Core.

## Project Structure

The project structure is organized as follows:

- `src/AuctionService/`: The main project folder for the AuctionService.
    - `Entities/`: Contains the entity classes representing auctions and items.
    - `Data/`: Contains the database context and migrations.
    - `Migrations/`: Contains the database migration files.
    - `Properties/`: Contains project-related properties files.
    - `appsettings.json`: Contains the application settings.
    - `appsettings.Development.json`: Contains the development-specific application settings.
    - `DbInitializer.cs`: Contains the database initializer class.
    - `Program.cs`: Contains the entry point of the application.
    - `Status.cs`: Contains the enum representing auction status.

- `src/SearchService/`: The main project folder for the SearchService.
    - `Data/`: Contains the data access layer for the search service.
    - `Services/`: Contains the business logic layer for the search service.
    - `Consumers/`: Contains the message consumers for the search service.
    - `Program.cs`: Contains the entry point of the application.

- `src/IdentityService/`: The main project folder for the IdentityService.
    - `Data/`: Contains the data access layer for the identity service.
    - `Models/`: Contains the models used in the identity service.
    - `Pages/`: Contains the Razor Pages for the identity service.
    - `Program.cs`: Contains the entry point of the application.

- `src/GatewayService/`: The main project folder for the GatewayService.
    - `Controllers/`: Contains the controllers for the gateway service.
    - `Services/`: Contains the services used in the gateway service.
    - `Program.cs`: Contains the entry point of the application.

- `tests/AuctionService.Tests/`: Contains the unit tests for the AuctionService.

## Getting Started

To get started with the AuctionNext project, follow these steps:

1. Clone the repository: `git clone <repository-url>`
2. Navigate to the project directory: `cd AuctionNext`
3. Restore the dependencies: `dotnet restore`
4. Update the database connection string in the respective `appsettings.Development.json` files to match your PostgreSQL
   database configuration.
5. Apply the database migrations for each project: `dotnet ef database update` (
   e.g., `dotnet ef database update --project src/AuctionService`)
6. Run each project individually using the `dotnet run` command (e.g., `dotnet run --project src/AuctionService`)

## Usage

Once the application is running, you can access the API documentation using Swagger UI. Open your web browser and
navigate to the respective URLs for each project:

- AuctionService: `https://localhost:5001/swagger/index.html`
- SearchService: `https://localhost:7002/swagger/index.html`
- IdentityService: `https://localhost:5000/swagger/index.html`
- GatewayService: `https://localhost:6001/swagger/index.html`

Here, you can explore the available endpoints and test them.

## Contributing

Contributions to the AuctionNext project are welcome. To contribute, follow these steps:

1. Fork the repository
2. Create a new branch: `git checkout -b feature/your-feature-name`
3. Make your changes and commit them: `git commit -am 'Add some feature'`
4. Push the changes to your fork: `git push origin feature/your-feature-name`
5. Create a new pull request

## License

The AuctionNext project is licensed under the [MIT License](LICENSE).