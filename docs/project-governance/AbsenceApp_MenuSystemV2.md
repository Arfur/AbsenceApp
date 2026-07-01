# **AbsenceApp_MenuSystemV2.md**  
**Version:** 1.0.0  
**Author:** Michael  
**Purpose:**  
This document defines the **complete architecture and behaviour** of the AbsenceApp V2 Menu System.  
It supplements the original PRD, which does **not** describe the actual menu implementation used in the live application.

Claude AI must treat this file as the authoritative reference for all menu‑related logic.

---

# **1. Overview**

The V2 Menu System is a **database‑driven navigation framework** that determines:

- Which menu items a user can see  
- How the sidebar is structured  
- How parent/child menu groups are pruned  
- How role‑based visibility is enforced  
- How the V2 layout loads navigation dynamically  

The original PRD describes role‑based navigation conceptually, but the real system uses a **new explicit mapping table** and a **new filtering pipeline** that did not exist when the PRD was written.

This document explains that system.

---

# **2. Database Tables**

The V2 Menu System uses **three** tables:

### **2.1 `ui_MenuItems`**  
The authoritative source of all menu items.

| Column | Purpose |
|--------|---------|
| `MenuItemId` | Primary key |
| `ParentId` | Parent menu item (nullable) |
| `Label` | Display text |
| `Icon` | Fluent UI icon name |
| `Route` | Navigation route (nullable for parent groups) |
| `SortOrder` | Ordering within parent |
| `IsActive` | Visibility flag |

### **2.2 `RoleMenuItems` (NEW — not in PRD)**  
Defines explicit **Role → MenuItem** visibility mapping.

| Column | Purpose |
|--------|---------|
| `RoleId` | Role identifier |
| `MenuItemId` | Menu item identifier |

### **2.3 `Roles`**  
Standard role table (SuperAdmin, Admin, OfficeStaff, etc.)

---

# **3. Why This System Exists**

The original PRD assumed:

- Menu visibility could be inferred from feature permissions  
- Menu visibility was tied to `perm_RoleFeatureMap`  

This caused:

- Ambiguity  
- Inconsistent visibility  
- Hard‑to‑debug behaviour  
- Incorrect menu rendering  

The V2 system fixes this by making menu visibility **explicit**.

---

# **4. Menu Loading Pipeline**

The V2 menu system follows a **four‑stage pipeline**.

## **4.1 Stage 1 — Load All Menu Items**
Load all rows from `ui_MenuItems`:

- Sorted by `ParentId` and `SortOrder`  
- Includes inactive items (filtered later)

## **4.2 Stage 2 — Filter by RoleMenuItems**
For the current user’s role:

```
SELECT MenuItemId FROM RoleMenuItems WHERE RoleId = @role
```

Only menu items in this list are visible.

This is the **core rule**:

> If a role does not have a row in RoleMenuItems for a menu item,  
> **that menu item is not visible**, regardless of feature permissions.

## **4.3 Stage 3 — Prune Empty Parent Categories**
If a parent menu item has:

- No visible children  
- No route of its own  

…it is removed.

This ensures:

- No empty categories  
- No dead menu groups  
- Clean sidebar structure  

## **4.4 Stage 4 — Build Final Navigation Tree**
The final tree is:

- Hierarchical  
- Sorted  
- Role‑filtered  
- Pruned  
- Ready for SidebarV2  

---

# **5. Integration with SidebarV2**

SidebarV2:

- Loads the final navigation tree  
- Renders parent/child groups  
- Highlights active route  
- Supports collapse/expand  
- Uses Fluent UI icons  
- Applies token‑driven styling  

No hardcoded menu items exist in SidebarV2.

---

# **6. Integration with Feature Permissions**

Menu visibility and feature permissions are **separate systems**.

### **Menu visibility**  
Controlled by:

- `RoleMenuItems`

### **Feature permissions**  
Controlled by:

- `perm_RoleFeatureMap`

### **Rules**
- A user may **see** a menu item but still be blocked from actions inside it.  
- A user may **not** see a menu item even if they have feature permissions.  
- Visibility is **explicit**, not inferred.

This separation is intentional and required.

---

# **7. Behavioural Rules**

### **7.1 Default Deny**
If a role is not mapped to a menu item → it is hidden.

### **7.2 No Inference**
Menu visibility is **never** inferred from:

- Role type  
- Feature permissions  
- User profile  
- Route metadata  

### **7.3 No Hardcoded Menus**
All menus must come from:

- `ui_MenuItems`  
- `RoleMenuItems`  

### **7.4 No Orphaned Children**
If a parent is hidden, all children are hidden.

### **7.5 No Empty Parents**
Parents with no visible children are removed.

---

# **8. Example Flow**

User logs in as **OfficeStaff**:

1. Load all menu items  
2. Load RoleMenuItems for OfficeStaff  
3. Filter menu items  
4. Remove empty parents  
5. Build final tree  
6. SidebarV2 renders it  

If OfficeStaff is not mapped to “Global Settings”:

- The entire Global Settings group disappears  
- Even if the user has feature permissions  
- Even if the route exists  

---

# **9. Why Claude Needs This File**

The PRD does **not** describe:

- RoleMenuItems  
- Explicit menu mapping  
- Pruning rules  
- Filtering pipeline  
- Integration with SidebarV2  

Without this file, Claude will:

- Guess  
- Infer old behaviour  
- Misunderstand menu visibility  
- Break navigation logic  
- Generate incorrect code  

This file prevents that.

---

# **End of AbsenceApp_MenuSystemV2.md**