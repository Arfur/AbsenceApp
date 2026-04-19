# Developer Setup

## Prerequisites

| Tool | Version | Notes |
|---|---|---|
| .NET SDK | 8.0+ | [Download](https://dotnet.microsoft.com/download) |
| Visual Studio 2022 | 17.8+ | Or VS Code with C# Dev Kit |
| SQL Server LocalDB | Any | Included with VS; used for local DB |
| Android SDK | API 21+ | Required for Android target (optional) |
| Xcode | 15+ | Required for iOS/macOS targets (Mac only) |
| EF Core Tools | Latest | `dotnet tool install --global dotnet-ef` |
| Git | Any | `git config core.hooksPath .githooks` after clone |

## Clone and Restore

```powershell
git clone <repo-url> C:\DevAbsence1
cd C:\DevAbsence1
dotnet restore AbsenceApp.sln
```

## Configure Git Hooks

```powershell
git config core.hooksPath .githooks
```

The pre-commit hook will block commits where staged `.cs` files are missing the required block-header comment.

## Build

```powershell
# Build all projects
dotnet build AbsenceApp.sln --configuration Debug

# Build a specific project
dotnet build src/AbsenceApp.Data/AbsenceApp.Data.csproj
```

## Run Tests

```powershell
dotnet test src/AbsenceApp.Tests/AbsenceApp.Tests.csproj --verbosity normal
```

For code coverage:

```powershell
dotnet test src/AbsenceApp.Tests/AbsenceApp.Tests.csproj `
    --collect:"XPlat Code Coverage" `
    --results-directory ./TestResults
```

## Apply Database Migrations

```powershell
dotnet ef database update `
    --project src/AbsenceApp.Data `
    --startup-project src/AbsenceApp.EfHost
```

## Seed Development Data

```powershell
dotnet run --project src/AbsenceApp.EfHost
```

## Run the API

```powershell
dotnet run --project src/AbsenceApp.Api
# Swagger UI: https://localhost:<port>/swagger
```

## Run the MAUI Client

Open `AbsenceApp.sln` in Visual Studio 2022 and select the desired target platform (Windows, Android, iOS, macOS) from the run toolbar.

Or from the command line (Windows target only):

```powershell
dotnet run --project src/AbsenceApp.Client --framework net8.0-windows10.0.19041.0
```

## Project Structure Quick Reference

```
AbsenceApp.sln
src/
  AbsenceApp.Core/        Domain models, DTOs, interfaces
  AbsenceApp.Data/        EF Core, repositories, services, mappers
  AbsenceApp.Client/      MAUI Blazor Hybrid UI
  AbsenceApp.EfHost/      EF migration startup + seeder runner
  AbsenceApp.Api/         Minimal API (REST + Swagger)
  AbsenceApp.Analyzers/   Roslyn header-comment analyzer
  AbsenceApp.Updater/     Update checker and downloader
  AbsenceApp.Tests/       xUnit tests
docs/                     Project documentation
.githooks/                Git hooks (pre-commit header check)
```
