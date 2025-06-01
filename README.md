# AccountingManagement

Accounting Management application built with .NET MAUI Blazor, Domain-Driven Design, and Clean Architecture.

## Solution Structure

The solution is organized into the following projects, adhering to Clean Architecture principles:

-   **`AccountingManagement.Domain`**: Contains all domain entities, value objects, domain events, aggregates, and repository interfaces. This layer has no dependencies on other layers in the solution.
-   **`AccountingManagement.Application`**: Contains application logic, including use cases (implemented as services or commands/queries), DTOs (Data Transfer Objects), and interfaces for infrastructure concerns (e.g., `IAppDbContext`, `IPasswordHasher`, `IDateTimeProvider`). This layer depends on `AccountingManagement.Domain`.
-   **`AccountingManagement.Infrastructure`**: Implements interfaces defined in the Application and Domain layers. This includes Entity Framework Core DbContext, repository implementations, and other services like password hashing or date/time providers. This layer depends on `AccountingManagement.Application`.
-   **`AccountingManagement.UI.Maui`**: The presentation layer, built with .NET MAUI Blazor. It handles user interaction, displays data, and communicates with the Application layer. This layer depends on `AccountingManagement.Application` and `AccountingManagement.Infrastructure` (for DI registration).

-   **`AccountingManagement.Domain.Tests`**: Unit tests for the Domain layer.
-   **`AccountingManagement.Application.Tests`**: Unit tests for the Application layer, typically mocking infrastructure dependencies.
-   **`AccountingManagement.Infrastructure.Tests`**: Integration tests for the Infrastructure layer, such as testing EF Core repositories against a test database.

## Technologies

-   .NET 9
-   .NET MAUI Blazor
-   Entity Framework Core 9 (for data access)
-   Tailwind CSS (for UI styling)
-   xUnit (for testing)
-   Clean Architecture
-   Domain-Driven Design principles

## Getting Started

1.  Ensure you have the .NET 9 SDK installed.
2.  Install Node.js and npm if you don't have them (for Tailwind CSS CLI).
3.  Navigate to `src/AccountingManagement.UI.Maui/` and run `npm install` to install Tailwind CSS dependencies.
4.  To build and run the Tailwind CSS compiler in watch mode:
    ```bash
    cd src/AccountingManagement.UI.Maui/
    npm run build:css
    ```
5.  Open `AccountingManagement.sln` in Visual Studio.
6.  Set `AccountingManagement.UI.Maui` as the startup project.
7.  Build and run the application.

## Database

The application currently uses an in-memory database for demonstration purposes. The schema is created and seeded on application startup. For a production environment, this should be configured to use a persistent database (e.g., SQL Server, PostgreSQL, SQLite) and Entity Framework Core migrations.