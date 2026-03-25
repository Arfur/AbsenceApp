# FrameworkV2 — Theming & Branding

## Overview

The theming and branding system allows runtime control of color mode (light/dark/system) and application identity (app name, logo, colors). Both systems are JSON-config driven and CSS-custom-property based.

---

## CSS Custom Properties

Theming integrates with the CSS token layer. The active theme class on `<body>` switches the active token set:

| Body class | Theme |
|---|---|
| *(none)* | Light (default) |
| `theme-dark` | Dark mode |

---

## `theme.json` — Theme Configuration

Reference: `FrameworkV2/Config/theme.json`

```json
{
  "defaultMode": "System",
  "modes": {
    "light": { ... },
    "dark": { ... }
  }
}
```

`defaultMode` accepts: `Light`, `Dark`, `System`.

---

## `branding.json` — Branding Configuration

Reference: `FrameworkV2/Config/branding.json`

```json
{
  "appName": "MyApp",
  "logoUrl": "",
  "primaryColor": "#0066cc",
  "accentColor": "#ff6600"
}
```

---

## `ThemeServiceV2`

Source: `FrameworkV2/Services/ThemeServiceV2.cs` (original: `Services/Theming/`)

Manages the active theme mode and applies the correct body class.

### Registration

```csharp
builder.Services.AddSingleton<ThemeServiceV2>();
```

### API

| Member | Description |
|---|---|
| `GetConfigAsync()` | Returns current `ThemeConfigModel` |
| `SetModeAsync(ThemeMode mode)` | Switches to `Light`, `Dark`, or `System` |
| `CycleModeAsync()` | Cycles through modes |
| `IsDarkMode` | `true` when effective mode is dark |
| `ActiveBodyClass` | CSS class string to apply to `<body>` |
| `OnChange` | Event for component re-render subscriptions |

### Usage in a Component

```razor
@inject ThemeServiceV2 ThemeService
@implements IDisposable

<button @onclick="ToggleTheme">Toggle theme</button>
<p>Dark mode: @ThemeService.IsDarkMode</p>

@code {
    protected override void OnInitialized()
        => ThemeService.OnChange += StateHasChanged;

    private async Task ToggleTheme()
        => await ThemeService.CycleModeAsync();

    public void Dispose()
        => ThemeService.OnChange -= StateHasChanged;
}
```

---

## `BrandingServiceV2`

Source: `FrameworkV2/Services/BrandingServiceV2.cs` (original: `Services/Theming/`)

Loads and provides the `BrandingConfigModel` from `branding.json`.

### Registration

```csharp
builder.Services.AddSingleton<BrandingServiceV2>();
```

### API

| Member | Description |
|---|---|
| `GetConfigAsync()` | Returns `BrandingConfigModel` |
| `ReloadAsync()` | Re-fetches from JSON |
| `OnChange` | Event for component re-render subscriptions |

### Usage in a Component

```razor
@inject BrandingServiceV2 BrandingService

<h1>@_branding?.AppName</h1>

@code {
    private BrandingConfigModel? _branding;

    protected override async Task OnInitializedAsync()
        => _branding = await BrandingService.GetConfigAsync();
}
```

---

## `ThemeMode` Enum

```csharp
public enum ThemeMode { Light, Dark, System }
```

---

## `ThemeConfigModel` / `BrandingConfigModel`

Both are simple POCO classes defined in `FrameworkV2/Models/`. They are deserialized directly from JSON and have no business logic.
