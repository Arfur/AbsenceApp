# AbsenceAPP V2 — Architecture Overview

**Version:** 2.0.0 · **Generated:** 2026-05-09  
**Scope:** AbsenceAppV2 only (V1 is excluded).  
**Evidence provenance:** Live MariaDB introspection + AppDbContext + EF migrations + PRD Table Structures.md.

---

## Table of Contents

1. [Solution Architecture](#1-solution-architecture)
2. [Runtime Data Flow](#2-runtime-data-flow)
3. [Access Control Model](#3-access-control-model)
4. [Subsystem 1 — Menu System](#4-subsystem-1--menu-system)
5. [Subsystem 2 — Navigation System](#5-subsystem-2--navigation-system)
6. [Subsystem 3 — Page Registry](#6-subsystem-3--page-registry)
7. [Subsystem 4 — Permissions System](#7-subsystem-4--permissions-system)
8. [Subsystem 5 — Feature Flags](#8-subsystem-5--feature-flags)
9. [Subsystem 6 — User Management](#9-subsystem-6--user-management)
10. [Subsystem 7 — Student Absence Management](#10-subsystem-7--student-absence-management)
11. [Subsystem 8 — Student Profile](#11-subsystem-8--student-profile)
12. [Subsystem 9 — Staff Management](#12-subsystem-9--staff-management)
13. [Subsystem 10 — Messaging & Notifications](#13-subsystem-10--messaging--notifications)
14. [Subsystem 11 — Audit Logging](#14-subsystem-11--audit-logging)
15. [Subsystem 12 — Lookup Tables](#15-subsystem-12--lookup-tables)
16. [Subsystem 13 — Future Staff Profile Tables](#16-subsystem-13--future-staff-profile-tables)
17. [DTO Reference](#17-dto-reference)
18. [Service Reference](#18-service-reference)
19. [ViewModel & Page Reference](#19-viewmodel--page-reference)
20. [End-to-End Examples](#20-end-to-end-examples)
21. [Evidence Provenance](#21-evidence-provenance)

---

## 1. Solution Architecture

### 1.1 Projects

| Project | Type | Role |
|---|---|---|
| `AbsenceApp.Client` | .NET MAUI Blazor Hybrid (net8.0) | Main application shell; Razor UI, ViewModels, client API services |
| `AbsenceApp.Core` | Class library | Interfaces, DTOs, shared models |
| `AbsenceApp.Data` | Class library | EF Core; AppDbContext, repositories, data services, migrations |
| `AbsenceApp.EfHost` | Class library | Design-time EF host (`AppDbContextFactory`) |
| `AbsenceApp.Api` | ASP.NET Core Minimal API | Optional REST + Swagger surface (not used by MAUI at runtime) |
| `AbsenceApp.Tests` | xUnit | Repository, service, mapper unit tests |
| `AbsenceApp.Client.Tests` | xUnit | Client-side unit tests |
| `AbsenceApp.Analyzers` | Roslyn stub | Static analysis hooks |
| `AbsenceApp.Updater` | Stub | Auto-update logic |

### 1.2 Client Startup Flow

```
MauiProgram.CreateMauiApp()
  └─ AddDataLayer()                    — registers AppDbContext (Scoped), repositories, data services
  └─ AddAbsenceAppV2Framework()        — registers client services, ViewModels, singletons
  └─ App.xaml.cs (WinUI)              — WindowsTrayIcon, system-tray behaviour
       └─ MainPage.xaml               — BlazorWebView
            └─ AppHost.razor          — root component → <Routes />
                 └─ Routes.razor      — DefaultLayout = BaseLayoutV2
                      └─ BaseLayoutV2 — shell: SidebarV2 | HeaderV2 | content
```

### 1.3 Layout Architecture

```
BaseLayoutV2.razor          — LayoutComponentBase; root layout, applies .dark CSS
  └─ SidebarV2.razor        — role-driven menu rendered from NavigationApiServiceV2
  └─ HeaderV2.razor         — logo, sidebar toggle, dark-mode, notifications bell
  └─ BreadcrumbV2.razor     — route-aware breadcrumb from NavigationServiceV2
  └─ PageTemplateV2.razor   — sub-shell with content slots
       └─ [Module pages]    — Razor pages inside Modules/**
```

### 1.4 Layer Diagram

```
┌─────────────────────────────────────────────┐
│  AbsenceApp.Client (MAUI Blazor Hybrid)      │
│  ┌──────────┐  ┌──────────┐  ┌───────────┐  │
│  │ Razor    │  │ViewModel │  │ ApiV2     │  │
│  │ Module   │◄─│  (V2)    │◄─│ Service   │  │
│  │ Pages    │  │          │  │ (Module)  │  │
│  └──────────┘  └──────────┘  └─────┬─────┘  │
└────────────────────────────────────│─────────┘
                                     │ IServiceScopeFactory
                                     ▼ (direct DB — no HTTP in MAUI)
┌─────────────────────────────────────────────┐
│  AbsenceApp.Data                             │
│  ┌───────────────┐  ┌─────────────────────┐ │
│  │ Data Services │◄─│ Repositories        │ │
│  │ (IXxxService) │  │ (IXxxRepository)    │ │
│  └───────┬───────┘  └──────────┬──────────┘ │
│          └──────────────────────┘            │
│                     ▼                        │
│           AppDbContext (EF Core)             │
└─────────────────────────────────────────────┘
                     ▼
              MariaDB (absenceapp)
```

> **MAUI Blazor Hybrid constraint:** In the .NET MAUI C# context, `HttpClient` cannot reach `http://localhost/` (that address exists only inside the WebView2 browser context). Every client API service therefore resolves the data layer directly via `IServiceScopeFactory`, not via HTTP.

### 1.5 CSS Architecture

| Layer | Location | Convention |
|---|---|---|
| V1 design system | `wwwroot/css/app.css` | CSS vars `--chrome-bg`, `--main-bg`, `--text`, `--accent` |
| V2 design tokens | `wwwroot/css/tokens/` | Prefixed `--ds-*`; reference V1 vars via `var(--main-bg, fallback)` |
| Component scoping | `*.razor.css` | Scoped styles per component |
| Framework classes | `.razor` files | `dsv2-*` (framework), `tpt-*` (TablePageTemplate) |
| Dark mode | `BaseLayoutV2` | `.dark` class on root `v2-layout-root` |

---

## 2. Runtime Data Flow

### 2.1 General Request Flow

```
User interaction (Razor component event)
  └─ ViewModel method called
       └─ ApiV2 Module Service method called
            └─ IServiceScopeFactory.CreateScope()
                 └─ Resolve Data Service (Scoped) or Repository
                      └─ Data Service queries AppDbContext
                           └─ EF Core generates SQL
                                └─ MariaDB returns rows
                           └─ Data Service maps Entity → DTO
                      └─ DTO returned up the call chain
                 └─ Scope disposed
            └─ ApiV2 Service returns DTO to ViewModel
       └─ ViewModel updates its properties
  └─ Razor component re-renders with new data (StateHasChanged)
```

### 2.2 Authentication Flow

```
LoginPageV2 → AuthService.LoginAsync(username, password)
  └─ Load User from users table by Username
  └─ VerifyPassword(): PBKDF2-SHA256 → BCrypt fallback → plain-text fallback
  └─ If valid:
       └─ Load role code via: userrole JOIN roles (raw SQL)
       └─ Load UserProfile (ProfilePictureUrl, DisplayName)
       └─ Update users.LastLoginAt, LoginCount
       └─ Write LoginAudit row (Success=true)
       └─ Return AuthResultDto { Success, UserName, RoleName, UserId, ... }
  └─ If invalid:
       └─ Write LoginAudit row (Success=false)
       └─ Return AuthResultDto { Success=false, ErrorMessage }
  └─ AppStateService.SetAuthenticated(result) — client singleton updated
  └─ Routes.razor redirects to /v2/dashboard
```

### 2.3 Navigation Build Flow

```
App launch / NavigationApiServiceV2.GetMenuCategoriesAsync()
  └─ Resolve CurrentUserId from AppStateService
  └─ SQL: SELECT menuitems.*
          FROM menuitems m
          INNER JOIN rolemenuitems rm ON rm.MenuItemId = m.Id
          INNER JOIN userrole ur ON ur.RoleId = rm.RoleId
          WHERE ur.UserId = @UserId AND rm.IsEnabled = 1 AND m.IsHidden = 0
  └─ BuildCategories(): 3-phase tree assembly
       Phase 1 — category rows
       Phase 2 — menu (group) rows, parented to categories
       Phase 3 — submenu (item) rows, parented to groups
       Phase 4 — single-submenu collapsing (accordion → flat link)
  └─ FilterByPermissionsAsync(): calls PermissionServiceV2.CanViewAsync()
       per submenu item and flat group — prunes hidden routes
  └─ Return List<MenuCategoryModel> to SidebarV2 for rendering
```

---

## 3. Access Control Model

AbsenceAppV2 has three orthogonal access-control layers:

| Layer | Tables | Controls |
|---|---|---|
| **Navigation** | `menuitems`, `rolemenuitems`, `userrole` | Which sidebar links are visible |
| **Page permissions** | `apppages`, `roledefaultpagepermissions`, `userpagepermissions`, `userpageoverrides` | CRUD flags (Read/Write/Create/Delete/Import/Export) per page per user |
| **Feature flags** | `features`, `rolefeature`, `userfeatureoverride` | Module-level on/off toggles |

### 3.1 Permission Resolution (Page Level)

```
PermissionServiceV2.LoadAsync(userId)
  1. Resolve roleCode via: userrole JOIN roles WHERE UserId = @UserId (raw SQL)
  2. Load all active apppages
  3. Load userpagepermissions WHERE UserId = @UserId
  4. Load roledefaultpagepermissions WHERE RoleTypeName = roleCode
  5. For each page:
     - If userpagepermissions row exists → use its CRUD flags (user override wins)
     - Else if roledefaultpagepermissions row exists → use role defaults
     - Else → all-false (deny)
  6. Cache result dictionary { route → EffectivePermissionDto }
```

### 3.2 Feature Flag Resolution

```
FeaturePermissionApiServiceV2.IsAllowedAsync(featureKey)
  1. Resolve RoleId via: userrole WHERE UserId = @UserId (raw SQL)
  2. Check features.IsEnabled for featureKey — return false if globally disabled
  3. Check rolefeature WHERE RoleId = @RoleId AND FeatureCode = featureKey AND IsEnabled = 1
  4. Return true if found and enabled; false otherwise
```

---

## 4. Subsystem 1 — Menu System

### Purpose
Controls which navigation items appear in the left-hand sidebar. Role-driven and data-driven.

### Live Tables (DB Verified)

| Table | PK | Key Columns |
|---|---|---|
| `menuitems` | `Id` int | `ParentId`, `ItemType` (category/menu/submenu), `Label`, `Icon`, `Route`, `SortOrder`, `IsHidden`, `Category`, `GroupName`, `GroupIcon`, `IsFlat`, `Status`, `Description` |
| `rolemenuitems` | `Id` int (auto_increment) | `RoleId` → `roles.Id`, `MenuItemId` → `menuitems.Id`, `IsEnabled`, `AssignedAt`, `AssignedBy` |
| `menuitemsglobalconfig` | `Id` | Mirrors `menuitems` shape; used exclusively for Global Settings sidebar |
| `roles` | `Id` int | `Name`, `Code`, `Description`, `IsSystemRole`, `IsDefault`, `Priority` |
| `userrole` | `Id` int | `UserId` → `users.Id`, `RoleId` → `roles.Id`, `AssignedAt`, `AssignedBy` |

### EF Mappings

| Entity class | DbSet | Table |
|---|---|---|
| `MenuItem` | `MenuItems` | `menuitems` |
| `RoleMenuItem` | `RoleMenuItem` | `rolemenuitems` |
| `MenuItemsGlobalConfig` | `MenuItemsGlobalConfig` | `menuitemglobalconfigs` |
| `RoleType` | `RoleTypes` | `roles` |
| `UserRole` | `UserRole` | `userrole` |

### Data Flow

```
users.Id
  └─ userrole (UserId) ──► userrole.RoleId
                                └─ rolemenuitems (RoleId) ──► rolemenuitems.MenuItemId
                                                                    └─ menuitems
```

### Relationships

```
roles ──► rolemenuitems ──► menuitems
users ──► userrole ──► roles ──► rolemenuitems ──► menuitems
```

---

## 5. Subsystem 2 — Navigation System

### Purpose
Dynamically resolves the sidebar structure for the current user. No hard-coded menu; everything comes from the DB.

### Live Tables
Same as Subsystem 1: `menuitems`, `rolemenuitems`, `userrole`, `roles`.

### Client Services

| Class | File | Purpose |
|---|---|---|
| `NavigationApiServiceV2` | `Services/ApiV2/Modules/NavigationApiServiceV2.cs` | Queries DB, builds `MenuCategoryModel` tree |
| `PermissionServiceV2` | `Services/PermissionServiceV2.cs` | Filters tree by per-user page permissions |

### Tree Model

```
MenuCategoryModel
  └─ Category: string
  └─ Groups: List<MenuGroupModel>
       └─ Group: string
       └─ Icon, Route, IsFlat, Description
       └─ Items: List<MenuItemModel>
            └─ Title, Icon, Route, Status, Description
```

### Menu Build Algorithm

```
Phase 1: Build category nodes from rows WHERE ItemType = 'category'
Phase 2: Build group nodes  from rows WHERE ItemType = 'menu', attach to parent category
Phase 3: Build item nodes   from rows WHERE ItemType = 'submenu', attach to parent group
Phase 4: Collapse single-item groups where item title == group label (→ flat link)
Phase 5: Prune groups with no items and no direct route
Phase 6: FilterByPermissionsAsync — remove items/groups user cannot CanRead
Phase 7: Remove empty categories
```

### Global Settings Navigation
Uses `menuitemsglobalconfig` directly (no role filter). Superadmin-only by design. Queried by `GetGlobalSettingsCategoriesAsync()`.

---

## 6. Subsystem 3 — Page Registry

### Purpose
Canonical registry of every permission-controlled page/route in the system.

### Live Table: `apppages`

| Column | Type | Notes |
|---|---|---|
| `Id` | int AUTO_INCREMENT PK | |
| `Name` | varchar(200) | Human-readable |
| `Slug` | varchar(200) UNIQUE | URL-safe identifier |
| `Route` | varchar(500) UNIQUE | Matches Blazor `@page` route |
| `CategoryKey` | varchar(100) | Must match `menuitems.Category` exactly (case-sensitive) |
| `MenuKey` | varchar(200) | Optional menu grouping |
| `IconKey` | varchar(100) NULL | Bootstrap icon key |
| `IsActive` | tinyint(1) DEFAULT 1 | |
| `SortOrder` | int DEFAULT 0 | |
| `SupportsRead/Write/Create/Delete/Import/Export` | tinyint(1) DEFAULT 0 | Capability flags |
| `CreatedAt`, `UpdatedAt` | datetime(6) | Auto-maintained |

### EF Mapping
`AppPage` entity → `apppages` via `UserManagementModelBuilderExtensions.ConfigureUserManagement()`.  
27 rows seeded (IDs 1–27) covering all V2 routes.

### Seeded Pages (summary)

| Id | Name | Route | CategoryKey |
|---|---|---|---|
| 1 | Dashboard | `/v2/dashboard` | MAIN SIDEBAR |
| 2 | Students | `/v2/students` | PEOPLE |
| 3 | Staff | `/v2/staff` | PEOPLE |
| 4 | Classes | `/v2/classes` | ACADEMICS |
| 5 | Attendance | `/v2/attendance` | ATTENDANCE |
| 6 | Audit Log | `/v2/auditlog` | CONFIGURATION |
| 7 | Settings | `/v2/settings` | SETTINGS |
| 8 | Users | `/v2/users` | CONFIGURATION |
| 9 | Pages | `/v2/pages` | CONFIGURATION |
| 10–12 | Dashboard sub-pages | `/v2/dashboard/overview`, `/trends`, `/safeguarding` | MAIN SIDEBAR |
| 13–14 | Settings sub-pages | `/v2/diagnostics`, `/v2/site` | SETTINGS |
| 15–16 | Student sub-pages | `/v2/students/details`, `/v2/students/new` | PEOPLE |
| 17–18 | Staff sub-pages | `/v2/staff/details`, `/v2/staff/new` | PEOPLE |
| 19–20 | Class sub-pages | `/v2/classes/details`, `/v2/classes/new` | ACADEMICS |
| 21–27 | System admin | `/v2/system/users` … `/v2/system/pages/metadata` | CONFIGURATION |

### Related Tables

| Table | EF Entity | Purpose |
|---|---|---|
| `roledefaultpagepermissions` | `RoleDefaultPagePermission` | Default CRUD per role per page |
| `userpagepermissions` | `UserPagePermission` | Per-user CRUD overrides |
| `userpageoverrides` | `UserPageOverride` | Grant/Deny override type (future use) |

### Services

| Class | Interface | Key operations |
|---|---|---|
| `PagesService` | `IPagesService` | GetAllAsync, GetByIdAsync, GetByRouteAsync, CreateAsync, UpdateAsync, DeleteAsync |

---

## 7. Subsystem 4 — Permissions System

### Purpose
Controls what a user can do on each page: Read, Write, Create, Delete, Import, Export.

### Live Tables

**`roledefaultpagepermissions`**

| Column | Type |
|---|---|
| `Id` | int AUTO_INCREMENT PK |
| `RoleTypeName` | varchar(100) — matches `roles.Code` (e.g. `super_admin`) |
| `PageId` | int → `apppages.Id` (CASCADE DELETE) |
| `CanRead/Write/Create/Delete/Import/Export` | tinyint(1) DEFAULT 0 |
| `CreatedAt` | datetime(6) |

**`userpagepermissions`**

| Column | Type |
|---|---|
| `Id` | int AUTO_INCREMENT PK |
| `UserId` | bigint → `users.Id` |
| `PageId` | int → `apppages.Id` (CASCADE DELETE) |
| `CanRead/Write/Create/Delete/Import/Export` | tinyint(1) DEFAULT 0 |
| `CreatedAt` | datetime(6) |

**`userpageoverrides`**

| Column | Type |
|---|---|
| `Id` | int AUTO_INCREMENT PK |
| `UserId` | bigint → `users.Id` |
| `PageId` | int → `apppages.Id` |
| `OverrideType` | varchar(10) — `"Grant"` or `"Deny"` |
| `CreatedAt` | datetime(6) |

**`userpermissionoverrides`** (live table — additional override mechanism)

| Column | Type |
|---|---|
| `Id` | int AUTO_INCREMENT PK |
| `UserId` | int |
| `PageId` | int |
| `CanRead/Write/Create/Delete/Import/Export` | tinyint(1) |
| `CreatedAt`, `UpdatedAt` | datetime(6) |

### Permission Resolution Order
1. `userpagepermissions` (UserId, PageId) → use directly if present.
2. `roledefaultpagepermissions` (RoleTypeName, PageId) → fallback.
3. All-false deny if neither exists.

### DTO

```csharp
// AbsenceApp.Core/DTOs/PagesDtos.cs  (EffectivePermissionDto)
public class EffectivePermissionDto {
    public string PageRoute { get; set; }
    public bool CanRead, CanWrite, CanCreate, CanDelete, CanImport, CanExport;
}
```

### Key Service
`UserManagementService` — manages page permission matrices (`GetPagePermissionsAsync`, `SavePagePermissionsAsync`, `GetRoleDefaultsAsync`).

---

## 8. Subsystem 5 — Feature Flags

### Purpose
Module-level capability toggles. Separate from page CRUD permissions. Controls higher-level feature availability (e.g. enabling a sidebar, enabling a module).

### Live Tables

**`features`**

| Column | Type |
|---|---|
| `Id` | int PK |
| `Code` | varchar(100) — unique feature key |
| `DisplayName` | varchar(150) |
| `Description` | varchar(255) NULL |
| `IsEnabled` | tinyint(1) — global on/off |
| `CreatedAt`, `UpdatedAt` | datetime |

**`rolefeature`**

| Column | Type |
|---|---|
| `Id` | int PK |
| `RoleId` | int → `roles.Id` |
| `FeatureCode` | varchar(100) — string FK to `features.Code` (no DB FK constraint) |
| `IsEnabled` | tinyint(1) |
| `AssignedAt` | datetime |
| `AssignedBy` | int |

**`userfeatureoverride`**

| Column | Type |
|---|---|
| `Id` | int PK |
| `UserId` | int → `users.Id` |
| `FeatureCode` | varchar(100) |
| `IsEnabled` | tinyint(1) |
| `OverriddenAt` | datetime |
| `OverriddenBy` | int |

### EF Mappings
`EntitlementsModelBuilderExtensions.ConfigureEntitlements()` registers:
- `Feature` → `features`
- `RoleFeature` → `rolefeatures`
- `UserFeatureOverride` → `userfeatureoverrides`

### Client Service
`FeaturePermissionApiServiceV2` — `IsAllowedAsync(featureKey)` resolves via `userrole → rolefeature`.

### Resolution Precedence (PRD intent)
1. Global `features.IsEnabled` (false = globally off regardless of role/user).
2. `rolefeature.IsEnabled` for the user's role.
3. `userfeatureoverride` for the specific user (highest priority user-level).

> **Current implementation:** `FeaturePermissionApiServiceV2` implements steps 1 and 2. User overrides (`userfeatureoverride`) are mapped in EF but not yet applied in the runtime resolution path.

---

## 9. Subsystem 6 — User Management

### Purpose
Manages user accounts, roles, profiles, login tracking, and account verification.

### Live Tables

**`users`**

| Column | Type | Notes |
|---|---|---|
| `Id` | int AUTO_INCREMENT PK | |
| `Username` | varchar(100) | Login identity |
| `Email` | varchar(255) | |
| `EmailVerifiedAt` | datetime NULL | |
| `Password` | varchar(255) | PBKDF2-SHA256 (new) or BCrypt (seeded) |
| `StaffId` | int NULL → `staff.Id` | Links user account to staff record |
| `Status` | varchar(50) | `active`, `disabled`, etc. |
| `IsAdmin` | tinyint(1) | Legacy admin flag |
| `LastLoginAt` | datetime NULL | |
| `LastLoginIp` | varchar(50) NULL | |
| `LoginCount` | int | |
| `IsTwoFactorEnabled` | tinyint(1) | |
| `TwoFactorSecret`, `BackupCodes` | varchar NULL | |
| `RememberToken` | varchar(255) NULL | |
| `Timezone` | varchar(50) | |
| `LanguageCode` | varchar(10) | |
| `CreatedAt`, `UpdatedAt` | datetime | |

**`userprofiles`**

| Column | Type | Notes |
|---|---|---|
| `Id` | int AUTO_INCREMENT PK | |
| `UserId` | int → `users.Id` | 1:1 with users |
| `DisplayName` | varchar(150) | |
| `ProfilePictureUrl` | varchar(255) NULL | Local file path (MAUI AppDataDirectory) |
| `Bio` | varchar(500) NULL | |
| `Timezone` | varchar(50) | |
| `LanguageCode` | varchar(10) | |
| `ThemePreference` | varchar(20) | |
| `AccentColor` | varchar(20) NULL | |
| `CreatedAt`, `UpdatedAt` | datetime | |

**`roles`**

| Column | Type |
|---|---|
| `Id` | int AUTO_INCREMENT PK |
| `Name` | varchar(100) |
| `Code` | varchar(50) — used by permission resolution |
| `Description` | varchar(255) NULL |
| `IsSystemRole`, `IsDefault` | tinyint(1) |
| `Priority` | int |
| `CreatedAt`, `UpdatedAt` | datetime |

**`userrole`**

| Column | Type |
|---|---|
| `Id` | int AUTO_INCREMENT PK |
| `UserId` | int → `users.Id` |
| `RoleId` | int → `roles.Id` |
| `AssignedAt` | datetime |
| `AssignedBy` | int |

**`loginaudits`**

| Column | Type |
|---|---|
| `Id` | int AUTO_INCREMENT PK |
| `UserId` | int NULL → `users.Id` |
| `LoginAt` | datetime |
| `LoginIp` | varchar(50) NULL |
| `UserAgent` | varchar(255) NULL |
| `WasSuccessful` | tinyint(1) |
| `FailureReason` | varchar(255) NULL |

**`accountverificationevents`**

| Column | Type |
|---|---|
| `Id` | bigint AUTO_INCREMENT PK |
| `UserId` | bigint |
| `EventType` | (inferred varchar) |
| `EventTime` | datetime |

**`rolechangeaudits`**

Tracks role changes: `UserId`, `OldRoleId`, `NewRoleId`, `ChangedBy`, `ChangedAt`.

### Supplementary User Tables (new — migration `20260509002508_UnifiedProfileSchemaV1`)

| Table | Key Columns | Purpose |
|---|---|---|
| `usercontacts` | `UserId`, `ContactName`, `Relationship`, `Phone`, `Email`, `IsPrimary` | Emergency/alternative contacts for any user account |
| `userdevices` | `UserId`, `DeviceType`, `SerialNumber`, `AssignedDate`, `ReturnedDate` | Devices assigned directly to a user account |
| `userexternalaccounts` | `UserId`, `SystemId`, `SystemName`, `SystemCode`, `AccountUsername`, `AccountEmail`, `Status` | External system accounts linked to a user |
| `usernotes` | `UserId`, `NoteType`, `Body`, `CreatedBy` | Free-text notes on any user account |
| `userpermissionoverrides` | `UserId`, `PageId`, CRUD flags | Additional fine-grained page permission overrides |

### DTOs

| DTO | File | Used for |
|---|---|---|
| `UserDto` | `UserDto.cs` | Simple user list item |
| `UserManagementDtos.cs` | `UserManagementDtos.cs` | UserListItemDto, UserFormDto, UserProfileFullDetailDto, UserSelectDto, RoleDto, LoginAuditRowDto, PermissionMatrixDto, etc. |
| `UserFullViewDto` | `UserFullViewDto.cs` | Full aggregate user profile (used by UserFormPageV2 edit mode) |
| `ProfileSupplementDtos.cs` | `ProfileSupplementDtos.cs` | StaffDeviceRowDto, StaffExternalAccountRowDto, StaffAbsenceRowDto, etc. |

### Interfaces & Services

| Interface | Service | Key operations |
|---|---|---|
| `IUserService` | `UserService` | Basic CRUD on `users` |
| `IUserManagementService` | `UserManagementService` | Aggregate user ops: GetUsersAsync, GetUserDetailAsync, CreateUserAsync, UpdateUserAsync, DeleteUserAsync, GetPagePermissionsAsync, SavePagePermissionsAsync, GetFeaturesAsync, GetStaffAbsencesAsync |
| `IUserFullViewService` | `UserFullViewService` | GetAllAsync (full view with role name) |
| `IAuthService` | `AuthService` | LoginAsync (PBKDF2/BCrypt/plain-text verify, login audit write) |

### Client Services

| Class | Key methods |
|---|---|
| `UserManagementApiServiceV2` | GetUsersAsync, GetUserDetailAsync, CreateUserAsync, UpdateUserAsync, DeleteUserAsync, GetPagePermissionsAsync, SavePagePermissionsAsync, GetFeaturesAsync |

### ViewModels & Pages

| ViewModel | Page | Routes |
|---|---|---|
| `UserListViewModelV2` | `UsersListPageV2` | `/v2/users` |
| `UserFormViewModelV2` | `UserFormPageV2` | `/v2/users/new`, `/v2/users/{StaffId}/new`, `/v2/system/users/new`, `/v2/system/users/{Id}` |
| `UserProfileViewModelV2` | `UserProfilePageV2` | `/v2/users/{Id}` |

**UserFormPageV2 tabs (edit mode):** 0=Basic Info, 1=Contacts, 2=Classes, 3=Devices, 4=External Systems, 5=Medical (placeholder), 6=Absences, 7=Login Audit, 8=Permissions.

---

## 10. Subsystem 7 — Student Absence Management

### Purpose
Records, updates, and views absences for students and staff using a unified `absences` table with a `PersonType` discriminator.

### Live Tables

**`absences`**

| Column | Type | Notes |
|---|---|---|
| `Id` | bigint unsigned AUTO_INCREMENT PK | |
| `PersonType` | enum('Staff','Student') | Discriminator |
| `PersonId` | bigint unsigned | ID of student or staff member |
| `AbsenceTypeId` | bigint unsigned → `absencetypes.Id` | |
| `StatusId` | bigint unsigned → `absencestatuses.Id` | |
| `StartDate`, `EndDate` | date | |
| `DurationDays` | int DEFAULT 0 | |
| `ReportedVia` | enum('Manual','Email','Phone','MIS') DEFAULT 'Manual' | |
| `Notes` | text NULL | |
| `RecordedBy` | bigint unsigned NULL → `users.Id` | |
| `ApprovedBy` | bigint unsigned NULL → `users.Id` | |
| `ApprovedAt` | datetime NULL | |
| `CreatedAt`, `UpdatedAt` | datetime AUTO-UPDATED | |
| `StaffId` | bigint NULL | Denormalised staff link |

**`absencetypes`**

| Column | Type |
|---|---|
| `Id` | bigint unsigned AUTO_INCREMENT PK |
| `Code` | varchar(20) UNIQUE |
| `Name` | varchar(100) |
| `Category` | varchar(50) |
| `IsAuthorised` | tinyint(1) DEFAULT 1 |
| `CreatedAt` | datetime |

**`absencestatuses`**

| Column | Type |
|---|---|
| `Id` | bigint unsigned AUTO_INCREMENT PK |
| `Code` | varchar(20) UNIQUE |
| `Name` | varchar(100) |
| `IsFinal` | tinyint(1) DEFAULT 0 |
| `CreatedAt` | datetime |

**`absenceaudits`**

Audit trail for absence changes: `AbsenceId`, `ChangedBy`, `ChangeType`, `OldStatusId`, `NewStatusId`, `Notes`, `ChangedAt`.

> **Implementation gap:** `AbsenceService` does not currently write `absenceaudits` rows.

### DTOs

| DTO | Purpose |
|---|---|
| `AbsenceDto` | Single absence record returned to client |
| `AbsenceTypeDto` | Absence type lookup item |
| `AbsenceStatusDto` | Status lookup item |
| `AttendanceDto`, `AttendanceMarkDto`, `AttendanceRegisterDto` | Attendance-related data |
| `CreateAbsenceDto` / `UpdateAbsenceDto` / `UpdateAbsenceStatusDto` | Write DTOs |

### Interfaces & Services

| Interface | Service | Key methods |
|---|---|---|
| `IAbsenceService` | `AbsenceService` | GetByPersonAsync, GetByIdAsync, CreateAsync, UpdateAsync, UpdateStatusAsync, DeleteAsync |
| `IAbsenceTypeService` | `AbsenceTypeService` | GetAllAsync, GetByIdAsync |
| `IAbsenceStatusService` | `AbsenceStatusService` | GetAllAsync, GetByIdAsync |
| `IAttendanceService` | `AttendanceService` | Register-based attendance |
| `IAttendanceRegisterService` | `AttendanceService` | Class register operations |

### Client Services

| Class | Key methods |
|---|---|
| `AttendanceApiServiceV2` | GetAttendanceAsync, MarkAttendanceAsync |
| `StudentProfileApiServiceV2` | GetAbsencesAsync, CreateAbsenceAsync, UpdateAbsenceAsync, DeleteAbsenceAsync |
| `StaffProfileApiServiceV2` | GetAbsencesAsync, CreateAbsenceAsync, UpdateAbsenceAsync |

### Pages

| Page | Routes |
|---|---|
| `AttendanceListPageV2` | `/v2/attendance` |
| `AttendanceDetailPageV2` | `/v2/attendance/{Id}` |
| `AttendanceFormPageV2` | `/v2/attendance/{Id}/mark` |
| `AttendanceStudentPageV2` | `/v2/attendance/student` (placeholder) |
| `AttendanceStaffPageV2` | `/v2/attendance/staff` |
| `StudentAbsenceFormPageV2` | `/v2/students/{StudentId}/absences/new`, `/v2/students/{StudentId}/absences/{AbsenceId}/edit` |

### Relationship Map

```
students ──(PersonType='Student', PersonId)──► absences
staff    ──(PersonType='Staff',   PersonId)──► absences
absences ──► absencetypes
absences ──► absencestatuses
absences ──► absenceaudits
users    ──► absenceaudits (ChangedBy)
```

---

## 11. Subsystem 8 — Student Profile

### Purpose
Aggregates multiple related tables to present a complete view of a student: core details, contacts, medical information, flags, notes, classes, and absences.

### Live Tables

**`students`**

| Column | Type | Notes |
|---|---|---|
| `Id` | int PK | |
| `AdmissionNumber` | varchar(50) | |
| `UPN` | varchar(50) NULL | Unique Pupil Number |
| `FirstName`, `LastName` | varchar(100) | |
| `LegalFirstName`, `LegalLastName` | varchar(100) NULL | |
| `MiddleName`, `PreferredName` | varchar(100) NULL | |
| `DateOfBirth` | date | |
| `Gender` | varchar(20) | |
| `YearGroupId` | int → `yeargroups.Id` | |
| `ClassId` | int NULL | (denormalised; classmembers is canonical) |
| `HouseId` | int NULL → `houses.Id` | |
| `Username` | varchar(150) NULL | |
| `SchoolId` | int NULL → `schools.Id` | |
| `AdmissionDate` | date NULL | |
| `Status` | varchar(50) NULL | |
| `CreatedAt`, `UpdatedAt` | datetime | |

**`studentcontacts`**

| Column | Type |
|---|---|
| `Id` | int AUTO_INCREMENT PK |
| `StudentId` | int → `students.Id` |
| `ContactName` | varchar(100) |
| `Relationship` | varchar(50) |
| `Phone` (Mobile/Home/Work) | varchar(50) NULL |
| `Email` | varchar(255) NULL |
| `AddressLine1/2`, `City`, `Postcode` | varchar NULL |
| `Priority` | int |
| `IsEmergencyContact` | tinyint(1) DEFAULT 1 |
| `CreatedAt`, `UpdatedAt` | datetime |

**`studentmedicals`**

Stores `StudentId`, `MedicalCondition`, `IsAllergic`, `AllergyDetails`, `Medication`, `EmergencyActionPlan`, `RecordedBy`.

**`studentflags`**

Stores `StudentId`, `FlagCode`, `IsActive`, `Notes`, `AssignedAt`, `AssignedBy`.

**`studentnotes`** (new — UnifiedProfileSchemaV1)

| Column | Type |
|---|---|
| `Id` | bigint AUTO_INCREMENT PK |
| `StudentId` | int → `students.Id` |
| `NoteType` | varchar(50) DEFAULT 'General' |
| `Body` | text |
| `CreatedBy` | int DEFAULT 0 |
| `CreatedAt`, `UpdatedAt` | datetime(6) AUTO-UPDATED |

### DTOs

| DTO | File | Purpose |
|---|---|---|
| `StudentDto` | `StudentDto.cs` | Basic student row |
| `StudentFullViewDto` | `StudentFullViewDto.cs` | Denormalised student with resolved names (YearGroup, Class, House) |
| `StudentContactDto` | `StudentContactDto.cs` | Contact record |
| `StudentProfileDtos.cs` | `StudentProfileDtos.cs` | StudentMedicalDto, StudentFlagDto, StudentNoteDto |

### Interfaces & Services

| Interface | Service | Key methods |
|---|---|---|
| `IStudentService` | `StudentService` | CRUD on `students` |
| `IStudentFullViewService` | `StudentFullViewService` | GetAllAsync (resolves FK names), GetByIdAsync |
| `IStudentContactService` | `StudentContactService` | GetByStudentAsync |

### Client Services

`StudentProfileApiServiceV2`:
- `GetStudentAsync(studentId)` → resolves via `IStudentFullViewService`
- `GetContactsAsync(studentId)` → via `IStudentContactService`
- `GetMedicalAsync(studentId)` → via `IStudentMedicalRepository` direct
- `GetFlagsAsync(studentId)` → direct DB query
- `GetNotesAsync(studentId)` → direct DB query on `studentnotes`
- `GetAbsencesAsync(studentId)` → via `IAbsenceService.GetByPersonAsync("Student", id)`
- `CreateAbsenceAsync`, `UpdateAbsenceAsync`, `DeleteAbsenceAsync`

### ViewModel: `StudentProfileViewModelV2`

**Tabs:** 0=Profile, 1=Contacts, 2=Medical, 3=Flags, 4=Additional, 5=Notes, 6=Absences (CRUD)

**Initialisation:**
```csharp
await InitAsync(studentId):
  ├─ GetStudentAsync     → Student (StudentFullViewDto)
  ├─ GetContactsAsync    → Contacts
  ├─ GetMedicalAsync     → Medical
  ├─ GetFlagsAsync       → Flags
  ├─ GetNotesAsync       → Notes
  ├─ GetAbsencesAsync    → Absences
  ├─ GetAbsenceTypesAsync
  └─ GetAbsenceStatusesAsync
```

### Pages

| Page | Routes |
|---|---|
| `StudentsListPageV2` | `/v2/students` |
| `StudentProfilePageV2` | `/v2/students/{Id}` |
| `StudentFormPageV2` | `/v2/students/new`, `/v2/students/{Id}/edit` |
| `StudentCalendarPageV2` | `/v2/students/{Id}/calendar` |
| `StudentAbsenceFormPageV2` | `/v2/students/{StudentId}/absences/new`, `/v2/students/{StudentId}/absences/{AbsenceId}/edit` |

### Relationship Map

```
students ──► studentcontacts
students ──► studentmedicals
students ──► studentflags
students ──► studentnotes  (new)
students ──► absences (PersonType='Student')
students ──► yeargroups, houses, schools
students ──► classmembers ──► classes ──► classyeargroups ──► yeargroups
```

---

## 12. Subsystem 9 — Staff Management

### Purpose
Manages staff records, contacts, devices, external accounts, audit trails, and the Staff Profile UI.

### Live Tables

**`staff`**

| Column | Type | Notes |
|---|---|---|
| `Id` | int AUTO_INCREMENT PK | |
| `StaffNumber` | varchar(20) | |
| `FirstName`, `LastName` | varchar(50) | |
| `PreferredName` | varchar(50) NULL | |
| `Title` | varchar(20) | |
| `DateOfBirth` | date | |
| `Gender` | varchar(20) NULL | |
| `WorkEmail` | varchar(100) | |
| `AltEmail` | varchar(100) NULL | |
| `PhoneHome/Mobile/Emergency` | varchar(20) NULL | |
| `EmploymentType`, `ContractType` | varchar(50) | |
| `HireDate` | date | |
| `EndDate` | date NULL | |
| `WorkLocation` | varchar(100) | |
| `ReportingManagerId` | int NULL → `staff.Id` (self-ref) | |
| `JobTitleId` | int → `jobtitles.Id` | |
| `JobGroupId` | int → `jobgroups.Id` | |
| `DepartmentId` | int → `staffdepartments.Id` | |
| `ProfilePhotoUrl` | varchar(255) NULL | Local file path |
| `AccountStatus` | varchar(20) | |
| `CreatedAt`, `UpdatedAt` | datetime(6) | |

**`staffcontacts`**

Same shape as `studentcontacts` but with `StaffId` FK.

**`staffdevices`**

| Column | Notes |
|---|---|
| `StaffId` | → `staff.Id` |
| `DeviceType`, `SerialNumber`, `AssignedDate`, `ReturnedDate`, `Status` | |

**`staffdeviceaudits`**

Tracks device assignment history: `StaffDeviceId`, `ChangedBy`, `ChangeType`, `ChangedAt`, `Notes`.

**`staffexternalaccounts`**

| Column | Notes |
|---|---|
| `StaffId` | → `staff.Id` |
| `SystemName`, `SystemCode`, `AccountUsername`, `AccountEmail`, `Status` | |

**`staffexternalaccountsaudits`**

Audit trail for external account changes.

**`staffnotes`** (new — UnifiedProfileSchemaV1)

| Column | Type |
|---|---|
| `Id` | bigint AUTO_INCREMENT PK |
| `StaffId` | int → `staff.Id` |
| `NoteType` | varchar(50) DEFAULT 'General' |
| `Body` | text |
| `CreatedBy` | int DEFAULT 0 |
| `CreatedAt`, `UpdatedAt` | datetime(6) AUTO-UPDATED |

### Lookup Tables

| Table | EF Entity | Purpose |
|---|---|---|
| `jobtitles` | `JobTitle` | Staff job titles |
| `jobgroups` | `JobGroup` | Job group categories |
| `staffdepartments` | `StaffDepartment` | Departments |
| `staffassignments` | `StaffAssignment` | Work location assignments |

### DTOs

| DTO | Purpose |
|---|---|
| `StaffDto` | Basic staff record (raw FK IDs for edit forms) |
| `StaffFullViewDto` | Denormalised staff with resolved names (JobTitle, JobGroup, Department, Manager) |
| `JobTitleDto` | Lookup |
| `DepartmentDto` | Lookup |
| `ProfileSupplementDtos.cs` | StaffDeviceRowDto, StaffExternalAccountRowDto, StaffAbsenceRowDto, StaffNoteDto |

### Interfaces & Services

| Interface | Service | Key methods |
|---|---|---|
| `IStaffService` | `StaffService` | CRUD on `staff` |
| `IStaffFullViewService` | `StaffFullViewService` | GetAllAsync (resolves JobTitle, JobGroup, Department, ReportingManager name) |

### Client Services

`StaffProfileApiServiceV2`:
- `GetStaffAsync(id)` → resolves via `IStaffFullViewService.GetAllAsync()`, filters by ID
- `GetStaffRawAsync(id)` → direct EF query on `staff` for edit mode
- `GetAbsencesAsync(staffId)` → via `IAbsenceService.GetByPersonAsync("Staff", id)`
- `CreateAbsenceAsync`, `UpdateAbsenceAsync`
- `GetAbsenceTypesAsync`, `GetAbsenceStatusesAsync`
- `UpdateStaffAsync(id, dto)` → direct EF update on `staff`

`StaffApiServiceV2`:
- `GetAllStaffAsync`, `GetStaffByIdAsync`, `CreateStaffAsync`, `UpdateStaffAsync`, `DeleteStaffAsync`

### ViewModel: `StaffProfileViewModelV2`

**Tabs:** 0=Profile, 1=Contacts, 2=Employment, 3=Absences, 4=Additional

**Initialisation:**
```csharp
await InitAsync(staffId):
  ├─ GetStaffAsync       → Staff (StaffFullViewDto)
  ├─ GetAbsencesAsync    → Absences
  ├─ GetAbsenceTypesAsync
  └─ GetAbsenceStatusesAsync
```

### Pages

| Page | Routes |
|---|---|
| `StaffListPageV2` | `/v2/staff` |
| `StaffProfilePageV2` | `/v2/staff/{Id}`, `/v2/staff/new` |
| `StaffDetailPageV2` | `/v2/staff/{Id}/detail` |
| `StaffFormPageV2` | `/v2/staff/{Id}/edit` |

### Relationship Map

```
staff ──► staffcontacts
staff ──► staffdevices ──► staffdeviceaudits
staff ──► staffexternalaccounts ──► staffexternalaccountsaudits
staff ──► staffnotes  (new)
staff ──► absences (PersonType='Staff')
staff ──► staffassignments
staff ──► jobtitles, jobgroups, staffdepartments
staff ──► staffattributes ──► staffattributetypes  (future)
staff ──► staffphases, staffqualifications, staffresponsibilities, staffworkingpatterns (future)
users.StaffId ──► staff.Id
```

---

## 13. Subsystem 10 — Messaging & Notifications

### Purpose
Internal user-to-user messages and in-app notifications. Tables exist and are fully mapped; full UI flows are partially implemented (header icons wired; full page flows in progress).

### Live Tables

**`messages`**

| Column | Type |
|---|---|
| `Id` | bigint AUTO_INCREMENT PK |
| `UserId` | bigint → `users.Id` (recipient) |
| `SenderName` | longtext |
| `Subject` | longtext |
| `Preview` | longtext |
| `CreatedAt` | datetime(6) |
| `IsRead` | tinyint(1) |

**`appnotifications`**

| Column | Type |
|---|---|
| `Id` | bigint AUTO_INCREMENT PK |
| `UserId` | bigint → `users.Id` |
| `Title` | longtext |
| `Body` | longtext |
| `CreatedAt` | datetime(6) |
| `IsRead` | tinyint(1) |

### EF Mappings
`Message` → `messages`; `AppNotification` → `appnotifications`. Both use `ValueGeneratedOnAdd()` (AUTO_INCREMENT).

### DTOs

| DTO | Purpose |
|---|---|
| `MessageDto` | Message list item (Id, SenderName, Subject, Preview, CreatedAt, IsRead) |
| `AppNotificationDto` | Notification list item (Id, Title, Body, CreatedAt, IsRead) |

### Interfaces & Services

| Interface | Service | Key methods |
|---|---|---|
| `IMessageService` | `MessageService` | GetUnreadMessagesAsync(userId) |
| `INotificationService` | `NotificationService` | GetUnreadNotificationsAsync(userId) |

> `MessageService` queries `_db.Messages` filtered by `UserId` and `IsRead = false`.  
> `NotificationService` queries `_db.AppNotifications` filtered by `UserId` and `IsRead = false`.

### Relationship Map

```
users ──► messages       (UserId = recipient)
users ──► appnotifications (UserId = recipient)
```

---

## 14. Subsystem 11 — Audit Logging

### Purpose
Tracks important events and changes: absences, logins, role changes, and generic entity changes.

### Live Tables

**`auditlog`** (generic)

| Column | Type |
|---|---|
| `AuditId` | int PK |
| `UserId` | int |
| `Action` | longtext |
| `Timestamp` | datetime(6) |
| `UserId1` | int (EF shadow FK) |

**`loginaudits`** — see [Subsystem 6](#9-subsystem-6--user-management).

**`rolechangeaudits`** — see [Subsystem 6](#9-subsystem-6--user-management).

**`absenceaudits`** — see [Subsystem 7](#10-subsystem-7--student-absence-management).

### EF Mappings

| Entity | Table | Key |
|---|---|---|
| `AuditLog` | `auditlog` | `AuditId` |
| `LoginAudit` | `loginaudits` | `Id` AUTO_INCREMENT |
| `RoleChangeAudit` | `rolechangeaudits` | `Id` |
| `AbsenceAudit` | `absenceaudits` | `Id` AUTO_INCREMENT |
| `AccountVerificationEvent` | `accountverificationevents` | `Id` bigint AUTO_INCREMENT |
| `StaffDeviceAudit` | `staffdeviceaudits` | |
| `StaffExternalAccountAudit` | `staffexternalaccountsaudits` | |

### DTO & Services

| DTO | File |
|---|---|
| `AuditLogDto` | `AuditLogDto.cs` |

| Interface | Service | Key methods |
|---|---|---|
| `IAuditLogService` | `AuditLogService` | GetAllAsync, GetByIdAsync, GetByUserAsync |

### Client Services & Pages

| Class | Page | Routes |
|---|---|---|
| `AuditLogApiServiceV2` | `AuditLogListPageV2` | `/v2/auditlog` |
| | `AuditLogDetailPageV2` | `/v2/auditlog/{Id}` |
| | `AuditLogFormPageV2` | `/v2/auditlog/{Id}/view` |

### Implementation Gap Notes
- `absenceaudits`: entity and mapping exist; `AbsenceService` does not yet write audit records.
- `auditlog`: entity and mapping exist; no active write code found in current services.

---

## 15. Subsystem 12 — Lookup Tables

### Purpose
Provide static or semi-static reference data used across the system. Small, read-mostly tables defining codes, categories, and structures.

### Live Tables (DB Verified)

| Table | EF Entity | DbSet | Key Columns | Used By |
|---|---|---|---|---|
| `absencetypes` | `AbsenceType` | `AbsenceTypes` | `Id`, `Code`, `Name`, `Category`, `IsAuthorised` | absences.AbsenceTypeId; absence form dropdowns |
| `absencestatuses` | `AbsenceStatus` | `AbsenceStatuses` | `Id`, `Code`, `Name`, `IsFinal` | absences.StatusId; workflow logic |
| `devicetypes` | `DeviceType` | (mapped) | `Id`, `Name`, `Code` | staffdevices classification |
| `externalsystems` | `ExternalSystem` | (mapped) | `Id`, `Name`, `Code` | staffexternalaccounts |
| `phases` | `Phase` | `Phases` | `Id`, `Name` | staffphases; academic structure |
| `roles` | `RoleType` | `RoleTypes` | `Id`, `Name`, `Code`, `IsSystemRole`, `Priority` | userrole, rolemenuitems, roledefaultpagepermissions |
| `houses` | `House` | `Houses` | `Id`, `Name` | students.HouseId |
| `yeargroups` | `YearGroup` | `YearGroups` | `Id`, `Name` | students.YearGroupId, classyeargroups |
| `schools` | `School` | `Schools` | `Id`, `Name` | students.SchoolId |
| `jobtitles` | `JobTitle` | `JobTitles` | `Id`, `Title`, `Code` | staff.JobTitleId |
| `jobgroups` | `JobGroup` | `JobGroups` | `Id`, `Name` | staff.JobGroupId |
| `staffdepartments` | `StaffDepartment` | `StaffDepartments` | `Id`, `Name` | staff.DepartmentId |
| `responsibilitytypes` | `ResponsibilityType` | `ResponsibilityTypes` | `Id`, `Name` | staffresponsibilities |

### DTOs

| DTO | File |
|---|---|
| `YearGroupDto` | `YearGroupDto.cs` |
| `HouseDto` | `HouseDto.cs` |
| `AbsenceTypeDto` | `AbsenceTypeDto.cs` |
| `AbsenceStatusDto` | `AbsenceStatusDto.cs` |
| `JobTitleDto` | `JobTitleDto.cs` |
| `DepartmentDto` | `DepartmentDto.cs` |
| `SubjectDto` | `SubjectDto.cs` |

### Services

| Interface | Service |
|---|---|
| `IYearGroupService` | `YearGroupService` |
| `IHouseService` | `HouseService` |
| `IAbsenceTypeService` | `AbsenceTypeService` |
| `IAbsenceStatusService` | `AbsenceStatusService` |
| `IJobTitleService` | `JobTitleService` |
| `IDepartmentService` | `DepartmentService` |

---

## 16. Subsystem 13 — Future Staff Profile Tables

### Purpose
Preserved for future expansion of Staff Profile and User Profile features. All tables exist in the DB and are mapped in EF; they are not actively used in current service or UI code.

### Tables

| Table | EF Entity | DbSet | Intended Purpose |
|---|---|---|---|
| `staffattributes` | `StaffAttribute` | `StaffAttributes` | Custom key-value attributes for staff (e.g. skills, preferences) |
| `staffattributetypes` | `StaffAttributeType` | `StaffAttributeTypes` | Defines types of staff attributes |
| `staffphases` | `StaffPhase` | `StaffPhases` | Links staff to academic phases/terms |
| `staffqualifications` | `StaffQualification` | `StaffQualifications` | Staff formal qualifications |
| `staffresponsibilities` | `StaffResponsibility` | `StaffResponsibilities` | Named responsibilities assigned to staff |
| `staffworkingpatterns` | `StaffWorkingPattern` | `StaffWorkingPatterns` | Working hours/schedule patterns |
| `staffworklocations` | `StaffWorkLocation` | `StaffWorkLocations` | Location assignments over time |

> **PRD intent:** These tables are explicitly designed as future extension points. No services or client pages currently consume them beyond basic EF mapping. They are safe to query directly when the feature is built.

---

## 17. DTO Reference

All DTOs live in `AbsenceApp.Core/DTOs/`.

| File | Key DTOs | Used by |
|---|---|---|
| `AbsenceDto.cs` | `AbsenceDto`, `CreateAbsenceDto`, `UpdateAbsenceDto`, `UpdateAbsenceStatusDto` | AbsenceService, StudentProfileApiServiceV2, StaffProfileApiServiceV2 |
| `AbsenceTypeDto.cs` | `AbsenceTypeDto` | Absence form dropdowns |
| `AbsenceStatusDto.cs` | `AbsenceStatusDto` | Absence status dropdowns |
| `AppNotificationDto.cs` | `AppNotificationDto` | NotificationService, HeaderV2 |
| `AttendanceDto.cs`, `AttendanceMarkDto.cs`, `AttendanceRegisterDto.cs` | Attendance-related | AttendanceService |
| `AuditLogDto.cs` | `AuditLogDto` | AuditLogService, AuditLogListPageV2 |
| `AuthResultDto.cs` | `AuthResultDto` | AuthService → AppStateService |
| `ClassDto.cs`, `ClassFullViewDto.cs` | Class data | ClassService, ClassFullViewService |
| `DashboardDto.cs` | Dashboard stats | DashboardService |
| `DepartmentDto.cs` | Department lookup | DepartmentService |
| `HouseDto.cs` | House lookup | StudentFullViewService |
| `JobTitleDto.cs` | Job title lookup | StaffFullViewService |
| `MessageDto.cs` | Internal message | MessageService |
| `PagesDtos.cs` | `AppPageDto`, `EffectivePermissionDto`, `RoleDefaultPermDto`, `UserPagePermDto` | PagesService, PermissionServiceV2, UserManagementService |
| `ParentDto.cs` | Parent/guardian data | ParentsApiServiceV2 |
| `ProfileSupplementDtos.cs` | `StaffDeviceRowDto`, `StaffExternalAccountRowDto`, `StaffAbsenceRowDto`, `StaffNoteDto`, `StudentNoteDto` | UserManagementService, profile pages |
| `RoleDto.cs` | `RoleDto` | Role dropdowns |
| `StaffDto.cs` | Raw staff record | StaffFormPageV2 |
| `StaffFullViewDto.cs` | Resolved staff view | StaffListPageV2, StaffProfilePageV2 |
| `StudentContactDto.cs` | Student contacts | StudentProfilePageV2 |
| `StudentDto.cs` | Raw student record | StudentFormPageV2 |
| `StudentFullViewDto.cs` | Resolved student view | StudentsListPageV2, StudentProfilePageV2 |
| `StudentProfileDtos.cs` | `StudentMedicalDto`, `StudentFlagDto`, `StudentNoteDto` | StudentProfilePageV2 |
| `SubjectDto.cs` | Subject lookup | ClassService |
| `TablePageSettingDto.cs`, `TableSettingsDiagnosticDto.cs` | Table config | TableSettingsService |
| `UserDto.cs` | Basic user row | UserService |
| `UserFullViewDto.cs` | Full aggregated user | UserFullViewService |
| `UserManagementDtos.cs` | `UserListItemDto`, `UserFormDto`, `UserProfileFullDetailDto`, `CreateUserDto`, `UpdateUserDto`, `UserSelectDto`, `LoginAuditRowDto`, `PermissionMatrixDto`, `SavePermissionsDto` | UserManagementService, UserFormPageV2 |
| `YearGroupDto.cs` | Year group lookup | StudentFullViewService |

---

## 18. Service Reference

### Data Services (`AbsenceApp.Data/Services/`)

| File | Interface | Primary operations |
|---|---|---|
| `AbsenceService.cs` | `IAbsenceService` | CRUD + status transitions for absences |
| `AbsenceStatusService.cs` | `IAbsenceStatusService` | Absence status lookup |
| `AbsenceTypeService.cs` (inferred) | `IAbsenceTypeService` | Absence type lookup |
| `AttendanceService.cs` | `IAttendanceService` | Attendance mark and register operations |
| `AuditLogService.cs` | `IAuditLogService` | Generic audit log queries |
| `AuthService.cs` | `IAuthService` | Login (PBKDF2/BCrypt), login audit write |
| `ClassFullViewService.cs` | `IClassFullViewService` | Classes with resolved FK names |
| `ClassService.cs` | `IClassService` | Class CRUD |
| `DashboardService.cs` | `IDashboardService` | Absence counts, recent absence list |
| `DepartmentService.cs` | `IDepartmentService` | Department lookup CRUD |
| `HouseService.cs` | `IHouseService` | House lookup |
| `JobTitleService.cs` | `IJobTitleService` | Job title lookup |
| `MessageService.cs` | `IMessageService` | Unread messages per user |
| `NotificationService.cs` | `INotificationService` | Unread notifications per user |
| `PagesService.cs` | `IPagesService` | AppPage CRUD (page registry management) |
| `RoleService.cs` | `IRoleService` | Roles list and lookup |
| `StaffFullViewService.cs` | `IStaffFullViewService` | Staff with resolved names (JobTitle/Group/Dept/Manager) |
| `StaffService.cs` | `IStaffService` | Staff CRUD |
| `StudentContactService.cs` | `IStudentContactService` | Student contact CRUD |
| `StudentFullViewService.cs` | `IStudentFullViewService` | Students with resolved YearGroup/Class/House names |
| `StudentService.cs` | `IStudentService` | Student CRUD |
| `SubjectService.cs` | `ISubjectService` | Subject lookup |
| `TableSettingsService.cs` | `ITableSettingsService` | Per-user table column configuration |
| `UserFullViewService.cs` | `IUserFullViewService` | Users with resolved role names |
| `UserManagementService.cs` | `IUserManagementService` | Aggregate user management (CRUD, permissions, features, staff absences) |
| `UserService.cs` | `IUserService` | Basic user CRUD |
| `YearGroupService.cs` | `IYearGroupService` | Year group lookup |

### Client API Services (`AbsenceApp.Client/Services/ApiV2/Modules/`)

| File | Primary operations |
|---|---|
| `AttendanceApiServiceV2.cs` | Attendance list, mark, register |
| `AuditLogApiServiceV2.cs` | Audit log list and detail |
| `ClassesApiServiceV2.cs` | Classes list, detail, CRUD |
| `FeaturePermissionApiServiceV2.cs` | `IsAllowedAsync(featureKey)` |
| `NavigationApiServiceV2.cs` | `GetMenuCategoriesAsync()`, `GetGlobalSettingsCategoriesAsync()` |
| `PagesApiServiceV2.cs` | AppPage registry CRUD |
| `ParentsApiServiceV2.cs` | Parent/guardian data |
| `SettingsApiServiceV2.cs` | Settings module data |
| `StaffApiServiceV2.cs` | Staff list, detail, CRUD |
| `StaffProfileApiServiceV2.cs` | Staff profile, absences, edit/save |
| `StudentProfileApiServiceV2.cs` | Student profile, contacts, medical, flags, notes, absences |
| `StudentsApiServiceV2.cs` | Students list, detail, CRUD |
| `UserManagementApiServiceV2.cs` | User CRUD, permissions, features, profile detail |

### Client Singleton Services (`AbsenceApp.Client/Services/`)

| Class | Purpose |
|---|---|
| `AppStateService` | IsAuthenticated, DarkMode, SidebarCollapsed, CurrentUserId/UserName/RoleName, OnChange event |
| `PermissionServiceV2` | Loads and caches effective page permissions for current user |
| `NavigationServiceV2` (legacy) | JSON-based menu loader (pre-DB navigation) |
| `ThemeServiceV2` | Theme management |
| `BrandingServiceV2` | Branding/logo configuration |
| `TableConfigService` | Per-schema column definitions for table framework |

---

## 19. ViewModel & Page Reference

All ViewModels in `AbsenceApp.Client/ViewModels/V2/`. All Razor pages in `AbsenceApp.Client/Modules/`.

| ViewModel | Page | Routes | Tabs / Key State |
|---|---|---|---|
| — | `DashboardPageV2` | `/v2/dashboard`, `/v2/dashboard/overview` | Stats cards, recent absences |
| `StudentsListViewModelV2` | `StudentsListPageV2` | `/v2/students` | Sortable/filterable student table |
| `StudentProfileViewModelV2` | `StudentProfilePageV2` | `/v2/students/{Id}` | 7 tabs: Profile, Contacts, Medical, Flags, Additional, Notes, Absences |
| `StudentFormViewModelV2` | `StudentFormPageV2` | `/v2/students/new`, `/v2/students/{Id}/edit` | Add/edit form |
| `StudentDetailViewModelV2` | (placeholder) | `/v2/students/details` | Detail view |
| `StudentCalendarViewModelV2` | `StudentCalendarPageV2` | `/v2/students/{Id}/calendar` | Absence calendar |
| `StudentAbsenceFormViewModelV2` | `StudentAbsenceFormPageV2` | `/v2/students/{StudentId}/absences/new`, `/absences/{AbsenceId}/edit` | Create/edit absence |
| `StaffListViewModelV2` | `StaffListPageV2` | `/v2/staff` | Sortable/filterable staff table |
| `StaffProfileViewModelV2` | `StaffProfilePageV2` | `/v2/staff/{Id}`, `/v2/staff/new` | 5 tabs: Profile, Contacts, Employment, Absences, Additional |
| `StaffDetailViewModelV2` | `StaffDetailPageV2` | `/v2/staff/{Id}/detail` | Read-only staff detail |
| `StaffFormViewModelV2` | `StaffFormPageV2` | `/v2/staff/{Id}/edit` | Edit staff record |
| `ClassesListViewModelV2` | `ClassesListPageV2` | `/v2/classes` | Classes table |
| `ClassDetailViewModelV2` | `ClassDetailPageV2` | `/v2/classes/{Id}` | Class detail |
| `ClassFormViewModelV2` | `ClassFormPageV2` | `/v2/classes/new`, `/v2/classes/{Id}/edit` | Class form |
| `AttendanceListViewModelV2` | `AttendanceListPageV2` | `/v2/attendance` | Attendance records |
| `AttendanceDetailViewModelV2` | `AttendanceDetailPageV2` | `/v2/attendance/{Id}` | Attendance detail |
| `AttendanceFormViewModelV2` | `AttendanceFormPageV2` | `/v2/attendance/{Id}/mark` | Mark attendance |
| `AuditLogListViewModelV2` | `AuditLogListPageV2` | `/v2/auditlog` | Audit log table |
| `AuditLogDetailViewModelV2` | `AuditLogDetailPageV2` | `/v2/auditlog/{Id}` | Audit entry detail |
| `UserListViewModelV2` | `UsersListPageV2` | `/v2/users` | User management table |
| `UserFormViewModelV2` | `UserFormPageV2` | `/v2/users/new`, `/v2/users/{StaffId}/new`, `/v2/system/users/new`, `/v2/system/users/{Id}` | 9 tabs (edit): Basic Info, Contacts, Classes, Devices, External, Medical, Absences, Login Audit, Permissions |
| `UserProfileViewModelV2` | `UserProfilePageV2` | `/v2/users/{Id}` | Read-only user profile |
| `PagesListViewModelV2` | `PagesListPageV2` | `/v2/pages` | App pages registry list |
| `PageFormViewModelV2` | `PageFormPageV2` | `/v2/pages/new`, `/v2/pages/{Id}/edit` | Page record form |
| `SettingsModuleViewModelV2` | `SettingsListPageV2` | `/v2/settings` | Settings module root |
| `TableSettingsViewModelV2` | `ThemeSettingPageV2` | `/v2/settings/theme` | Table/theme settings |

**System Management pages (no dedicated ViewModel — render directly):**

| Page | Route |
|---|---|
| `AllPagesPage` | `/v2/system/pages` |
| `PermissionsPage` | `/v2/system/permissions` |
| `RolesPage` | `/v2/system/roles` |
| `UsersPage` (system) | `/v2/system/users` |
| `PageAccessPage` | `/v2/system/page-access` |
| `PageLayoutsPage` | `/v2/system/pages/layouts` |
| `PageMetadataPage` | `/v2/system/pages/metadata` |

**Placeholder routes** (resolved by `PlaceholderPageV2.razor`):

`/v2/dashboard/trends`, `/v2/dashboard/safeguarding`, `/v2/parents`, `/v2/subjects`, `/v2/diagnostics`, `/v2/site`, `/v2/system/departments`, `/v2/system/job-titles`, `/v2/system/houses`, `/v2/system/year-groups`, `/v2/system/absence-types`, `/v2/system/config/*`

---

## 20. End-to-End Examples

### 20.1 Creating a Student Profile

**User action:** Admin navigates to `/v2/students/new` and submits the form.

```
1. StudentFormPageV2 renders (route: /v2/students/new)
   └─ Bound to StudentFormViewModelV2

2. User fills in: FirstName, LastName, AdmissionNumber, YearGroupId, HouseId, ...

3. User clicks Save → StudentFormViewModelV2.SaveAsync()
   └─ Validates required fields
   └─ Calls StudentsApiServiceV2.CreateStudentAsync(CreateStudentDto)
        └─ IServiceScopeFactory.CreateScope()
        └─ Resolve IStudentService (→ StudentService)
        └─ StudentService.CreateAsync(dto):
             └─ Map CreateStudentDto → Student entity
             └─ _repo.AddAsync(student)  [IStudentRepository]
                  └─ _db.Students.Add(student)
                  └─ _db.SaveChangesAsync()
                  └─ Returns saved Student entity (Id populated from AUTO_INCREMENT)
             └─ Map Student → StudentDto
             └─ Return StudentDto
        └─ Scope disposed
   └─ Navigate to /v2/students/{newId}

4. StudentProfilePageV2 renders (route: /v2/students/{newId})
   └─ StudentProfileViewModelV2.InitAsync(newId)
        ├─ StudentProfileApiServiceV2.GetStudentAsync(newId)
        │    └─ IStudentFullViewService.GetByIdAsync(newId)
        │         └─ Load Student from _db.Students
        │         └─ Load YearGroups dict, Houses dict
        │         └─ Project → StudentFullViewDto (resolves YearGroupName, HouseName)
        │    └─ Return StudentFullViewDto
        ├─ GetContactsAsync(newId)  → empty list (no contacts yet)
        ├─ GetMedicalAsync(newId)   → empty list
        ├─ GetFlagsAsync(newId)     → empty list
        ├─ GetNotesAsync(newId)     → empty list
        ├─ GetAbsencesAsync(newId)  → empty list
        ├─ GetAbsenceTypesAsync()   → full AbsenceType list
        └─ GetAbsenceStatusesAsync() → full AbsenceStatus list

5. Page renders:
   Tab 0 (Profile)   — student details
   Tab 1 (Contacts)  — empty, Add Contact button visible
   Tab 2 (Medical)   — empty
   Tab 3 (Flags)     — empty
   Tab 4 (Additional)
   Tab 5 (Notes)     — empty, Add Note button visible
   Tab 6 (Absences)  — empty, Add Absence button visible
```

**DB writes:**
- `students`: 1 INSERT

---

### 20.2 Editing a Staff Profile

**User action:** HR user navigates to `/v2/staff/42/edit` and updates job title and department.

```
1. StaffFormPageV2 renders (route: /v2/staff/42/edit)
   └─ StaffFormViewModelV2.InitAsync(42)
        └─ StaffProfileApiServiceV2.GetStaffRawAsync(42)
             └─ db.Staff.AsNoTracking().FirstOrDefaultAsync(s => s.Id == 42)
             └─ Map Staff → StaffDto (raw FK IDs for dropdowns)
        └─ Load JobTitles, JobGroups, Departments for dropdowns
        └─ Form populated with current values

2. User changes: JobTitleId = 5, DepartmentId = 3

3. User clicks Save → StaffFormViewModelV2.SaveAsync()
   └─ Calls StaffProfileApiServiceV2.UpdateStaffAsync(42, UpdateStaffDto)
        └─ IServiceScopeFactory.CreateScope()
        └─ Load staff entity: db.Staff.FirstOrDefaultAsync(s => s.Id == 42)
        └─ Apply changes: entity.JobTitleId = 5, entity.DepartmentId = 3, entity.UpdatedAt = now
        └─ db.SaveChangesAsync()
        └─ Scope disposed

4. Navigate to /v2/staff/42 (profile view)
   └─ StaffProfileViewModelV2.InitAsync(42)
        ├─ StaffProfileApiServiceV2.GetStaffAsync(42)
        │    └─ IStaffFullViewService.GetAllAsync()  (loads staff + lookup dicts)
        │    └─ Filter to Id == 42
        │    └─ Map → StaffFullViewDto (resolves JobTitleName, DepartmentName, etc.)
        ├─ GetAbsencesAsync(42)
        ├─ GetAbsenceTypesAsync()
        └─ GetAbsenceStatusesAsync()

5. Page renders:
   Tab 0 (Profile)    — updated JobTitle and Department shown
   Tab 3 (Absences)   — absence list
```

**DB writes:**
- `staff`: 1 UPDATE (JobTitleId, DepartmentId, UpdatedAt)

---

### 20.3 Updating a User Profile

**User action:** Admin navigates to `/v2/system/users/7` and changes email and role.

```
1. UserFormPageV2 renders (route: /v2/system/users/7)
   └─ UserProfileViewModelV2.InitEditAsync(7)
        └─ UserManagementApiServiceV2.GetUserDetailAsync(7)
             └─ UserManagementService.GetUserDetailAsync(7):
                  ├─ Load User (users table)
                  ├─ Load UserProfile (userprofiles table)
                  ├─ Resolve role name via raw SQL:
                  │    SELECT r.Name FROM userrole ur
                  │    INNER JOIN roles r ON r.Id = ur.RoleId
                  │    WHERE ur.UserId = 7
                  ├─ Load UserContacts  (usercontacts WHERE UserId=7)
                  ├─ Load UserDevices   (userdevices WHERE UserId=7)
                  ├─ Load UserExternalAccounts (userexternalaccounts WHERE UserId=7)
                  ├─ Resolve RoleId via raw SQL: SELECT RoleId FROM userrole WHERE UserId=7
                  ├─ Load StaffAssignments (if StaffId linked)
                  ├─ Load LoginAudit rows  (loginaudits WHERE UserId=7)
                  ├─ Load absences (absences WHERE PersonType='Staff' AND PersonId=staffId)
                  └─ Build UserProfileFullDetailDto (aggregate payload)
        └─ Populate all tabs from single DTO

2. User changes email to newemail@school.org and selects Role = "admin"

3. User clicks Save → UserProfileViewModelV2.SaveAsync()
   └─ UserManagementApiServiceV2.UpdateUserAsync(7, UpdateUserDto)
        └─ UserManagementService.UpdateUserAsync(7, dto):
             ├─ Load User from db.Users
             ├─ Apply: user.Email = "newemail@school.org"
             ├─ db.SaveChangesAsync()  [users UPDATE]
             ├─ Resolve existing RoleId:
             │    SELECT COUNT(*) FROM userrole WHERE UserId=7
             ├─ If exists: UPDATE userrole SET RoleId=2 WHERE UserId=7
             │   Else:     INSERT INTO userrole (UserId,RoleId,...) VALUES (7,2,...)
             └─ db.SaveChangesAsync()

4. Page shows Success message. Header banner refreshes to show new email and role.
```

**DB writes:**
- `users`: 1 UPDATE (Email, UpdatedAt)
- `userrole`: 1 UPDATE or INSERT (RoleId)

---

### 20.4 How DTOs Are Created and Used

**Pattern: Entity → DTO (read)**
```
Data Service reads Entity from AppDbContext
  └─ Mapper.ToDto(entity) returns DTO
       └─ DTO is passed to Client API Service
            └─ ViewModel stores DTO
                 └─ Razor page binds to ViewModel properties (renders DTO fields)
```

**Pattern: DTO → Entity (write)**
```
Razor page binds form fields to ViewModel form properties
  └─ ViewModel calls ApiService.CreateXxxAsync(createDto)
       └─ ApiService calls Data Service (via IServiceScopeFactory)
            └─ Data Service: Mapper.ToEntity(createDto) → entity
                 └─ Repository.AddAsync(entity)
                      └─ _db.XxxTable.Add(entity)
                      └─ _db.SaveChangesAsync()
```

**Example — AbsenceMapper:**
```csharp
// Read: Absence entity → AbsenceDto
public static AbsenceDto ToDto(Absence a) => new() {
    Id = a.Id, PersonType = a.PersonType, PersonId = a.PersonId,
    AbsenceTypeId = a.AbsenceTypeId, StatusId = a.StatusId,
    StartDate = a.StartDate, EndDate = a.EndDate, Notes = a.Notes, ...
};

// Write: CreateAbsenceDto → Absence entity
public static Absence ToEntity(CreateAbsenceDto dto, long pendingStatusId) => new() {
    PersonType = dto.PersonType, PersonId = dto.PersonId,
    AbsenceTypeId = dto.AbsenceTypeId, StatusId = pendingStatusId,
    StartDate = dto.StartDate, EndDate = dto.EndDate, Notes = dto.Notes,
    ReportedVia = dto.ReportedVia ?? "Manual", CreatedAt = DateTime.UtcNow, ...
};
```

---

### 20.5 How Permissions and Navigation Resolve (Runtime)

```
User logs in successfully → AppStateService.SetAuthenticated(authResult)
  └─ AppStateService stores: UserId, UserName, RoleName, IsAuthenticated

SidebarV2 initialises → NavigationApiServiceV2.GetMenuCategoriesAsync()
  └─ SQL: menuitems JOIN rolemenuitems JOIN userrole WHERE ur.UserId = @UserId
  └─ BuildCategories() assembles tree
  └─ FilterByPermissionsAsync():
       └─ For each submenu item route → PermissionServiceV2.CanViewAsync(route)
            └─ (on first call) PermissionServiceV2.LoadAsync():
                 └─ Resolves roleCode via userrole JOIN roles
                 └─ Loads all apppages
                 └─ Loads roledefaultpagepermissions for roleCode
                 └─ Loads userpagepermissions for UserId
                 └─ Builds cache: { route → EffectivePermissionDto }
            └─ Returns cache[route].CanRead
       └─ Removes items/groups user cannot read
  └─ Returns filtered MenuCategoryModel list → SidebarV2 renders

User navigates to /v2/students
  └─ StudentsListPageV2.OnInitializedAsync()
  └─ Optionally: PermissionServiceV2.GetAsync("/v2/students") → EffectivePermissionDto
       └─ VM shows/hides action buttons based on CanCreate, CanDelete, CanExport
```

---

## 21. Evidence Provenance

All claims in this document are derived from the following sources, in order of authority:

| # | Source | Status |
|---|---|---|
| 1 | **Live MariaDB introspection** (`SHOW TABLES; DESCRIBE <table>`) | ✅ Executed 2026-05-09 against `absenceapp` database |
| 2 | `AppDbContext.cs` — DbSet declarations and `OnModelCreating` table mappings | ✅ Read in full |
| 3 | `AppDbContextModelSnapshot.cs` — EF snapshot (entity configurations, seed data) | ✅ Partially read (key sections) |
| 4 | Migration `20260509002508_UnifiedProfileSchemaV1.cs` — latest schema migration | ✅ Referenced |
| 5 | Migration `20260424130406_AddNewAppPagesAndRoleDefaults.cs` — AppPages and role defaults | ✅ Referenced |
| 6 | `UserManagementModelBuilderExtensions.cs` — AppPage/permission configuration and seed data | ✅ Read in full |
| 7 | `EntitlementsModelBuilderExtensions.cs` — Feature/role-feature mapping | ✅ Read in full |
| 8 | `NavigationApiServiceV2.cs` — Menu assembly and permission pruning | ✅ Read in full |
| 9 | `PermissionServiceV2.cs` — Permission resolution and caching | ✅ Read in full |
| 10 | `FeaturePermissionApiServiceV2.cs` — Feature flag evaluation | ✅ Read in full |
| 11 | `UserManagementService.cs` — User/permission aggregate operations | ✅ Read (key sections) |
| 12 | `AbsenceService.cs`, `StudentFullViewService.cs`, `StaffFullViewService.cs` | ✅ Read |
| 13 | `StudentProfileApiServiceV2.cs`, `StaffProfileApiServiceV2.cs` | ✅ Read |
| 14 | `AuthService.cs` | ✅ Read |
| 15 | `StudentProfileViewModelV2.cs`, `StaffProfileViewModelV2.cs`, `UserProfileViewModelV2.cs` | ✅ Read |
| 16 | All Razor module page `@page` directives (grep scan) | ✅ Scanned |
| 17 | `docs/product/PRD Table Structures.md` — Authoritative 13-section PRD | ✅ Read in full |

**Conflict policy applied:** Where PRD intent and current implementation differ (e.g. `absenceaudits` not written, `userfeatureoverride` not applied in runtime), both are documented with explicit status labels.

---

*Document generated by GitHub Copilot on 2026-05-09. Re-generate after major migrations or subsystem additions.*
