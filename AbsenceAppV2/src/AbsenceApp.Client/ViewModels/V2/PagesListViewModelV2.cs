/*
===============================================================================
 File        : PagesListViewModelV2.cs
 Namespace   : AbsenceApp.Client.ViewModels.V2
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-04-11
 Updated     : 2026-04-11
-------------------------------------------------------------------------------
 Purpose     : ViewModel for PagesListPageV2. Manages paged page-registry data,
               search, sort, filter, and inline delete confirmation state.
               Uses PagesApiServiceV2 (direct DB via IServiceScopeFactory).
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-04-11  Initial creation (E16 Pages Registry).
-------------------------------------------------------------------------------
 Notes       :
   - Register as Scoped in V2ServiceCollectionExtensions.cs.
   - Delete confirmation is page-scoped: only one row shows the confirm
     prompt at a time.
===============================================================================
*/

using AbsenceApp.Client.Models.TableV2;
using AbsenceApp.Client.Services.ApiV2.Modules;
using AbsenceApp.Core.DTOs;

namespace AbsenceApp.Client.ViewModels.V2;

public sealed class PagesListViewModelV2
{
    // =========================================================================
    // Construction
    // =========================================================================

    private readonly PagesApiServiceV2 _svc;

    public PagesListViewModelV2(PagesApiServiceV2 svc) => _svc = svc;

    // =========================================================================
    // Full dataset cached after first load
    // =========================================================================

    private IReadOnlyList<PageListItemDto> _all = [];

    // =========================================================================
    // State
    // =========================================================================

    public List<PageListItemDto> Items      { get; private set; } = [];
    public int    TotalCount                { get; private set; }
    public int    Page                      { get; private set; } = 1;
    public int    PageSize                  { get; private set; } = 10;
    public string SearchTerm                { get; private set; } = string.Empty;
    public string SortColumn               { get; private set; } = string.Empty;
    public bool   SortAscending            { get; private set; } = true;
    public bool   IsLoading                { get; private set; } = true;
    public string? Error                   { get; private set; }

    /// <summary>The id of the page row showing an inline delete confirmation, or null.</summary>
    public int? ConfirmDeleteId            { get; private set; }

    private Dictionary<string, string> _activeFilters = new();

    // =========================================================================
    // Column schema
    // =========================================================================

    public static List<TableColumnModel> Columns =>
    [
        new() { Key = "name",        Label = "Page",      Visible = true,  Sortable = true,  Order = 0 },
        new() { Key = "slug",        Label = "Page Slug", Visible = true,  Sortable = true,  Order = 1 },
        new() { Key = "categoryKey", Label = "Category",  Visible = true,  Sortable = true,  Order = 2, Width = "130px" },
        new() { Key = "menuKey",     Label = "Menu",      Visible = false, Sortable = true,  Order = 3, Width = "130px" },
        new() { Key = "sortOrder",   Label = "Order",     Visible = false, Sortable = true,  Order = 4, Width = "70px"  },
        new() { Key = "status",      Label = "Status",    Visible = true,  Sortable = true,  Order = 5, Width = "100px" },
    ];

    // =========================================================================
    // Data loading
    // =========================================================================

    public async Task LoadAsync(CancellationToken ct = default)
    {
        IsLoading      = true;
        Error          = null;
        ConfirmDeleteId = null;
        try
        {
            _all = await _svc.GetAllPagesAsync(ct);
            ApplyView();
        }
        catch (Exception ex)
        {
            Error = ex.Message;
        }
        finally
        {
            IsLoading = false;
        }
    }

    // =========================================================================
    // Delete confirmation helpers
    // =========================================================================

    public void SetDeleteConfirm(int id)  => ConfirmDeleteId = id;
    public void CancelDelete()            => ConfirmDeleteId = null;

    public async Task<(bool Success, string? Error)> ConfirmDeleteAsync(
        int id, CancellationToken ct = default)
    {
        var (ok, err) = await _svc.DeletePageAsync(id, ct);
        if (ok)
        {
            ConfirmDeleteId = null;
            await LoadAsync(ct);
        }
        return (ok, err);
    }

    // =========================================================================
    // Search / filter / sort / page
    // =========================================================================

    public void SetSearch(string term)
    {
        SearchTerm = term;
        Page       = 1;
        ApplyView();
    }

    public void SetFilter(string field, string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            _activeFilters.Remove(field);
        else
            _activeFilters[field] = value;
        Page = 1;
        ApplyView();
    }

    public void SetSort(string column)
    {
        if (SortColumn == column)
            SortAscending = !SortAscending;
        else
        {
            SortColumn    = column;
            SortAscending = true;
        }
        ApplyView();
    }

    public void SetPage(int page)
    {
        Page = page;
        ApplyView();
    }

    public void SetPageSize(int size)
    {
        PageSize = size;
        Page     = 1;
        ApplyView();
    }

    public string GetFilterValue(string field) =>
        _activeFilters.TryGetValue(field, out var v) ? v : string.Empty;

    // =========================================================================
    // View computation
    // =========================================================================

    private void ApplyView()
    {
        var q = _all.AsEnumerable();

        // Search
        if (!string.IsNullOrWhiteSpace(SearchTerm))
        {
            var t = SearchTerm.Trim().ToLowerInvariant();
            q = q.Where(p =>
                p.Name.Contains(t, StringComparison.OrdinalIgnoreCase) ||
                p.Slug.Contains(t, StringComparison.OrdinalIgnoreCase) ||
                p.CategoryKey.Contains(t, StringComparison.OrdinalIgnoreCase));
        }

        // Active filters
        foreach (var (field, value) in _activeFilters)
        {
            q = field switch
            {
                "categoryKey" => q.Where(p => p.CategoryKey == value),
                "menuKey"     => q.Where(p => p.MenuKey == value),
                "status"      => q.Where(p => (p.IsActive ? "active" : "inactive") == value),
                _             => q,
            };
        }

        // Sort
        q = SortColumn switch
        {
            "name"        => SortAscending ? q.OrderBy(p => p.Name)        : q.OrderByDescending(p => p.Name),
            "slug"        => SortAscending ? q.OrderBy(p => p.Slug)        : q.OrderByDescending(p => p.Slug),
            "categoryKey" => SortAscending ? q.OrderBy(p => p.CategoryKey) : q.OrderByDescending(p => p.CategoryKey),
            "menuKey"     => SortAscending ? q.OrderBy(p => p.MenuKey)     : q.OrderByDescending(p => p.MenuKey),
            "sortOrder"   => SortAscending ? q.OrderBy(p => p.SortOrder)   : q.OrderByDescending(p => p.SortOrder),
            "status"      => SortAscending ? q.OrderBy(p => p.IsActive)    : q.OrderByDescending(p => p.IsActive),
            _             => q.OrderBy(p => p.SortOrder),
        };

        var list = q.ToList();
        TotalCount = list.Count;
        Items = list.Skip((Page - 1) * PageSize).Take(PageSize).ToList();
    }
}
