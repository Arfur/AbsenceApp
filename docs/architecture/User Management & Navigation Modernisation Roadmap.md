# ==============================================================================
# User Management & Navigation Modernisation Roadmap
# ==============================================================================
File        : UserManagement_Roadmap.md
Owner       : Michael
Version     : 1.0.0
Created     : 2026-04-24
Updated     : 2026-04-24
-------------------------------------------------------------------------------
Purpose     : 
  Define the multi-phase roadmap for the User Management & Navigation 
  Modernisation Programme (E15 → E19). This document establishes the 
  authoritative scope, deliverables, dependencies, and implementation order 
  for the access-control and navigation subsystems.

  This roadmap is the single source of truth for:
    - E15 Permission System
    - E16 Pages Registry
    - E17 Role Permission Matrix
    - E18 User Permission Overrides
    - E19 Navigation Audit & Remediation
-------------------------------------------------------------------------------
Status      :
  E15   ✔ COMPLETE
  E16   ☐ Pending
  E17   ☐ Pending
  E18   ☐ Pending
  E19   ☐ Backend fixes complete; UI pending
-------------------------------------------------------------------------------
Changelog   :
  - 1.0.0 (2026-04-24)
      Initial creation. Added full definitions for E15–E19, including scope,
      deliverables, dependencies, and implementation order.
==============================================================================


# ==============================================================================
# E15 — Permission System (COMPLETE)
# ==============================================================================
## Purpose
Establish a deterministic, fail-closed, role-based access control (RBAC) system 
for all V2 application pages.

## Deliverables
- AppPages table (canonical page registry)
- RoleDefaultPagePermission table (role-level defaults)
- UserPagePermission table (explicit user grants)
- UserPageOverride table (allow/deny overrides)
- PermissionServiceV2 (effective permission resolver)
- Fail-closed access control (deny unless explicitly allowed)
- Deterministic route → page resolution
- Full EF Core model configuration
- Seeded AppPages (IDs 1–27)
- Seeded RoleDefaultPagePermission (super_admin defaults)

## Notes
E15 is the foundation for all subsequent phases.  
All other phases depend on the correctness of E15.

Status: **✔ COMPLETE**


# ==============================================================================
# E16 — Pages Registry (Admin UI for AppPages)
# ==============================================================================
## Purpose
Provide an administrative UI to manage the AppPages registry dynamically, 
replacing hardcoded seed entries.

## Scope
- CRUD UI for AppPages
- Validation for Slug, Route, CategoryKey, MenuKey
- Ability to activate/deactivate pages
- Ability to modify SupportsRead/Write/Create/Delete/Import/Export
- Integration with PermissionServiceV2 (cache invalidation)
- Integration with NavigationServiceV2 (menu rebuild)
- Audit logging for all changes

## Deliverables
- `/admin/pages` Blazor module
- Backend API: GET/POST/PUT/DELETE `/api/pages`
- Full form-based editor for AppPages
- Sort order editor
- Category/Menu grouping editor
- Icon selector

## Dependencies
- E15 (AppPages must exist)

Status: **⟡ Pending**


# ==============================================================================
# E17 — Role Permission Matrix (Role-Level RBAC Editor)
# ==============================================================================
## Purpose
Provide a UI for administrators to manage role-level default permissions 
(RoleDefaultPagePermission) without modifying seed data.

## Scope
- Matrix/grid UI:
    Rows   = AppPages
    Columns = Read, Write, Create, Delete, Import, Export
- Role selector (super_admin, admin, teacher, etc.)
- Bulk actions (Allow All, Deny All, Reset)
- Copy permissions from another role
- Backend API for role permission updates
- PermissionServiceV2 cache invalidation
- Audit logging

## Deliverables
- `/admin/roles/{role}/permissions` Blazor module
- Backend API: GET/PUT `/api/roles/{role}/permissions`
- Real-time effective permission preview

## Dependencies
- E15 (RoleDefaultPagePermission)
- E16 (AppPages must be editable)

Status: **⟡ Pending**


# ==============================================================================
# E18 — User Permission Overrides (Per-User Access Control)
# ==============================================================================
## Purpose
Allow administrators to override role defaults on a per-user basis.

## Scope
- UI for viewing effective permissions per user
- Override types:
    - Allow (force allow)
    - Deny (force deny)
    - Reset (remove override)
