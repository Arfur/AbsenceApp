# Data Model

## Overview

The AbsenceApp SQL Server database is managed by EF Core using the Code-First approach. All entity configurations live in `AbsenceApp.Data/Configurations/` as individual `IEntityTypeConfiguration<T>` classes applied via `ApplyConfigurationsFromAssembly`.

## Entity Relationship Diagram

```
Users ──────── UserRoles ──────── Roles
  │               (M:M join)
  │
  ├── ClassMembers ──── Classes
  │     (M:M join)         │
  │                        └── Attendance
  │                                │
  │                     (RecordedBy FK)
  │                                │
  └── Attendance ─────────────────┘
  │
  └── AuditLogs
```

## Table Definitions

### Users

| Column | Type | Constraints |
|---|---|---|
| UserId | int | PK, identity |
| Username | nvarchar(100) | NOT NULL, UNIQUE |
| FirstName | nvarchar(100) | NOT NULL |
| LastName | nvarchar(100) | NOT NULL |
| Email | nvarchar(200) | nullable |
| IsActive | bit | NOT NULL, default 1 |
| CreatedAt | datetime2 | NOT NULL |

### Roles

| Column | Type | Constraints |
|---|---|---|
| RoleId | int | PK, identity |
| RoleName | nvarchar(100) | NOT NULL, UNIQUE |

### UserRoles (join table)

| Column | Type | Constraints |
|---|---|---|
| UserId | int | PK (composite), FK → Users |
| RoleId | int | PK (composite), FK → Roles |

Delete behaviour: `Restrict` on both FK sides.

### Classes

| Column | Type | Constraints |
|---|---|---|
| ClassId | int | PK, identity |
| ClassName | nvarchar(200) | NOT NULL |
| Description | nvarchar(500) | nullable |

### ClassMembers (join table)

| Column | Type | Constraints |
|---|---|---|
| UserId | int | PK (composite), FK → Users |
| ClassId | int | PK (composite), FK → Classes |

Delete behaviour: `Restrict` on both FK sides.

### Attendance

| Column | Type | Constraints |
|---|---|---|
| AttendanceId | int | PK, identity |
| UserId | int | FK → Users |
| ClassId | int? | FK → Classes, nullable |
| Status | nvarchar(50) | NOT NULL (e.g. "Present", "Absent", "Late") |
| Timestamp | datetime2 | NOT NULL |
| RecordedBy | int | FK → Users (recorder) |

Delete behaviour: `Restrict` on UserId and RecordedBy FKs.

### AuditLogs

| Column | Type | Constraints |
|---|---|---|
| AuditId | int | PK, identity |
| UserId | int | FK → Users |
| Action | nvarchar(500) | NOT NULL |
| Timestamp | datetime2 | NOT NULL |

Delete behaviour: `Restrict` on UserId FK.

## Notes

- `DeleteBehavior.Restrict` is used throughout to prevent accidental cascade deletes. SQL Server enforces this at the database level.
- The EF InMemory provider used in tests does **not** enforce `Restrict`. Integration tests against a real database are required to verify constraint behaviour.
- Audit logs are append-only; no `UPDATE` or `DELETE` is exposed via the service layer.
