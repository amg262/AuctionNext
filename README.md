# AuctionNext

AuctionNext is an online auction service that allows users to buy and sell items through an auction process. This README provides an overview of the project structure and key components.

## Table of Contents

- [Introduction](#introduction)
- [Technologies](#technologies)
- [Project Structure](#project-structure)
- [Getting Started](#getting-started)
- [Usage](#usage)
- [Contributing](#contributing)
- [License](#license)

## Introduction

AuctionNext is a web application built using ASP.NET Core and Entity Framework Core. It provides a platform for users to create auctions, bid on items, and monitor auction status. The application uses a PostgreSQL database to store auction and item data.

## Docker

```markdown
docker compose up -d
```

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

- `src/AuctionService/`: The main project folder.
  - `Entities/`: Contains the entity classes representing auctions and items.
  - `Data/`: Contains the database context and migrations.
  - `Migrations/`: Contains the database migration files.
  - `Properties/`: Contains project-related properties files.
  - `appsettings.json`: Contains the application settings.
  - `appsettings.Development.json`: Contains the development-specific application settings.
  - `DbInitializer.cs`: Contains the database initializer class.
  - `Program.cs`: Contains the entry point of the application.
  - `Status.cs`: Contains the enum representing auction status.
- `test/AuctionService.Tests/`: Contains the unit tests for the application.

## Getting Started

To get started with the AuctionNext project, follow these steps:

1. Clone the repository: `git clone <repository-url>`
2. Navigate to the project directory: `cd AuctionNext/src/AuctionService`
3. Restore the dependencies: `dotnet restore`
4. Update the database connection string in `appsettings.Development.json` to match your PostgreSQL database configuration.
5. Apply the database migrations: `dotnet ef database update`
6. Run the application: `dotnet run`

## Usage

Once the application is running, you can access the API documentation using Swagger UI. Open your web browser and navigate to `https://localhost:5001/swagger/index.html`. Here, you can explore the available endpoints and test them.

## Contributing

Contributions to the AuctionNext project are welcome. To contribute, follow these steps:

1. Fork the repository
2. Create a new branch: `git checkout -b feature/your-feature-name`
3. Make your changes and commit them: `git commit -am 'Add some feature'`
4. Push the changes to your fork: `git push origin feature/your-feature-name`
5. Create a new pull request
