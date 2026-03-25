# FrameworkV2 — Layout Templates

## Layout Components

Source: `FrameworkV2/Layouts/`

These components form the top-level shell of a page. They are designed to be composed together in a `MainLayout.razor` equivalent.

| Component | Description |
|---|---|
| `HeaderV2` | Top application header with branding and user actions |
| `SidebarV2` | Collapsible side navigation |
| `BreadcrumbV2` | Contextual breadcrumb trail |
| `ScrollSpyV2` | In-page section navigation anchor tracker |

### Typical Layout Composition

```razor
@inherits LayoutComponentBase
@using AbsenceApp.Client.Components.LayoutV2

<div class="app-shell">
    <HeaderV2 />
    <div class="app-body">
        <SidebarV2 />
        <main class="app-content">
            <BreadcrumbV2 />
            @Body
        </main>
    </div>
</div>
```

---

## Page Template Components

Source: `FrameworkV2/PageTemplates/`

Page templates are full-page compositions that standardize common UI patterns. Each template is a named Razor component accepting typed parameters.

| Template | Description |
|---|---|
| `DetailPageTemplateV2` | Read-only detail view with labeled sections |
| `FormPageTemplateV2` | Editable form with validation and submit/cancel actions |
| `ListPageTemplateV2` | Searchable, filterable list or table-based page |
| `DashboardPageTemplateV2` | Dashboard with metric cards and summary panels |
| `SettingsPageTemplateV2` | Grouped settings with toggle/input items |
| `EmptyStatePageTemplateV2` | Placeholder for empty or no-data pages |
| `ErrorPageTemplateV2` | Standardized error page with code and message |
| `ConfirmDialogTemplateV2` | Modal-style confirmation prompt |
| `WizardPageTemplateV2` | Multi-step wizard with step indicators |
| `ProfilePageTemplateV2` | User profile display with avatar and attributes |
| `ReportPageTemplateV2` | Data report with filter bar and export actions |
| `SplitDetailTemplateV2` | Master-detail split panel layout |

### Example — Detail Page

```razor
@using AbsenceApp.Client.Components.PageTemplatesV2
@using AbsenceApp.Client.Models.V2

<DetailPageTemplateV2 Title="Employee Record"
                       Icon="bi-person"
                       Sections="@_sections"
                       IsLoading="@_loading">
    <Actions>
        <IconButton Icon="bi-pencil" Title="Edit" OnClick="NavigateToEdit" />
    </Actions>
</DetailPageTemplateV2>

@code {
    private bool _loading = false;
    private List<DetailSectionModel> _sections = new();
}
```

### Example — List Page

```razor
@using AbsenceApp.Client.Components.PageTemplatesV2

<ListPageTemplateV2 Title="All Users"
                     Icon="bi-people"
                     IsLoading="@_loading">
    <TableContent>
        <TableV2 TItem="UserDto" Columns="@_columns" Items="@_items" />
    </TableContent>
</ListPageTemplateV2>
```

---

## ScrollSpyV2

`ScrollSpyV2` tracks the active in-page anchor as the user scrolls, enabling side-navigation highlighting for long detail or settings pages.

```razor
<ScrollSpyV2 Sections="@_anchorIds" />

@* Each target section must have a matching id *@
<div id="section-general">...</div>
<div id="section-contacts">...</div>
```

```csharp
private List<string> _anchorIds = ["section-general", "section-contacts"];
```
