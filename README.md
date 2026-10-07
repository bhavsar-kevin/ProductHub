# ProductHub

ProductHub is a hands-on ASP.NET Core application being progressively evolved into a complete CI/CD demonstration project using GitHub, GitHub Actions, automated testing, Docker, container registries, cloud deployment, security checks, and production delivery practices.

This project is intentionally simple at the application layer so the focus stays on learning the software delivery lifecycle in a practical, step-by-step way.

## Technology Stack

- C#
- ASP.NET Core Web API
- .NET 8
- Entity Framework Core
- SQL Server
- xUnit
- Moq
- Swagger / OpenAPI

## 🎯 Learning Goal

The overall goal of ProductHub is to understand the complete journey from code creation to production delivery:

Developer
→ Git
→ GitHub
→ Pull Request
→ CI
→ Automated Tests
→ Code Quality
→ Security
→ Docker
→ Container Registry
→ Deployment
→ Environments
→ Production

This project is being developed phase-by-phase so each concept is learned through hands-on implementation rather than simply copying configuration files.

## 📍 Current Phase

Phase 1 — ASP.NET Core Application ✅ COMPLETED

Phase 2 — Git & GitHub ✅ COMPLETED

Phase 3 — GitHub Actions CI ✅ COMPLETED

Phase 4 — Pull Request CI ✅ COMPLETED

Pull Requests are now protected by automated CI checks before merging into `main`.

```text
Phase 1  ████████████████████ 100%
Phase 2  ████████████████████ 100%
Phase 3  ████████████████████ 100%
Phase 4  ████████████████████ 100%
Phase 5  ░░░░░░░░░░░░░░░░░░░░ 0%
```

Phase 2 is complete. The project is now hosted in GitHub with a basic feature-branch and Pull Request workflow. The next step is to introduce GitHub Actions and automatically build and test the application.

## 🗺️ CI/CD Learning Roadmap

| Phase | Topic | Status |
|---|---|---|
| 1 | ASP.NET Core Application | ✅ Completed |
| 2 | Git & GitHub | ✅ Completed |
| 3 | First GitHub Actions CI Pipeline | ✅ Completed |
| 4 | Pull Request CI | ✅ Completed |
| 5 | Test Coverage & Code Quality | 🚧 Next |
| 6 | Secrets & Configuration | ⏳ Planned |
| 7 | Docker | ⏳ Planned |
| 8 | Container Registry | ⏳ Planned |
| 9 | Continuous Deployment | ⏳ Planned |
| 10 | Environments & Approval Gates | ⏳ Planned |
| 11 | Integration Testing | ⏳ Planned |
| 12 | Security & Dependency Scanning | ⏳ Planned |
| 13 | Deployment Strategies & Rollbacks | ⏳ Planned |

# Phase 1 — ASP.NET Core Application

Status:

✅ COMPLETED

Phase 1 established the application foundation for the project.

The application includes:

- ASP.NET Core Web API
- .NET 8
- Entity Framework Core
- SQL Server
- Repository Pattern
- Service Layer
- DTOs
- REST CRUD APIs
- Validation
- Global exception handling
- Swagger/OpenAPI
- EF Core migrations
- Unit testing with xUnit and Moq

Architecture:

Controller
↓
Service
↓
Repository
↓
EF Core
↓
SQL Server

# Phase 2 — Git & GitHub

Status:

✅ COMPLETED

Phase 2 introduced source control and collaborative development practices.

The concepts practiced included:

- Git repository initialization
- `.gitignore`
- Initial commit
- GitHub remote repository
- `main` branch
- Feature branches
- Pull Requests
- Code review
- Merge
- Branch synchronization
- Branch cleanup
- Merge conflict resolution

Workflow:

Developer
↓
Feature Branch
↓
Commit
↓
Push
↓
Pull Request
↓
Review
↓
Merge
↓
main

This workflow establishes the foundation for GitHub Actions and automated delivery pipelines.

# 🧠 What I Learned So Far

### Phase 1

- How to structure an ASP.NET Core Web API
- Separation of controller, service, and repository responsibilities
- EF Core database access
- Unit testing business logic
- Creating repeatable build/test commands

### Phase 2

- Difference between Git and GitHub
- Working directory, staging area, and repository
- Commits and history
- Branch-based development
- Pull Requests
- Remote repositories
- Merge conflicts
- Collaborative development workflow

### Phase 3 & 4 — Pull Request CI

- Configured CI to run automatically on Pull Requests.
- Added branch protection for `main`.
- Required Pull Request approval before merging.
- Required CI status checks to pass before merging.
- Learned how failed CI checks block a merge.
- Verified the complete cycle: CI failure → fix → CI pass → merge.

# 🔄 Current Development Workflow

Developer
    ↓
Feature Branch
    ↓
Code + Tests
    ↓
Push
    ↓
Pull Request
    ↓
GitHub Actions CI
    ↓
Build + Test
    ↓
CI Pass ✅
    ↓
Code Review
    ↓
Merge → main

Currently, the build and tests are still triggered manually by the developer.

This is intentional because the next phase will automate this process using GitHub Actions.

# 🚦 Current CI/CD State

Source Control ✅ Git + GitHub  
Application Build ✅  
Local Unit Tests ✅  
Pull Requests ✅  
Automated CI ✅  
Pull Request CI ✅  
Branch Protection ✅  
Test Coverage ⏳  
Code Quality ⏳  
Security Scanning ⏳  
Docker ⏳  
Container Registry ⏳  
Continuous Deployment ⏳  
Cloud Deployment ⏳  
Production Pipeline ⏳  

ProductHub now has an automated GitHub Actions CI pipeline that restores dependencies, builds the application, and runs unit tests.

Pull Requests targeting `main` are protected by required CI checks and code review. Failed CI checks block the merge until the issue is fixed.

# 🚀 Next Phase — Test Coverage & Code Quality

Phase 5 will introduce automated test coverage and code quality checks into the CI pipeline.

The pipeline will progressively evolve to:

git push / Pull Request
        ↓
GitHub Actions
        ↓
Checkout source
        ↓
Setup .NET
        ↓
Restore dependencies
        ↓
Build application
        ↓
Run unit tests
        ↓
Test Coverage
        ↓
Code Quality
        ↓
Pass / Fail

## 🧠 Phase 4 — What I Learned

- Pull Request CI
- Required status checks
- Branch protection
- Required code review
- CI as a merge gate
- Build failure blocks merging
- Test failure blocks merging
- Fixing CI failures and re-running the pipeline

## Project Structure

```text
ProductHub/
│
├── src/
│   └── ProductHub.Api/
│
├── tests/
│   └── ProductHub.Api.Tests/
│
├── .gitignore
├── README.md
└── ProductHub.sln
```

## Prerequisites

- .NET 8 SDK
- SQL Server LocalDB or SQL Server instance
- Visual Studio 2022 or VS Code
- EF Core CLI tools

## Configuration

The database connection string is stored in `appsettings.json` and can be adjusted for your local SQL Server environment.

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=ProductHubDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

Use `appsettings.Development.json` for local development values and keep production values out of source control.

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

## API Endpoints

- GET /api/products
- GET /api/products/{id}
- POST /api/products
- PUT /api/products/{id}
- DELETE /api/products/{id}

## 🛠️ Local Development

```bash
dotnet restore
dotnet build
dotnet test
dotnet run
```

Then open Swagger at https://localhost:5001/swagger or http://localhost:5000/swagger depending on your configuration.

## Architecture

Controller -> Service -> Repository -> EF Core -> SQL Server
