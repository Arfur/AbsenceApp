# Repositories

## Overview

The repository layer provides a typed abstraction over EF Core `DbSet<T>` operations. All repositories implement `IRepository<T>` (generic CRUD) plus an entity-specific interface adding any custom query methods.

## Generic Interface

```csharp
// AbsenceApp.Core/Interfaces/IRepository.cs
public interface IRepository<T> where T : class
{
    Task AddAsync(T entity);
    Task<T?> FindByIdAsync(int id);
    Task<IEnumerable<T>> ListAllAsync();
    IQueryable<T> Query();
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
}
```

## Repository Inventory

| Interface | Implementation | Entity | Extra Methods |
|---|---|---|---|
| `IUserRepository` | `UserRepository` | `User` | — |
| `IRoleRepository` | `RoleRepository` | `Role` | — |
| `IClassRepository` | `ClassRepository` | `Class` | — |
| `IAttendanceRepository` | `AttendanceRepository` | `Attendance` | — |
| `IAuditLogRepository` | `AuditLogRepository` | `AuditLog` | `GetByUserIdAsync(int userId)` |
| `IAbsenceRepository` | `AbsenceRepository` | *(in-memory)* | `GetAllAsync(string studentId)`, `AddAsync(AbsenceRecord)` |
| `IStudentRepository` | `StudentRepository` | `User` | `GetByIdAsync(string id)` |

## EF Core Repositories

All EF repositories (`UserRepository` through `AuditLogRepository`) follow the same pattern:

```
ctor(AppDbContext) → stores _context
AddAsync     → _context.Set<T>().Add(entity); SaveChangesAsync()
FindByIdAsync → _context.Set<T>().FindAsync(id)
ListAllAsync  → _context.Set<T>().ToListAsync()
Query        → _context.Set<T>().AsQueryable()
UpdateAsync   → _context.Entry(entity).State = Modified; SaveChangesAsync()
DeleteAsync   → _context.Set<T>().Remove(entity); SaveChangesAsync()
```

## Non-EF Repositories

### AbsenceRepository

**Location:** `src/AbsenceApp.Core/Repositories/AbsenceRepository.cs`

Uses a `static readonly List<AbsenceRecord>` as an in-memory store. Exists to bridge the legacy domain model with the EF pipeline during the migration period.

> **Note:** The static backing store means records persist for the lifetime of the process. Tests should use unique `StudentId` values to avoid cross-test interference.

### StudentRepository

**Location:** `src/AbsenceApp.Data/Repositories/StudentRepository.cs`

Wraps `AppDbContext.Users` and maps `User` entities to `Student` domain models using `StudentMapper.ToDomain`.

## DI Registration

All repositories are registered in `DataServiceRegistration.AddDataLayer()` as **Scoped** services (EF Core DbContext is Scoped by default). `AbsenceRepository` and `StudentRepository` are registered in `MauiProgram.cs` as **singletons**.
