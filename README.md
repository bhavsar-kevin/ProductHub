# ProductHub

ProductHub is a simple ASP.NET Core Web API for managing a product catalog and inventory. It is designed as a learning project for Git, GitHub, GitHub Actions, CI/CD, database migrations, testing, and deployment fundamentals.

## Technology Stack

- C#
- ASP.NET Core Web API
- .NET 8
- Entity Framework Core 8
- SQL Server
- xUnit
- Moq
- Swagger / OpenAPI

## Prerequisites

- .NET 8 SDK
- SQL Server LocalDB or SQL Server instance
- Visual Studio 2022 or VS Code
- EF Core CLI tools

## Project Structure

- src/ProductHub.Api - API project
- tests/ProductHub.Api.Tests - Unit tests

## Configuration

The database connection string is stored in appsettings.json and can be adjusted for your local SQL Server environment.

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=ProductHubDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

Use appsettings.Development.json for local development values and keep production values out of source control.

## Database Setup

This project uses EF Core with SQL Server. The basic local workflow is:

1. Ensure SQL Server is running locally.
2. Update the connection string in [src/ProductHub.Api/appsettings.Development.json](src/ProductHub.Api/appsettings.Development.json) if your server name or instance differs.
3. Create the initial migration.
4. Apply the migration to the database.

```bash
dotnet restore
dotnet ef migrations add InitialCreate
dotnet ef database update
```

What this does:

- `dotnet ef migrations add InitialCreate` creates a migration file that records the schema change for the `Products` table.
- `dotnet ef database update` applies the migration to the configured SQL Server database.

This keeps schema changes versioned in source control and makes it easy to reproduce the database from a clean checkout.

## Run Application

```bash
dotnet restore
dotnet build
dotnet test
dotnet run
```

Then open Swagger at https://localhost:5001/swagger or http://localhost:5000/swagger depending on your configuration.

## API Endpoints

- GET /api/products
- GET /api/products/{id}
- POST /api/products
- PUT /api/products/{id}
- DELETE /api/products/{id}

## Architecture

Controller -> Service -> Repository -> EF Core -> SQL Server

## Future CI/CD

This project intentionally does not include GitHub Actions, Docker, or cloud deployment yet. Those will be introduced in later phases after the local application is stable and tested.
