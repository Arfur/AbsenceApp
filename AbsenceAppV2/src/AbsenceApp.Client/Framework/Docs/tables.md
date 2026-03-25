# FrameworkV2 — Tables

## Overview

The TableV2 system provides a fully-featured, generic data table for Blazor. It supports sorting, searching, filtering, pagination, row selection, skeleton loading, inline actions, and column-level settings — all driven by `TableColumnModel` definitions.

---

## Component Family

Source: `FrameworkV2/Tables/`

| Component | Description |
|---|---|
| `TableV2` | Main table container — generic `TItem` |
| `FilterRowV2` | Per-column filter input row |
| `InlineActionsV2` | Per-row action button group |
| `PaginationV2` | Page navigation bar |
| `SearchBarV2` | Toolbar search input |
| `SkeletonRowV2` | Loading placeholder row |
| `TableSettingsPageV2` | Column visibility/order settings panel |
| `EmptyStateV2` | No-data placeholder |

---

## `TableColumnModel`

Model defined in `FrameworkV2/Models/TableColumnModel.cs` (original: `Models/TableV2/`).

```csharp
public class TableColumnModel
{
    public string Key { get; set; }           // Unique field identifier
    public string Label { get; set; }         // Column header label
    public bool Sortable { get; set; }        // Enable sort click
    public bool Filterable { get; set; }      // Enable filter row input
    public bool Visible { get; set; } = true; // Show/hide column
    public string? Width { get; set; }        // Optional CSS width
    public string? CssClass { get; set; }     // Extra CSS for cells
}
```

---

## `TableConfigService`

Manages per-user column visibility and ordering. Persisted via `LocalStorage` or equivalent.

Registration:
```csharp
builder.Services.AddSingleton<TableConfigService>();
```

Usage in a component:
```csharp
@inject TableConfigService TableConfig

var columns = await TableConfig.GetColumnsAsync("my-table-id", _defaultColumns);
```

---

## Basic Usage

```razor
@using AbsenceApp.Client.Components.TableV2
@using AbsenceApp.Client.Models.TableV2

<TableV2 TItem="PersonDto"
          Columns="@_columns"
          Items="@_people"
          TotalCount="@_total"
          IsLoading="@_loading"
          ShowSearch="true"
          ShowPagination="true"
          CurrentPage="@_page"
          PageSize="25"
          OnPageChanged="HandlePageChanged"
          OnSearchChanged="HandleSearch"
          AriaLabel="People table">
    <RowTemplate Context="person">
        <td>@person.Name</td>
        <td>@person.Email</td>
    </RowTemplate>
</TableV2>

@code {
    private int _page = 1, _total = 0;
    private bool _loading = false;
    private List<PersonDto> _people = new();
    private List<TableColumnModel> _columns = new()
    {
        new() { Key = "Name",  Label = "Name",  Sortable = true  },
        new() { Key = "Email", Label = "Email", Sortable = false }
    };

    private Task HandlePageChanged(int page) { _page = page; return Task.CompletedTask; }
    private Task HandleSearch(string q)      { return Task.CompletedTask; }
}
```

---

## Selection Modes

```razor
<TableV2 TItem="PersonDto"
          SelectionMode="TableSelectionMode.Multiple"
          OnSelectionChanged="HandleSelectionChanged" ... />
```

`TableSelectionMode` values: `None`, `Single`, `Multiple`.

---

## Empty State Customization

```razor
<TableV2 ... EmptyTitle="No results" EmptyMessage="Try a different search." EmptyIcon="bi-search">
    <EmptyActionsContent>
        <button class="btn btn-primary" @onclick="ResetSearch">Clear search</button>
    </EmptyActionsContent>
</TableV2>
```
