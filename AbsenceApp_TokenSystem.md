# **AbsenceApp V2 — Token System Specification (Authoritative Architecture Document)**  
**Version:** 1.0.0  
**Author:** Michael  
**Purpose:**  
This document defines the **complete, end‑to‑end architecture** of the AbsenceApp V2 Token System.  
It is the **single source of truth** for how tokens are:

- stored  
- structured  
- grouped  
- generated  
- consumed  
- rendered  
- edited  

Claude AI must follow this document **exactly** when generating code, SQL, JSON, or UI logic.

---

# **1. Overview**

The AbsenceApp V2 Token System is a **fully dynamic, database‑driven design‑system engine**.

Its goals:

- Zero hardcoded lists  
- Zero manual JSON editing  
- Zero duplication  
- Zero drift  
- One source of truth: **the database**  
- One action to add new components, families, variants, or tokens  
- Automatic generation of `components.json`  
- Automatic generation of editor groups  
- Automatic generation of preview CSS variables  
- Automatic generation of variant lists  
- Automatic generation of alias maps  
- Automatic UI rendering  

The system supports **any number of components**, including:

- Buttons  
- Cards  
- Tables  
- Alerts  
- Badges  
- Inputs  
- Future components  

Everything is driven by **database rows**.

---

# **2. Database Schema (Single Source of Truth)**

The table `designtokens` contains **all structural information** needed to generate:

- components.json  
- editor UI  
- preview CSS  
- variant lists  
- family lists  
- alias maps  

### **Required Columns**

| Column | Purpose |
|--------|---------|
| `Component` | The component type (button, card, table, alert, etc.) |
| `Family` | The family within the component (basic, outline, soft, icon, radius, etc.) |
| `Variant` | The variant (primary, secondary, success, danger, etc.) |
| `GroupName` | The editor group (basic-primary, outline-primary, soft-primary, etc.) |
| `TokenKey` | Unique token identifier |
| `CssVariable` | CSS variable name |
| `DefaultValue` | Default CSS value |
| `CurrentValue` | Current overridden value |
| `SortOrder` | Ordering within a group |
| `IsActive` | Active flag |

### **Rules**

- Every token belongs to exactly one **Component**.  
- Every token belongs to exactly one **Family**.  
- Every token belongs to exactly one **Variant**.  
- Every token belongs to exactly one **GroupName**.  
- GroupName must follow the pattern:  
  ```
  <family>-<variant>
  ```
- No hardcoded lists exist anywhere else in the system.

---

# **3. Token Meaning**

### **Component**
Defines the UI element the token belongs to.

Examples:
- `button`
- `card`
- `table`
- `alert`

### **Family**
Defines the style family within the component.

Examples:
- `basic`
- `outline`
- `soft`
- `icon`
- `radius`
- `action`

### **Variant**
Defines the colour or semantic variant.

Examples:
- `primary`
- `secondary`
- `success`
- `danger`
- `warning`

### **GroupName**
Defines the editor group for the UI.

Pattern:
```
<family>-<variant>
```

Examples:
- `basic-primary`
- `outline-primary`
- `soft-secondary`
- `icon-success`

---

# **4. Dynamic JSON Generator**

The JSON generator produces **100% of components.json dynamically**.

No manual editing.  
No static lists.  
No duplication.

### **4.1 Input**
A list of `DesignTokenRow` objects from SQL:

```sql
SELECT
    Component,
    Family,
    Variant,
    GroupName,
    TokenKey,
    CssVariable
FROM designtokens
WHERE IsActive = 1
ORDER BY Component, Family, Variant, SortOrder;
```

### **4.2 Output Structure**

For each component:

```
{
  "<component>": {
    "tokenFamily": "<component>",
    "defaultVariant": "primary",
    "defaultSize": "md",

    "preview": {
      "cssVariables": [ ... ]
    },

    "variants": [ ... ],

    "variantAliases": {
      "<groupName>": "<variant>"
    },

    "tokenMappings": {
      "<tokenKey>": "<cssVariable>"
    },

    "editor": {
      "groups": {
        "<groupName>": [ "<tokenKey>", ... ]
      }
    }
  }
}
```

### **4.3 How each section is generated**

#### **tokenMappings**
Dictionary of:
```
TokenKey → CssVariable
```

#### **preview.cssVariables**
Distinct list of all CSS variables for the component.

#### **variants**
Distinct list of all variants for the component.

#### **variantAliases**
Maps each GroupName to its Variant.

Example:
```
"basic-primary": "primary"
```

#### **editor.groups**
Groups tokens by GroupName.

Example:
```
"basic-primary": [
  "btn.primary.bg",
  "btn.primary.text",
  "btn.primary.border"
]
```

---

# **5. UI Behaviour (Fully Dynamic)**

The UI reads **only** from `components.json`.

### **5.1 The UI must NOT contain:**

- Hardcoded variant lists  
- Hardcoded family lists  
- Hardcoded component lists  
- Hardcoded editor groups  
- Hardcoded preview lists  
- Hardcoded alias maps  

### **5.2 The UI must dynamically generate:**

- Component accordions  
- Family accordions  
- Variant dropdowns  
- Editor groups  
- Token editors  
- Preview CSS variable lists  
- Example renderings  

### **5.3 Adding new items**

#### **Add a new component**
Add DB rows with:
```
Component = "card"
```
→ UI updates automatically

#### **Add a new family**
Add DB rows with:
```
Family = "soft"
```
→ UI updates automatically

#### **Add a new variant**
Add DB rows with:
```
Variant = "tertiary"
```
→ UI updates automatically

#### **Add a new token**
Add DB row with:
```
TokenKey = "btn.primary.bg"
```
→ UI updates automatically

---

# **6. Naming Conventions**

### **TokenKey**
```
<component>.<variant>.<property>
```

Example:
```
btn.primary.bg
```

### **CssVariable**
```
--ds-<component>-<variant>-<property>
```

Example:
```
--ds-btn-primary-bg
```

### **GroupName**
```
<family>-<variant>
```

Example:
```
soft-primary
```

---

# **7. System Guarantees**

The system guarantees:

- No manual JSON editing  
- No manual C# list editing  
- No duplication  
- No drift  
- No hardcoded values  
- One source of truth: the database  
- One action to add new components/families/variants/tokens  
- Automatic UI updates  
- Automatic JSON generation  
- Automatic editor grouping  
- Automatic preview generation  

---

# **8. What Claude Must Do**

When Claude generates code, it must:

- Use the database as the source of truth  
- Never hardcode variant lists  
- Never hardcode family lists  
- Never hardcode component lists  
- Never hardcode editor groups  
- Always generate JSON dynamically  
- Always follow naming conventions  
- Always follow grouping rules  
- Always follow this document  

---

# **9. End of Specification**

This document is the **authoritative definition** of the AbsenceApp V2 Token System.