# DTOs and Mappers

## Overview

Data Transfer Objects (DTOs) are plain C# record-like classes defined in `AbsenceApp.Core/DTOs/`. They cross layer boundaries so that EF Core entities never leak into the client or API layers.

Mappers are static classes in `AbsenceApp.Data/Mappers/` that convert between EF entities and DTOs (or domain models).

## DTOs

| Class | Namespace | Properties |
|---|---|---|
| `UserDto` | `AbsenceApp.Core.DTOs` | `UserId`, `Username`, `FirstName`, `LastName`, `FullName` (computed), `Email?`, `IsActive`, `CreatedAt` |
| `RoleDto` | `AbsenceApp.Core.DTOs` | `RoleId`, `RoleName` |
| `ClassDto` | `AbsenceApp.Core.DTOs` | `ClassId`, `ClassName`, `Description?` |
| `AttendanceDto` | `AbsenceApp.Core.DTOs` | `AttendanceId`, `UserId`, `ClassId?`, `Status`, `Timestamp`, `RecordedBy` |
| `AuditLogDto` | `AbsenceApp.Core.DTOs` | `AuditId`, `UserId`, `Action`, `Timestamp` |

## Domain Models (in Core, not DTOs)

| Class | Namespace | Properties |
|---|---|---|
| `Student` | `AbsenceApp.Core.Models` | `Id`, `FirstName`, `LastName`, `YearGroup` |
| `AbsenceRecord` | `AbsenceApp.Core.Models` | `Id`, `StudentId`, `Date`, `Reason` |

## Mappers

### UserMapper

| Method | From | To | Notes |
|---|---|---|---|
| `ToDto(User)` | EF `User` | `UserDto` | `FullName` is computed by the DTO property getter |
| `ToEntity(UserDto)` | `UserDto` | EF `User` | — |

### RoleMapper

| Method | From | To |
|---|---|---|
| `ToDto(Role)` | EF `Role` | `RoleDto` |
| `ToEntity(RoleDto)` | `RoleDto` | EF `Role` |

### ClassMapper

| Method | From | To | Notes |
|---|---|---|---|
| `ToDto(Class)` | EF `Class` | `ClassDto` | `Description` is nullable; passes through as-is |
| `ToEntity(ClassDto)` | `ClassDto` | EF `Class` | — |

### AttendanceMapper

| Method | From | To | Notes |
|---|---|---|---|
| `ToDto(Attendance)` | EF `Attendance` | `AttendanceDto` | `ClassId` nullable preserved |
| `ToEntity(AttendanceDto)` | `AttendanceDto` | EF `Attendance` | — |

### AuditLogMapper

| Method | From | To |
|---|---|---|
| `ToDto(AuditLog)` | EF `AuditLog` | `AuditLogDto` |
| `ToEntity(AuditLogDto)` | `AuditLogDto` | EF `AuditLog` |

### AbsenceMapper

Bridges the legacy `AbsenceRecord` domain model and the `Attendance` EF entity.

| Method | From | To | Notes |
|---|---|---|---|
| `ToDomain(Attendance)` | EF `Attendance` | `AbsenceRecord` | `AttendanceId` → `Id` (string); `Timestamp.Date` → `Date` |
| `ToEntity(AbsenceRecord, int recordedBy)` | `AbsenceRecord` | EF `Attendance` | `StudentId` parsed to int; defaults to 0 on parse failure |

### StudentMapper

Bridges the `Student` domain model and the `User` EF entity.

| Method | From | To | Notes |
|---|---|---|---|
| `ToDomain(User)` | EF `User` | `Student` | `YearGroup` always set to `string.Empty` — caller must enrich |
| `ToEntity(Student)` | `Student` | EF `User` | `Username` derived as `firstname.lastname` (lowercase); `IsActive = true` |

## Rules

- All mappers are **pure static** — no side-effects, no DI.
- Mappers never set PK values on `ToEntity` — those are DB-generated.
- `FullName` on `UserDto` is a **computed property** and is not persisted.
- `YearGroup` on `Student` has **no database column** and must be enriched by the caller if a non-empty value is needed.
