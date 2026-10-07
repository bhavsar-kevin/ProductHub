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

Phase 4 — Pull Request CI 🚧 NEXT

```text
Phase 1  ████████████████████ 100%
Phase 2  ████████████████████ 100%
Phase 3  ████████████████████ 100%
Phase 4  ░░░░░░░░░░░░░░░░░░░░ 0%
Phase 5+ Planned
```

Phase 3 is complete: GitHub Actions automatically restores, builds, and tests ProductHub on pushes and pull requests. The next milestone is Phase 4, making passing PR checks an enforced merge requirement.

## 🗺️ CI/CD Learning Roadmap

| Phase | Topic | Status |
|------|------|------|
| 1 | ASP.NET Core Application | ✅ Completed |
| 2 | Git & GitHub | ✅ Completed |
| 3 | First GitHub Actions CI Pipeline | ✅ Completed |
| 4 | Pull Request CI | 🚧 Next |
| 5 | Test Coverage & Code Quality | ⏳ Planned |
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

# Phase 3 — First GitHub Actions CI Pipeline

Status:

✅ COMPLETED

Created `.github/workflows/ci.yml` to run the **ProductHub CI** workflow automatically on branch pushes and pull requests targeting `main`.

The `build-and-test` job runs on an Ubuntu GitHub-hosted runner and performs:

1. Checkout repository (`actions/checkout@v4`)
2. Set up .NET 8 (`actions/setup-dotnet@v4`)
3. Restore dependencies (`dotnet restore`)
4. Build in Release mode (`dotnet build --no-restore --configuration Release`)
5. Run tests (`dotnet test --no-build --configuration Release`)

### Practical CI failure and recovery exercises

- **Build failure:** Introduced a deliberate C# syntax error. The build step failed and the test step was skipped.
- **Test failure:** Introduced a failing unit test. The build succeeded, but the test step failed.
- **Recovery:** Fixed both issues, pushed the changes, and observed successful CI checks for both `push` and `pull_request` events.

The exercises demonstrated that CI provides fast feedback and reports unsuccessful builds or tests before merging. **A green CI check alone does not prevent merging a later failing PR** until required status checks are configured in Phase 4.

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

### Phase 3

- Structure of GitHub Actions workflows, triggers, jobs, runners, and steps
- Difference between `uses` actions and `run` commands
- Automatic CI on branch pushes and pull requests
- Diagnosing compilation errors in GitHub Actions logs
- Understanding failed tests and skipped steps
- Re-running CI automatically after pushing a fix
- Interpreting successful and failed PR checks

# 🔄 Current Development Workflow

```text
Developer
    ↓
Feature Branch
    ↓
Code Changes + Local Tests
    ↓
Commit + Push
    ↓
GitHub Actions CI (Restore → Build → Test)
    ↓
Pull Request → CI Checks
    ↓
Review → Merge into main
```

CI currently reports failures and successes automatically. Enforcing successful checks as a condition for merging is the focus of Phase 4.

# 🚦 Current CI/CD State

| Capability | Status |
|---|---|
| Source Control | ✅ Git + GitHub |
| Application Build | ✅ Local + CI |
| Unit Tests | ✅ Local + CI |
| Pull Requests | ✅ Working |
| Automated CI | ✅ GitHub Actions |
| Required PR status checks | ⏳ Phase 4 |
| Docker | ⏳ Planned |
| Container Registry | ⏳ Planned |
| Continuous Deployment | ⏳ Planned |
| Cloud Deployment | ⏳ Planned |
| Production Pipeline | ⏳ Planned |

# 🚀 Next Phase — Pull Request CI

Phase 4 will strengthen the PR workflow by requiring successful CI checks before merging into `main`. The focus will be on branch protection/rulesets, required status checks, and verifying that a failing PR cannot be merged under the configured rules.

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
├── .github/
│   └── workflows/
│       └── ci.yml
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
