# FrameworkV2 — Design System

## Overview

The design system layer provides the primitive UI building blocks used throughout FrameworkV2. All components are CSS-custom-property-driven via design tokens.

---

## CSS Design Tokens

Tokens live in `FrameworkV2/Config/Tokens/` (copies of `wwwroot/css/tokens/`). They define the visual language of the framework.

| File | Purpose |
|---|---|
| `colors.css` | Color palette and semantic color aliases |
| `spacing.css` | Spacing scale (`--space-1` … `--space-16`) |
| `typography.css` | Font family, size, weight tokens |
| `radius.css` | Border-radius scale |
| `layout.css` | Container widths, sidebar widths, z-index layers |

### Loading Tokens

Include the token files in your app's root stylesheet or `index.html`:

```html
<link rel="stylesheet" href="css/tokens/colors.css" />
<link rel="stylesheet" href="css/tokens/spacing.css" />
<link rel="stylesheet" href="css/tokens/typography.css" />
<link rel="stylesheet" href="css/tokens/radius.css" />
<link rel="stylesheet" href="css/tokens/layout.css" />
```

---

## DesignSystem Components

Source: `FrameworkV2/Components/DesignSystem/`

### `Icon`

Renders a Bootstrap icon by name.

```razor
@using AbsenceApp.Client.Components.DesignSystem

<Icon Name="bi-star" Size="md" />
<Icon Name="bi-check-circle" Color="var(--color-success)" AriaLabel="Success" />
```

| Parameter | Type | Default | Description |
|---|---|---|---|
| `Name` | `string` | *(required)* | Bootstrap icon class name |
| `Size` | `string` | `"md"` | `xs`, `sm`, `md`, `lg`, `xl` |
| `Color` | `string?` | `null` | CSS color value |
| `AriaLabel` | `string?` | `null` | Accessibility label |
| `Class` | `string?` | `null` | Extra CSS classes |

---

### `IconButton`

A button rendered as an icon with optional label.

```razor
<IconButton Icon="bi-pencil" Title="Edit" Variant="ghost" OnClick="HandleEdit" />
<IconButton Icon="bi-trash" Label="Delete" Variant="danger" Size="sm" />
```

| Parameter | Type | Default | Description |
|---|---|---|---|
| `Icon` | `string` | *(required)* | Bootstrap icon class |
| `Title` | `string?` | `null` | Tooltip title |
| `Label` | `string?` | `null` | Visible text label |
| `Variant` | `string` | `"ghost"` | `ghost`, `primary`, `danger`, etc. |
| `Size` | `string` | `"md"` | `sm`, `md`, `lg` |
| `Disabled` | `bool` | `false` | Disable state |
| `OnClick` | `EventCallback` | — | Click handler |

---

### `Card`

Flexible content card container.

```razor
<Card Title="My Card" Shadow="sm" Hoverable="true">
    <p>Card body content here.</p>
</Card>
```

| Parameter | Type | Default | Description |
|---|---|---|---|
| `Title` | `string?` | `null` | Card heading |
| `ChildContent` | `RenderFragment?` | `null` | Body content |
| `Header` | `RenderFragment?` | `null` | Custom header slot |
| `Footer` | `RenderFragment?` | `null` | Custom footer slot |
| `Shadow` | `string` | `"xs"` | `none`, `xs`, `sm`, `md`, `lg` |
| `Hoverable` | `bool` | `false` | Hover effect |
| `ShowDivider` | `bool` | `true` | Divider below header |
| `OnClick` | `EventCallback` | — | Click handler |

---

### `SectionHeader`

Page or section heading with optional icon, subtitle, and action slot.

```razor
<SectionHeader Title="Users" Icon="bi-people" Subtitle="Manage all users">
    <Actions>
        <IconButton Icon="bi-plus" Title="Add User" />
    </Actions>
</SectionHeader>
```

| Parameter | Type | Default | Description |
|---|---|---|---|
| `Title` | `string` | *(required)* | Heading text |
| `Subtitle` | `string?` | `null` | Supporting text |
| `Icon` | `string?` | `null` | Bootstrap icon |
| `Size` | `string` | `"md"` | `sm`, `md`, `lg` |
| `ShowDivider` | `bool` | `true` | Bottom divider |
| `Actions` | `RenderFragment?` | `null` | Right-aligned action slot |

---

## Design System Config

The `FrameworkV2/Config/components.json` and `FrameworkV2/Config/icons.json` files drive runtime behavior of `DesignSystemConfigService`. These are reference copies — the live files served are in `wwwroot/config/designsystem/`.
