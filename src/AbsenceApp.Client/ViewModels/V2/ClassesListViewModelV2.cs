/*
===============================================================================
 File        : ClassesListViewModelV2.cs
 Namespace   : AbsenceApp.Client.ViewModels.V2
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-21
 Updated     : 2026-03-21
-------------------------------------------------------------------------------
 Purpose     : ViewModel for ClassesListPageV2. Manages paged class data,
               search, sort, and the column schema for the V2 table component.
               Delegates data access to ClassesApiServiceV2.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-21  Initial implementation (Phase 7).
-------------------------------------------------------------------------------
 Notes       :
   - Register as Scoped in DI (Phase 10).
   - Columns is a static property — schema defined once, shared across renders.
===============================================================================
*/

using AbsenceApp.Client.Models.DataV2;
using AbsenceApp.Client.Models.TableV2;
using AbsenceApp.Client.Services.ApiV2.Modules;
using AbsenceApp.Core.DTOs;

namespace AbsenceApp.Client.ViewModels.V2;

/// <summary>
/// Drives the Classes list page V2. Register as Scoped (Phase 10).
/// </summary>
public sealed class ClassesListViewModelV2
{
    private readonly ClassesApiServiceV2 _api;

    public ClassesListViewModelV2(ClassesApiServiceV2 api) => _api = api;

    // -------------------------------------------------------------------------
    // Data state
    // -------------------------------------------------------------------------

    public List<ClassDto> Items { get; private set; } = [];
    public int TotalCount { get; private set; }
    public int Page { get; private set; } = 1;
    public int PageSize { get; private set; } = 25;
    public string SearchTerm { get; private set; } = string.Empty;
    public string SortColumn { get; private set; } = string.Empty;
    public bool SortAscending { get; private set; } = true;
    public bool IsLoading { get; private set; }
    public string? Error { get; private set; }

    // -------------------------------------------------------------------------
    // Column schema — static, shared across all renders
    // -------------------------------------------------------------------------

    public static List<TableColumnModel> Columns =>
    [
        new() { Key = "id",          Label = "ID",          Visible = true, Sortable = true, Order = 0, Width = "80px" },
        new() { Key = "name",        Label = "Class Name",  Visible = true, Sortable = true, Order = 1 },
        new() { Key = "description", Label = "Description", Visible = true, Sortable = false, Order = 2 },
    ];

    // -------------------------------------------------------------------------
    // Data loading
    // -------------------------------------------------------------------------

    public async Task LoadAsync(CancellationToken ct = default)
    {
        IsLoading = true;
        Error = null;
        var result = await _api.GetPagedAsync(Page, PageSize, BuildQueryString(), ct);
        if (result.Success && result.Data is not null)
        {
            Items = result.Data.Items;
            TotalCount = result.Data.TotalCount;
        }
        else
        {
            Error = result.ErrorMessage ?? "Failed to load classes.";
        }
        IsLoading = false;
    }

    public async Task GoToPageAsync(int page, CancellationToken ct = default)
    { Page = page; await LoadAsync(ct); }

    public async Task ChangePageSizeAsync(int size, CancellationToken ct = default)
    { PageSize = size; Page = 1; await LoadAsync(ct); }

    public async Task SearchAsync(string term, CancellationToken ct = default)
    { SearchTerm = term; Page = 1; await LoadAsync(ct); }

    public async Task SortAsync(string column, bool ascending, CancellationToken ct = default)
    { SortColumn = column; SortAscending = ascending; await LoadAsync(ct); }

    private string BuildQueryString()
    {
        var parts = new List<string>();
        if (!string.IsNullOrWhiteSpace(SearchTerm))
            parts.Add($"search={Uri.EscapeDataString(SearchTerm)}");
        if (!string.IsNullOrWhiteSpace(SortColumn))
        {
            parts.Add($"sortBy={SortColumn}");
            parts.Add($"sortDir={( SortAscending ? "asc" : "desc" )}");
        }
        return string.Join("&", parts);
    }
}
