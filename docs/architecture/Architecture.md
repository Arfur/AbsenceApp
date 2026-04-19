# Architecture

## Overview

AbsenceApp is a cross-platform school attendance and absence management system built with .NET 8. It follows a clean, layered architecture with strict separation of concerns between the domain, data, client, and infrastructure layers.

```
┌─────────────────────────────────────────────────────────────┐
│                     AbsenceApp.Client                        │
│     (.NET MAUI Blazor Hybrid — iOS, Android, Windows, Mac)  │
│                                                              │
│  Pages (Razor)  ─►  ViewModels  ─►  Service Interfaces      │
└────────────────────────────┬────────────────────────────────┘
                             │ IStudentService / IAbsenceService
                             │ IClassService / IRoleService
                             │ IAuditLogService
                             ▼
┌─────────────────────────────────────────────────────────────┐
│                      AbsenceApp.Core                         │
│     Domain models, DTOs, service/repository interfaces       │
│                                                              │
│  Models: Student, AbsenceRecord                             │
│  DTOs:   UserDto, ClassDto, RoleDto, AttendanceDto,         │
│          AuditLogDto                                         │
│  Interfaces: IAbsenceService, IStudentService,              │
│              IClassService, IRoleService, IAuditLogService   │
└────────────────────────────┬────────────────────────────────┘
                             │ (referenced by Data + Client)
                             ▼
┌─────────────────────────────────────────────────────────────┐
│                      AbsenceApp.Data                         │
│     EF Core repositories, EF-backed services, mappers        │
│                                                              │
│  Context:       AppDbContext                                 │
│  Models:        User, Role, Class, Attendance, AuditLog,     │
│                 UserRole, ClassMember                        │
│  Repositories:  User/Role/Class/Attendance/AuditLog          │
│  Services:      ClassService, RoleService, AuditLogService  │
│  Mappers:       User, Role, Class, Attendance,               │
│                 AuditLog, Absence, Student                   │
│  Seeding:       DatabaseSeeder                              │
└────────────────────────────┬────────────────────────────────┘
                             │
        ┌────────────────────┼────────────────────┐
        ▼                    ▼                     ▼
┌───────────────┐   ┌────────────────┐   ┌────────────────────┐
│ AbsenceApp    │   │  AbsenceApp    │   │  AbsenceApp        │
│ .EfHost       │   │  .Api          │   │  .Tests            │
│               │   │                │   │                    │
│ EF migrations │   │ Minimal API    │   │ xUnit test project │
│ + Seeder run  │   │ (REST/Swagger) │   │ for all layers     │
└───────────────┘   └────────────────┘   └────────────────────┘
```

## Project Descriptions

| Project | SDK | Purpose |
|---|---|---|
| `AbsenceApp.Core` | `Microsoft.NET.Sdk` | Domain models, DTOs, service and repository interfaces. No external dependencies. |
| `AbsenceApp.Data` | `Microsoft.NET.Sdk` | EF Core entities, DbContext, repositories, services, mappers, seeder. |
| `AbsenceApp.Client` | `Microsoft.NET.Sdk.Razor` | .NET MAUI Blazor Hybrid UI (Razor pages + ViewModels). |
| `AbsenceApp.EfHost` | `Microsoft.NET.Sdk` | Minimal console app used as EF Core startup project for migrations. |
| `AbsenceApp.Api` | `Microsoft.NET.Sdk.Web` | ASP.NET Core Minimal API exposing REST endpoints. |
| `AbsenceApp.Analyzers` | `Microsoft.NET.Sdk` | Roslyn analyzer enforcing block-header comment convention. |
| `AbsenceApp.Updater` | `Microsoft.NET.Sdk` | Application update checking and download logic. |
| `AbsenceApp.Tests` | `Microsoft.NET.Sdk` | xUnit test project covering repositories, services, and mappers. |

## Design Principles

- **Core has no upward references** — `AbsenceApp.Core` does not reference `AbsenceApp.Data` or any infrastructure package.
- **DTOs at the boundary** — repositories and services return DTOs, not EF entities, to the client.
- **Mappers are static** — all mapper classes use pure static methods; no DI required.
- **Repository pattern** — `IRepository<T>` generic base plus per-entity interfaces keep data access testable.
- **DI registered centrally** — `DataServiceRegistration.AddDataLayer()` wires up the entire data layer in one call.