- Writes to:
    - UserPagePermission
    - UserPageOverride
- Effective permission calculation (role defaults + overrides)
- Audit logging

## Deliverables
- `/admin/users/{id}/permissions` Blazor module
- Backend API: GET/PUT `/api/users/{id}/permissions`
- Real-time effective permission preview

## Dependencies
- E15 (UserPagePermission, UserPageOverride)
- E17 (role defaults must be stable)

Status: **⟡ Pending**


# ==============================================================================
# E19 — Navigation Audit & Remediation
# ==============================================================================
## Purpose
Ensure the navigation system (MenuItems, MenuItemsGlobalConfig, AppPages, 
Blazor routes) remains consistent, correct, and self-healing.

## Scope
- Detect mismatches between:
    - AppPages
    - MenuItems
    - MenuItemsGlobalConfig
    - Actual Blazor routes
- Identify:
    - Missing AppPages
    - Missing MenuItems
    - Orphaned menu entries
    - Wrong CategoryKey/MenuKey
    - Wrong routes
    - Duplicate slugs/routes
- Provide remediation actions:
    - Insert missing AppPages
    - Insert missing MenuItems
    - Fix CategoryKey/MenuKey mismatches
    - Fix route mismatches
    - Remove stale entries

## Deliverables
- `/admin/navigation/audit` Blazor module
- Backend API: GET `/api/navigation/audit`
- Backend API: POST `/api/navigation/fix`
- Automated remediation engine

## Dependencies
- E15 (AppPages)
- E16 (editable AppPages)
- E17/E18 (permissions must be stable)

Status:  
- Backend fixes: **✔ COMPLETE**  
- UI tooling: **⟡ Pending**


# ==============================================================================
# Implementation Order (Mandatory)
# ==============================================================================
1. **E15 — Permission System** (complete)
2. **E16 — Pages Registry**
3. **E17 — Role Permission Matrix**
4. **E18 — User Permission Overrides**
5. **E19 — Navigation Audit UI**

This order is mandatory due to dependency chains.


# ==============================================================================
# Non-Goals
# ==============================================================================
- Not a redesign of the entire navigation UX
- Not a replacement for MenuItems until E19
- Not a redesign of the authentication system
- Not a redesign of the database schema beyond E15 tables


# ==============================================================================
# Risks & Mitigations
# ==============================================================================
- **Risk:** Drift between AppPages and MenuItems  
  **Mitigation:** E19 audit tool

- **Risk:** Incorrect role defaults  
  **Mitigation:** E17 matrix editor

- **Risk:** Incorrect user overrides  
  **Mitigation:** E18 override UI

- **Risk:** Hardcoded AppPages becoming stale  
  **Mitigation:** E16 registry UI


# ==============================================================================
# End of Document
# ==============================================================================


---

# ⭐ **E16 — Pages Registry (Admin UI for AppPages)**  
### **Purpose:**  
Make the AppPages table **editable** instead of hardcoded.

### **Why it exists:**  
Right now, AppPages is seeded in code.  
E16 turns it into a **dynamic, admin‑managed registry**.

### **What E16 contains:**

### 1. **Pages Registry UI (Blazor Admin Page)**
A full CRUD interface for managing AppPages:

- Create new pages  
- Edit existing pages  
- Deactivate pages  
- Change routes  
- Change slugs  
- Change category/menu grouping  
- Change icons  
- Change sort order  
- Change SupportsRead/Write/Create/Delete/Import/Export flags  

### 2. **Validation**
- Slug must be unique  
- Route must be unique  
- CategoryKey must match MenuItems categories  
- MenuKey must match MenuItems menu groups  
- Route must match a real Blazor `@page`  

### 3. **Backend API**
- GET /api/pages  
- POST /api/pages  
- PUT /api/pages/{id}  
- DELETE /api/pages/{id}  

### 4. **Integration**
- PermissionServiceV2 cache invalidation  
- NavigationServiceV2 cache invalidation  
- Audit logging  

### **Deliverable:**  
A fully editable Page Registry that replaces hardcoded AppPages.

---

# ⭐ **E17 — Role Permission Matrix (Role‑Level RBAC Editor)**  
### **Purpose:**  
Give admins a UI to manage **role defaults** instead of editing seeds.

### **Why it exists:**  
Right now, RoleDefaultPagePermission is static.  
E17 makes it **dynamic**.

