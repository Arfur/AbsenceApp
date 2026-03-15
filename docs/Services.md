# Services

## Overview

Service classes implement business logic on top of the repository layer. All services implement interfaces defined in `AbsenceApp.Core/Interfaces/` and are registered via `DataServiceRegistration.AddDataLayer()` or `MauiProgram`.

## Service Inventory

| Interface | Implementation | Project | Notes |
|---|---|---|---|
| `IClassService` | `ClassService` | Data | Full async CRUD |
| `IRoleService` | `RoleService` | Data | Full async CRUD |
| `IAuditLogService` | `AuditLogService` | Data | Read + append-only write |
| `IStudentService` | `StudentService` | Core | Maps `User` → `Student` |
| `IAbsenceService` | `AbsenceService` | Core | In-memory store backed by `AbsenceRepository` |

## ClassService

**Location:** `src/AbsenceApp.Data/Services/ClassService.cs`

| Method | Description |
|---|---|
| `GetAllAsync()` | Returns all classes as `IEnumerable<ClassDto>`. |
| `GetByIdAsync(int id)` | Returns the class with the given ID as `ClassDto`, or `null`. |
| `CreateAsync(ClassDto)` | Validates `ClassName` (throws `ArgumentException` if blank). Resets `ClassId` to 0 for DB generation. Returns the persisted DTO. |
| `UpdateAsync(ClassDto)` | Validates `ClassName`. Fetches the entity, applies changes, saves. |
| `DeleteAsync(int id)` | Deletes the class by ID via the repository. |

## RoleService

**Location:** `src/AbsenceApp.Data/Services/RoleService.cs`

| Method | Description |
|---|---|
| `GetAllAsync()` | Returns all roles as `IEnumerable<RoleDto>`. |
| `GetByIdAsync(int id)` | Returns the role with the given ID, or `null`. |
| `CreateAsync(RoleDto)` | Validates `RoleName`. Resets `RoleId` to 0. Returns the persisted DTO. |
| `UpdateAsync(RoleDto)` | Validates `RoleName`. Fetches, applies, saves. |
| `DeleteAsync(int id)` | Deletes the role by ID. |

## AuditLogService

**Location:** `src/AbsenceApp.Data/Services/AuditLogService.cs`

| Method | Description |
|---|---|
| `GetAllAsync()` | Returns all audit log entries as `IEnumerable<AuditLogDto>`. |
| `GetByIdAsync(int id)` | Returns the entry with the given ID, or `null`. |
| `GetByUserAsync(int userId)` | Filters entries by `UserId` — WHERE clause pushed to SQL. |
| `LogAsync(int userId, string action)` | Validates `action` (throws if blank). Sets `Timestamp = DateTime.UtcNow`. Returns the persisted DTO. |

## StudentService

**Location:** `src/AbsenceApp.Core/Services/StudentService.cs`

Uses `IStudentRepository` which is backed by `StudentRepository` (Data layer). Maps `User` EF entities to `Student` domain models via `StudentMapper.ToDomain`.

| Method | Description |
|---|---|
| `GetAllStudentsAsync()` | Returns all users as `IEnumerable<Student>`. |
| `GetStudentByIdAsync(string id)` | Returns the student with the given string ID, or `null`. |

## AbsenceService

**Location:** `src/AbsenceApp.Core/Services/AbsenceService.cs`

Uses an in-memory `List<AbsenceRecord>` backed by `AbsenceRepository` (static store). Maps to/from EF `Attendance` entities via `AbsenceMapper`.

| Method | Description |
|---|---|
| `GetAbsencesForStudentAsync(string studentId)` | Returns all absences for the given student ID. |
| `AddAbsenceAsync(AbsenceRecord)` | Adds a new absence record to the store. |

## Error Handling

- `ArgumentException` is thrown by `ClassService` and `RoleService` when a required string field is null or whitespace.
- `AuditLogService.LogAsync` throws `ArgumentException` when `action` is blank.
- Repository methods may throw `InvalidOperationException` (from EF `FirstAsync`) if an expected entity is not found.
