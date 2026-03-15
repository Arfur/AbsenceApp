# Migration History

## Overview

Database migrations are managed by EF Core's Code-First approach. The `AbsenceApp.EfHost` project is the designated startup project for migration commands.

## Running Migrations

### Prerequisites

```powershell
# Install EF Core tools globally (once)
dotnet tool install --global dotnet-ef

# Or update if already installed
dotnet tool update --global dotnet-ef
```

### Add a Migration

```powershell
cd C:\DevAbsence1
dotnet ef migrations add <MigrationName> `
    --project src/AbsenceApp.Data `
    --startup-project src/AbsenceApp.EfHost
```

### Apply Migrations to the Database

```powershell
dotnet ef database update `
    --project src/AbsenceApp.Data `
    --startup-project src/AbsenceApp.EfHost
```

### Revert the Last Migration

```powershell
# Revert to the previous migration in the database
dotnet ef database update <PreviousMigrationName> `
    --project src/AbsenceApp.Data `
    --startup-project src/AbsenceApp.EfHost

# Then remove the migration files
dotnet ef migrations remove `
    --project src/AbsenceApp.Data `
    --startup-project src/AbsenceApp.EfHost
```

## Migration Log

| # | Name | Date | Description |
|---|---|---|---|
| 1 | `InitialCreate` | 2026-03-13 | Initial schema: Users, Roles, UserRoles, Classes, ClassMembers, Attendance, AuditLogs |

> Add rows here whenever a new migration is added to the project.

## Connection String Configuration

The EfHost uses the connection string in `src/AbsenceApp.EfHost/appsettings.json` (or set via environment variable `ConnectionStrings__Default`).

Default local development value:

```json
{
  "ConnectionStrings": {
    "Default": "Server=(localdb)\\MSSQLLocalDB;Database=AbsenceAppDev;Trusted_Connection=True;"
  }
}
```

## Seeder

After applying migrations, run the EfHost to seed default data:

```powershell
dotnet run --project src/AbsenceApp.EfHost
```

This calls `DatabaseSeeder.SeedAsync()` which idempotently inserts:

- Default roles: Admin, Teacher, Student
- Admin user: `admin / admin@absenceapp.local`
- Admin role assignment
- Sample class: Year 7A
- Sample attendance record
- Initial audit log entry
