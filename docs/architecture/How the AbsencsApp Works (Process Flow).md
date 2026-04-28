# AbsenceAppV2 — Full Application Architecture & Execution Flow
# (Permanent Reference Document)

This document explains how the AbsenceAppV2 system works end‑to‑end:
- Startup
- Authentication
- Navigation
- Sidebar
- Page header
- Routing
- ViewModels
- API services
- Database access
- Permission layers
- Where failures occur when tables/columns are missing

It is designed to be stored as a long‑term reference.

===============================================================
1. APPLICATION STARTUP
===============================================================

The app starts through MAUI → Blazor WebView → index.html → blazor.webview.js.

MauiProgram.cs registers all services:

- AppDbContext (MySQL)
- NavigationApiServiceV2
- PermissionServiceV2
- UserManagementApiServiceV2
- AppState (global state container)
- All ViewModels

The first Razor component loaded is Routes.razor.

Routes.razor checks:
- If user is authenticated → continue
- If not authenticated → redirect to /login

===============================================================
2. LOGIN FLOW
===============================================================

Login.razor:
- User enters username/password
- Calls LoginService.LoginAsync()

LoginService.LoginAsync:
- Queries Users table
- Joins userrole → roles → roletypes
- Returns UserId + RoleTypeName

If login succeeds:
- AppState.SetUser(UserId, RoleTypeName)
- AppState.Notify() triggers UI refresh
- PermissionServiceV2.ResetAsync()
- NavigationApiServiceV2.ResetAsync()
- Navigate to /v2/dashboard/overview

===============================================================
3. MAIN LAYOUT INITIALISATION
===============================================================

MainLayoutV2.razor:
- Subscribes to AppState.OnChange
- On first render:
  - Loads SidebarV2
  - Loads PageHeaderV2
  - Loads the active page

===============================================================
4. SIDEBAR LOADING (MENU SYSTEM)
===============================================================

SidebarV2 calls:
NavigationServiceV2.GetMenuCategoriesAsync()

If cache empty:
- NavigationApiServiceV2.GetMenuCategoriesAsync() executes SQL:

  SELECT menuitems
  JOIN rolemenuitem
  JOIN userrole
  WHERE RoleType = current user role

This returns:
- 8 categories
- 46 menu items

Sidebar renders:
- Categories
- Menus
- Submenus
- Icons
- Routes

===============================================================
5. PAGE HEADER LOADING (BREADCRUMBS)
===============================================================

PageHeaderV2:
- Receives current route (e.g., /v2/students)
- Uses NavigationServiceV2 to find matching menu item
- Extracts:
  - Group label
  - Item label
  - Icon
  - Description

If route is not in menuitems (e.g., /v2/users/2/edit):
- Breadcrumb shows blank (expected)

===============================================================
6. ROUTING TO A PAGE
===============================================================

When user navigates to a route:
- Routes.razor checks authentication
- Loads the corresponding Razor page
- Razor page loads its ViewModel
- ViewModel loads data via API services
- API services call database via AppDbContext

===============================================================
7. VIEWMODEL EXECUTION FLOW
===============================================================

Example: UsersPageViewModelV2

OnInitializedAsync:
- Calls UserManagementApiServiceV2.GetUsersAsync()
- That calls UserManagementService.GetUsersAsync()
- SQL joins:
  Users → userrole → roles → roletypes → staff

Data returned → ViewModel → Razor page renders table.

===============================================================
8. EDIT USER PAGE FLOW
===============================================================

Route: /v2/users/{id}/edit

UserFormPageV2:
- Loads UserProfileViewModelV2

UserProfileViewModelV2.InitialiseEditAsync(id):
1. GetUserProfileHeaderAsync(id)
2. GetUserProfileDetailAsync(id)
3. GetUserPermissionsAsync(id)

Each of these calls backend services.

===============================================================
9. BACKEND SERVICES (API V2)
===============================================================

UserManagementApiServiceV2 → calls:
UserManagementService (server-side)

PermissionServiceV2 → calls:
AppDbContext.AppPages
AppDbContext.RoleDefaultPagePermissions
AppDbContext.UserPagePermissions

NavigationApiServiceV2 → calls:
Raw SQL against menuitems + rolemenuitem

===============================================================
10. DATABASE ACCESS LAYERS
===============================================================

There are TWO permission systems:

---------------------------------------------------------------
A. NAVIGATION PERMISSIONS (SIDEBAR VISIBILITY)
---------------------------------------------------------------

Tables:
- menuitems
- rolemenuitem
- userfeatureoverride (optional)

Purpose:
- Controls which pages appear in the sidebar

Used by:
- NavigationApiServiceV2
- SidebarV2
- PageHeaderV2

---------------------------------------------------------------
B. PAGE ACTION PERMISSIONS (CRUD RIGHTS)
---------------------------------------------------------------

Tables:
- app_pages
- role_default_page_permissions
- user_page_permissions

Purpose:
- Controls what actions user can perform on a page:
  - CanRead
  - CanWrite
  - CanCreate
  - CanDelete
  - CanImport
  - CanExport

Used by:
- PermissionServiceV2
- UserFormPageV2 (Permissions tab)

===============================================================
11. WHERE FAILURES OCCUR WHEN TABLES ARE MISSING
===============================================================

If app_pages is missing:
- PermissionServiceV2 throws MySQL error
- Caught internally → fail-open (all pages visible)
- Permissions tab shows empty grid

If user_profiles is missing:
- GetUserProfileDetailAsync fails
- ProfileExists=false
- Edit User page shows “not found”

If LoginAudit columns mismatch:
- GetUserProfileHeaderAsync fails
- Header shows blank
- Login history tab empty

===============================================================
12. SUMMARY OF EXECUTION ORDER
===============================================================

1. App starts
2. Routes.razor loads
3. If not authenticated → /login
4. User logs in
5. AppState updated
6. Sidebar loads (menuitems)
7. PageHeader loads (breadcrumbs)
8. Razor page loads
9. ViewModel loads
10. API service loads
11. Database queried
12. UI rendered

===============================================================
END OF DOCUMENT
===============================================================
