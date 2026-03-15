# Coding Standards

## File Header Comment

Every `.cs` file in the AbsenceApp solution **must** begin with the following block-header comment. This is enforced by the Roslyn analyzer (`AA0001`) and the Git pre-commit hook.

```csharp
/*
===============================================================================
 File        : FileName.cs
 Namespace   : AbsenceApp.<Project>.<SubNamespace>
 Author      : <Author Name>
 Version     : 1.0.0
 Created     : YYYY-MM-DD
 Updated     : YYYY-MM-DD
-------------------------------------------------------------------------------
 Purpose     : <One-line summary of what this file does>
-------------------------------------------------------------------------------
 Description :
   <Optional multi-line detail.>
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  YYYY-MM-DD  Initial creation.
===============================================================================
*/
```

### Required Labels

The following labels must be present (with a colon):

`File`, `Namespace`, `Author`, `Version`, `Created`, `Updated`, `Purpose`, `Changes`

### Versioning

Use semantic versioning: `MAJOR.MINOR.PATCH`.

| Increment | When |
|---|---|
| PATCH | Bug fixes, minor tweaks |
| MINOR | New methods/properties without breaking changes |
| MAJOR | Breaking API changes or significant redesigns |

## Naming Conventions

| Item | Convention | Example |
|---|---|---|
| Classes | PascalCase | `ClassService` |
| Interfaces | `I` + PascalCase | `IClassService` |
| Methods | PascalCase | `GetAllAsync` |
| Properties | PascalCase | `ClassName` |
| Fields (private) | `_camelCase` | `_classRepository` |
| Parameters | camelCase | `connectionString` |
| Local variables | camelCase | `classDto` |
| Constants | PascalCase or UPPER_SNAKE | `DiagnosticId` |
| Async methods | Suffix `Async` | `CreateAsync` |

## Code Organisation

Every file should use **section comments** to group related members:

```csharp
// =========================================================================
// Constructor
// =========================================================================

// =========================================================================
// Public methods
// =========================================================================

// =========================================================================
// Private helpers
// =========================================================================
```

## Architecture Rules

1. **Core has no upward references** — `AbsenceApp.Core` must not reference `AbsenceApp.Data`, `AbsenceApp.Client`, or any EF Core packages.
2. **Services return DTOs**, not EF entities. Mappers convert at the boundary.
3. **Repositories are injected via interfaces** — never reference a concrete repository class directly outside of DI registration.
4. **Mappers are static** — no state, no DI, no side-effects.
5. **Validation belongs in services** — `ArgumentException` is thrown for blank required strings.

## Razor Pages

- Every Razor page with a route parameter (`/page/{id}`) must have a corresponding `[Parameter] public string id { get; set; } = default!;` in an `@code` block.
- Navigation links should use strongly typed route patterns, not magic strings where possible.

## Testing

- All tests use **xUnit** with `[Fact]` and `[Theory]`.
- Tests are arranged in **Arrange / Act / Assert** sections with blank-line separation.
- Repository tests use EF Core InMemory; service tests mock repositories with **Moq**.
- Async LINQ tests (`ToListAsync`, etc.) require the `AsyncQueryHelper.AsAsyncQueryable()` wrapper.
- Test class names follow the pattern `<Target>Tests` (e.g. `ClassServiceTests`).
- Test method names follow `MethodName_Scenario_ExpectedResult` or plain descriptive names.

## Commits

The pre-commit hook blocks commits where staged `.cs` files are missing the required block-header. Always ensure headers are up to date before committing.