### **What E17 contains:**

### 1. **Role Permission Matrix UI**
A grid:

- Rows = AppPages  
- Columns = Read / Write / Create / Delete / Import / Export  
- One matrix per role (e.g., super_admin, teacher, admin, attendance_officer)

### 2. **Role Selector**
Dropdown to switch between roles.

### 3. **Permission Toggles**
Each cell is a toggle:

- On = allow  
- Off = deny  

### 4. **Bulk Actions**
- Allow all  
- Deny all  
- Reset to defaults  
- Copy permissions from another role  

### 5. **Backend API**
- GET /api/roles/{role}/permissions  
- PUT /api/roles/{role}/permissions  

### 6. **Integration**
- Writes to RoleDefaultPagePermission  
- PermissionServiceV2 cache invalidation  
- Audit logging  

### **Deliverable:**  
A full RBAC editor for role‑level permissions.

---

# ⭐ **E18 — User Permission Overrides (Per‑User Access Control)**  
### **Purpose:**  
Allow per‑user overrides on top of role defaults.

### **Why it exists:**  
Some users need exceptions:

- A teacher who needs access to Staff  
- A staff member who needs access to Classes  
- A temporary override for a project  

E18 enables this.

### **What E18 contains:**

### 1. **User Permission UI**
A screen under User Profile:

- Shows effective permissions  
- Shows role defaults  
- Shows overrides  
- Allows toggling overrides  

### 2. **Override Types**
- **Allow** (force allow even if role denies)  
- **Deny** (force deny even if role allows)  
- **Reset** (remove override → fall back to role default)  

### 3. **Backend API**
- GET /api/users/{id}/permissions  
- PUT /api/users/{id}/permissions  

### 4. **Integration**
- Writes to:
  - UserPagePermission  
  - UserPageOverride  
- PermissionServiceV2 merges:
  - Role defaults  
  - User overrides  
- Audit logging  

### **Deliverable:**  
A complete per‑user access control system.

---

# ⭐ **E19 — Navigation Audit & Remediation (Menu ↔ Page Registry Alignment)**  
### **Purpose:**  
Ensure the navigation system is **consistent**, **correct**, and **self‑healing**.

### **Why it exists:**  
Your system has:

- AppPages  
- MenuItems  
- MenuItemsGlobalConfig  
- Blazor routes  
- Role permissions  
- User overrides  

These can drift out of sync.

E19 fixes that.

### **What E19 contains:**

### 1. **Navigation Audit Tool**
A diagnostic screen that checks:

- Missing AppPages  
- Missing MenuItems  
- Orphaned menu entries  
- Wrong CategoryKey  
- Wrong MenuKey  
- Wrong routes  
- Duplicate slugs  
- Duplicate routes  
- Pages that exist in UI but not in AppPages  
- Pages that exist in AppPages but not in UI  

### 2. **Remediation Actions**
- Insert missing AppPages  
- Insert missing MenuItems  
- Fix CategoryKey/MenuKey mismatches  
- Fix route mismatches  
- Remove stale entries  
- Auto‑generate RoleDefaultPagePermission rows  

### 3. **Backend API**
- GET /api/navigation/audit  
- POST /api/navigation/fix  

### 4. **Integration**
- PermissionServiceV2  
- NavigationServiceV2  
- AppPages  
- MenuItems  
- MenuItemsGlobalConfig  

### **Deliverable:**  
A self‑correcting navigation system.

---

# ⭐ **Summary Table**

| Phase | Name | Purpose | Status |
|-------|------|---------|--------|
| **E15** | Permission System | Core RBAC engine | ✔ Complete |
| **E16** | Pages Registry | Admin UI for AppPages | Pending |
| **E17** | Role Permission Matrix | Role‑level RBAC editor | Pending |
| **E18** | User Permission Overrides | Per‑user ACL | Pending |
| **E19** | Navigation Audit | Detect + fix menu/page mismatches | Backend complete, UI pending |

---

# ⭐ If you want, I can now generate:

### ✔ A full roadmap `.md` file  
### ✔ A full implementation plan for E16  
### ✔ A full implementation plan for E17  
### ✔ A full implementation plan for E18  
### ✔ A full implementation plan for E19  
### ✔ A combined multi‑phase governance document  

Just tell me:

**“Generate the roadmap file”**