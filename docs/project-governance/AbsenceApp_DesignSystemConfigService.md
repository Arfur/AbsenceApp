# **AbsenceApp_DesignSystemConfigService.md**  
**Version:** 1.0.0  
**Author:** Michael  
**Purpose:**  
This document defines the **complete architecture, responsibilities, and behaviour** of the `DesignSystemConfigService` used in AbsenceApp V2.  
It supplements the original PRD, which does **not** describe the dynamic configuration loader or the regeneration pipeline.

Claude AI must treat this file as the authoritative reference for all design‑system configuration loading and regeneration logic.

---

# **1. Overview**

`DesignSystemConfigService` is the **central configuration engine** for the AbsenceApp V2 design system.

It is responsible for:

- Loading all design‑system JSON files  
- Caching them for the lifetime of the application  
- Dynamically regenerating `components.json`  
- Merging static + dynamic JSON  
- Writing output to the MSIX sandbox  
- Providing helper methods for JSON traversal  
- Logging and debugging  

This service is used by:

- V2 components  
- V2 layout  
- Token editor UI  
- Preview rendering  
- Theming  
- Any system that consumes design‑system configuration  

---

# **2. File Structure**

All design‑system JSON files live under:

```
wwwroot/config/designsystem/
```

### **Static JSON files**
- `theme.json`
- `menu.json`
- `table-schema.json`
- `icons.json`
- `static-components.json`

### **Dynamic JSON output**
Generated at runtime:

- `components.json`  
  (written to MSIX sandbox via `FileSystem.AppDataDirectory`)

---

# **3. Responsibilities of DesignSystemConfigService**

## **3.1 Load JSON Files**
Each JSON file is loaded using:

- `FileStream`  
- `JsonSerializer.DeserializeAsync<JsonNode>`  
- `_jsonOptions` (case‑insensitive, comments allowed, trailing commas allowed)

Files are cached in memory:

- `_theme`
- `_menu`
- `_tableSchema`
- `_components`
- `_icons`
- `_globalSettingsMenu`

Once loaded, they are **never reloaded** unless the app restarts.

---

## **3.2 Provide JSON Accessors**

Public methods:

- `GetThemeAsync()`
- `GetMenuAsync()`
- `GetTableSchemaAsync()`
- `GetComponentsAsync()`
- `GetIconsAsync()`
- `GetGlobalSettingsMenuAsync()`

These return **cached JsonObject instances**.

---

## **3.3 Provide JSON Path Lookup**

`GetValue(JsonObject config, string dotPath)`  
Allows components to retrieve nested values using dot‑notation:

Example:

```
GetValue(theme, "colors.primary.bg")
```

Returns a `JsonNode?`.

---

## **3.4 Regenerate components.json**

This is the **core dynamic behaviour**.

The regeneration pipeline:

1. **Load tokens from DB**  
   Using Dapper and the SQL contract defined in the Token System spec.

2. **Build dynamic JSON**  
   Using `BuildDynamicComponentsJson(rows)`.

3. **Load static JSON**  
   From `static-components.json`.

4. **Merge static + dynamic**  
   Static keys override dynamic keys.

5. **Write final components.json**  
   To MSIX sandbox:

   ```
   FileSystem.AppDataDirectory/components.json
   ```

6. **Log debug output**  
   Using `Console.WriteLine` and `Debug.WriteLine`.

7. **Update timestamps**  
   `_updated` field inside the JSON.

---

# **4. Dynamic JSON Builder**

`BuildDynamicComponentsJson(List<DesignTokenRow> rows)`  
Constructs the dynamic portion of `components.json`.

### **4.1 Grouping**
Tokens are grouped by:

```
Component
```

### **4.2 Output Structure**
Each component block contains:

- `tokenFamily`
- `defaultVariant`
- `defaultSize`
- `preview.cssVariables[]`
- `variants[]`
- `variantAliases{}`
- `tokenMappings{}`
- `editor.groups{}`

This structure is fully described in `AbsenceApp_TokenSystem.md`.

---

# **5. Static + Dynamic Merge**

After generating dynamic JSON:

- Load `static-components.json`
- For each key in static JSON:
  - Override or extend dynamic JSON
- Write merged result to output

This allows:

- Static metadata  
- Static examples  
- Static component definitions  

…to coexist with dynamic token‑driven content.

---

# **6. MSIX Output Path**

Because AbsenceApp V2 is a **MAUI Windows MSIX application**, it cannot write to `wwwroot`.

Instead, output is written to:

```
FileSystem.AppDataDirectory/components.json
```

This path is:

- User‑specific  
- Sandbox‑safe  
- Writable  
- Persistent across sessions  

V2 components must load `components.json` from this location.

---

# **7. Error Handling & Logging**

The service logs:

- File load paths  
- Regeneration start/end  
- Token row counts  
- Merge operations  
- Exceptions  

Errors are surfaced to the UI via:

- Console output  
- Debug output  
- Exception propagation  

This ensures:

- Debuggability  
- Transparency  
- Traceability  

---

# **8. Integration with V2 Components**

All V2 components must:

- Load tokens via `GetComponentsAsync()`  
- Use `GetValue()` for nested lookups  
- React to dynamic token changes  
- Use CSS variables for styling  

Examples:

- ButtonV2  
- CardV2  
- TableV2  
- LayoutV2 components  
- Token editor UI  

---

# **9. Integration with Token System**

The service is the **execution engine** for the Token System.

It:

- Loads token rows  
- Builds dynamic JSON  
- Generates editor groups  
- Generates preview CSS variables  
- Generates variant lists  
- Generates alias maps  
- Writes final components.json  

The Token System spec defines the rules;  
DesignSystemConfigService implements them.

---

# **10. Why Claude Needs This File**

The PRD does **not** describe:

- Dynamic JSON loading  
- Regeneration pipeline  
- Static + dynamic merge  
- MSIX output path  
- JSON caching  
- JSON traversal  
- Token integration  

Without this file, Claude will:

- Assume JSON is static  
- Misunderstand where components.json comes from  
- Break token regeneration  
- Break V2 components  
- Break layout integration  

This file prevents that.

---

# **End of AbsenceApp_DesignSystemConfigService.md**