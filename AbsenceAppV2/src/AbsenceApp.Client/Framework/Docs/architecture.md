# FrameworkV2 — Architecture

## Purpose

FrameworkV2 is the extracted, reusable design-system and component layer of AbsenceApp. It separates **framework** (generic, reusable UI primitives) from **app** (Absence-specific business logic and modules). The goal is to make this layer extractable into a standalone NuGet package or shared library in the future.

---

## High-Level Structure

```
FrameworkV2/
├── Components/
│   └── DesignSystem/       # Primitive UI components (Icon, Card, etc.)
├── Config/
│   └── Tokens/             # CSS design tokens (colors, spacing, typography…)
├── Demo/                   # Self-contained demo pages (no app dependencies)
├── Docs/                   # This documentation
├── Layouts/                # Layout shell components (Header, Sidebar, etc.)
├── Models/                 # Shared model types for framework components
├── PageTemplates/          # Full-page layout template compositions
├── Services/               # Framework-level service layer
├── Tables/                 # TableV2 component family
└── Theming/                # (Reserved for theme Razor logic)
```

---

## Relationship to AbsenceApp

FrameworkV2 is a **copy-only, additive** extraction. All original V2 source files remain in place and fully functional under `AbsenceApp.Client`. FrameworkV2 does not replace, move, or delete any existing files.

| Layer | Original Location | FrameworkV2 Location |
|---|---|---|
| DesignSystem components | `Components/DesignSystem/` | `FrameworkV2/Components/DesignSystem/` |
| Layout components | `Components/LayoutV2/` | `FrameworkV2/Layouts/` |
| Page templates | `Components/PageTemplatesV2/` | `FrameworkV2/PageTemplates/` |
| TableV2 components | `Components/TableV2/` | `FrameworkV2/Tables/` |
| Models | `Models/V2/`, `Models/TableV2/`, `Models/Theming/` | `FrameworkV2/Models/` |
| Services | `Services/`, `Services/TableV2/`, `Services/Theming/` | `FrameworkV2/Services/` |
| Config | `wwwroot/config/designsystem/` | `FrameworkV2/Config/` |
| CSS Tokens | `wwwroot/css/tokens/` | `FrameworkV2/Config/Tokens/` |

---

## Namespace Strategy

Razor components in FrameworkV2 automatically get distinct Roslyn namespaces from their folder path. No modifications needed.

C# files explicitly declare `namespace`, so all framework copies have updated declarations:

- **Models:** `AbsenceApp.Client.FrameworkV2.Models`
- **Services:** `AbsenceApp.Client.FrameworkV2.Services`

`using` statements inside service/model files still reference the original namespaces (types are still there — no conflict, no duplication).

---

## Future Packaging

To publish as a standalone library:
1. Move the FrameworkV2 folder into a separate `.csproj` (e.g., `BlazorFrameworkV2`).
2. Update all namespaces to the new root.
3. Replace any `HttpClient` / MAUI-specific DI with abstracted interfaces.
4. Publish as a NuGet package.
