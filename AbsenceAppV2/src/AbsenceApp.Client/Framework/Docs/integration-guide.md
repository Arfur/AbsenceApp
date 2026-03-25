# FrameworkV2 — Integration Guide

## Overview

This guide covers how to integrate FrameworkV2 into a new Blazor application (WebAssembly or Hybrid). FrameworkV2 is currently embedded in `AbsenceApp.Client` but is structured for future extraction.

---

## 1. Project Setup

### Required NuGet Packages

```xml
<PackageReference Include="Microsoft.AspNetCore.Components.WebAssembly" Version="8.*" />
```

For MAUI Blazor Hybrid:
```xml
<PackageReference Include="Microsoft.Maui.Controls" Version="8.*" />
<PackageReference Include="Microsoft.AspNetCore.Components.WebView.Maui" Version="8.*" />
```

---

## 2. Register Services

In `Program.cs` (or `MauiProgram.cs` for MAUI):

```csharp
// Required for config-file loading
builder.Services.AddSingleton<DesignSystemConfigService>();

// Navigation
builder.Services.AddSingleton<NavigationServiceV2>();

// Theming
builder.Services.AddSingleton<ThemeServiceV2>();
builder.Services.AddSingleton<BrandingServiceV2>();

// Tables
builder.Services.AddSingleton<TableConfigService>();

// HttpClient (needed by BrandingServiceV2 and DesignSystemConfigService)
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
});
```

---

## 3. Load CSS Tokens

In `wwwroot/index.html` (WebAssembly) or `wwwroot/index.html` (MAUI):

```html
<link rel="stylesheet" href="css/tokens/colors.css" />
<link rel="stylesheet" href="css/tokens/spacing.css" />
<link rel="stylesheet" href="css/tokens/typography.css" />
<link rel="stylesheet" href="css/tokens/radius.css" />
<link rel="stylesheet" href="css/tokens/layout.css" />
```

---

## 4. Copy Config Files

Copy the JSON config files from `FrameworkV2/Config/` to your `wwwroot/config/designsystem/`:

| File | Purpose |
|---|---|
| `branding.json` | App identity (name, logo, colors) |
| `theme.json` | Default theme mode |
| `components.json` | Component feature flags |
| `icons.json` | Icon alias map |
| `menu.json` | Navigation menu definition |
| `table-schema.json` | Default table column schemas |

---

## 5. Add `@using` Directives

In `_Imports.razor`:

```razor
@using AbsenceApp.Client.Components.DesignSystem
@using AbsenceApp.Client.Components.LayoutV2
@using AbsenceApp.Client.Components.TableV2
@using AbsenceApp.Client.Components.PageTemplatesV2
@using AbsenceApp.Client.Models.V2
@using AbsenceApp.Client.Models.TableV2
@using AbsenceApp.Client.Models.Theming
@using AbsenceApp.Client.Services
@using AbsenceApp.Client.Services.TableV2
@using AbsenceApp.Client.Services.Theming
```

---

## 6. Set Up Layout

Create a `MainLayout.razor` using the layout components:

```razor
@inherits LayoutComponentBase
@using AbsenceApp.Client.Components.LayoutV2

<div class="@_bodyClass">
    <HeaderV2 />
    <div class="app-body">
        <SidebarV2 />
        <main class="app-main">
            <BreadcrumbV2 />
            @Body
        </main>
    </div>
</div>

@code {
    [Inject] ThemeServiceV2 ThemeService { get; set; } = default!;
    private string _bodyClass => ThemeService.ActiveBodyClass;
}
```

---

## 7. Apply Theme Class to Body

For MAUI, apply the theme body class to the `<body>` element via JavaScript interop on app init:

```csharp
// In App.razor or MauiProgram bootstrapping
await ThemeService.GetConfigAsync(); // loads and applies initial mode
```

---

## 8. Verify Integration

Navigate to the demo routes to confirm all systems work:

- `/v2/demo/design-system`
- `/v2/demo/layout`
- `/v2/demo/table`
- `/v2/demo/theming`
