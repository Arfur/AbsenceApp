# **CLAUDE_PROJECT_README.md**  
**AbsenceApp V2 — Claude Project Master Guide**  
**Version:** 1.0.0  
**Author:** Michael  

This document tells Claude AI **exactly how to understand, navigate, and work inside the AbsenceApp V2 codebase**.  
It defines the authoritative files, architectural rules, and development constraints that Claude must follow at all times.

Claude must treat this README as the **root instruction file** for the entire project.

---

# **1. Project Overview**

AbsenceApp V2 is a **.NET 8 MAUI Blazor Hybrid** Windows desktop application.  
It contains:

- A frozen V1 UI (must never be modified)  
- A fully dynamic V2 design system  
- A database‑driven token architecture  
- A dynamic JSON configuration engine  
- A modular V2 layout system  
- A DB‑driven menu system  
- A large set of V2 feature modules  

All new development occurs **only in V2**.

---

# **2. Authoritative Documents**

Claude must load and follow these documents:

### **1. AbsenceApp_PRD.md**  
The master product requirements and architectural constraints.

### **2. AbsenceApp_TokenSystem.md**  
The full specification for the dynamic token system.  
This governs:

- Component/Family/Variant/GroupName  
- TokenKey and CssVariable naming  
- Dynamic JSON generation  
- Editor groups  
- Preview CSS variables  
- Variant aliases  
- How components.json is built  

### **3. AbsenceApp_LayoutV2.md**  
Defines the V2 layout architecture:

- SidebarV2  
- HeaderV2  
- BreadcrumbV2  
- ScrollSpyV2  
- PageTemplateV2  

### **4. AbsenceApp_MenuSystemV2.md**  
Defines the DB‑driven menu system:

- RoleMenuItems  
- Menu filtering  
- Pruning rules  
- Navigation tree building  

### **5. AbsenceApp_DesignSystemConfigService.md**  
Defines the dynamic configuration loader:

- JSON loading  
- JSON caching  
- components.json regeneration  
- static + dynamic merge  
- MSIX output path  

### **6. AbsenceApp_V2_TreeView.md**  
Defines the **actual folder structure** of V2.  
Claude must use this to:

- locate files  
- avoid guessing paths  
- place new components correctly  
- avoid modifying V1  

---

# **3. V1 vs V2 Rules**

### **V1 is frozen**
Claude must **never modify**:

- V1 pages  
- V1 components  
- V1 CSS  
- V1 layouts  
- V1 templates  

### **V2 is the only place for new development**
All new code must be placed under:

```
AbsenceApp.Client/Components/...
AbsenceApp.Client/Layout/...
AbsenceApp.Client/Modules/...
AbsenceApp.Client/Services/...
AbsenceApp.Client/PageTemplates/...
```

### **V2 must not break V1**
All V2 work must be:

- additive  
- isolated  
- token‑driven  
- non‑breaking  

---

# **4. Token System Rules (Summary)**

Full details are in **AbsenceApp_TokenSystem.md**, but Claude must remember:

- Tokens come from the database  
- components.json is generated dynamically  
- No hardcoded variant lists  
- No hardcoded family lists  
- No hardcoded component lists  
- No manual JSON editing  
- All styling must use CSS variables  
- Adding new components/families/variants/tokens = DB rows only  

---

# **5. Menu System Rules**

Claude must follow:

- Menu visibility is controlled by `RoleMenuItems`  
- Feature permissions do NOT control visibility  
- Empty parent categories must be pruned  
- SidebarV2 loads menu items dynamically  
- No hardcoded menu structures  

---

# **6. Layout V2 Rules**

Claude must follow:

- All V2 pages must use PageTemplateV2  
- SidebarV2, HeaderV2, BreadcrumbV2, ScrollSpyV2 are the only layout components  
- Layout must be token‑driven  
- No global CSS overrides  
- No modifying V1 layouts  

---

# **7. Design System Config Rules**

Claude must follow:

- components.json is written to MSIX sandbox  
- static + dynamic JSON must be merged  
- JSON is cached for app lifetime  
- All V2 components must load config via DesignSystemConfigService  
- No direct file access outside the service  

---

# **8. Development Rules for Claude**

Claude must:

### ✔ Use the V2 folder structure  
Never invent paths.  
Never place files in V1.

### ✔ Follow all subsystem documents  
Token System  
Layout V2  
Menu System V2  
DesignSystemConfigService  

### ✔ Follow the PRD  
All functional and non‑functional requirements apply.

### ✔ Avoid guessing  
If unsure, Claude must ask for clarification.

### ✔ Keep code additive  
Never break V1.  
Never modify frozen files.

### ✔ Use tokens for all styling  
No hardcoded colours, spacing, borders, radii, shadows, etc.

### ✔ Use components.json for all V2 components  
Never hardcode variants or families.

---

# **9. When Claude Generates Code**

Claude must:

- Place files in correct V2 folders  
- Use correct namespaces  
- Follow naming conventions  
- Follow token naming rules  
- Follow JSON structure rules  
- Follow menu filtering rules  
- Follow layout rules  
- Follow PRD constraints  
- Follow subsystem documents  

---

# **10. When Claude Is Unsure**

Claude must:

- Ask a clarifying question  
- Never guess  
- Never invent architecture  
- Never invent file paths  
- Never modify V1  

---

# **11. Summary**

This README defines:

- How Claude must behave  
- What documents are authoritative  
- Where code must go  
- What rules govern V2  
- How the design system works  
- How the menu system works  
- How the layout system works  
- How the config loader works  

Claude must follow this README **at all times** when assisting with AbsenceApp V2.

---

# **End of CLAUDE_PROJECT_README.md**