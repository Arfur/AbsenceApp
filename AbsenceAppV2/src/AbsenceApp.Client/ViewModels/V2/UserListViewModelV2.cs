/*
===============================================================================
 File        : UserListViewModelV2.cs
 Namespace   : AbsenceApp.Client.ViewModels.V2
 Author      : Michael
 Version     : 1.2.0
 Created     : 2026-04-11
 Updated     : 2026-04-21
-------------------------------------------------------------------------------
 Purpose     : ViewModel for UsersListPageV2. Manages paged user data, search,
               sort, and filter state. Uses UserManagementApiServiceV2 (direct
               DB via IServiceScopeFactory) — required because HTTP calls are
               unreachable from native C# in MAUI Blazor Hybrid.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-04-11  Initial creation (E15 User Management).
   - 1.1.0  2026-04-21  Fixed users disappearing on re-navigation: LoadAsync()
                         now resets SearchTerm, _activeFilters, SortColumn,
                         SortAscending, and Page before each DB reload so the
                         Scoped ViewModel does not retain stale filter state.
   - 1.2.0  2026-04-21  Added staffName sort case. Added DeleteUserAsync().
-------------------------------------------------------------------------------
 Notes       :
   - Register as Scoped in V2ServiceCollectionExtensions.cs.
   - All data operations are performed in-memory after a single EF Core load.
===============================================================================
*/

using AbsenceApp.Client.Models.TableV2;
using AbsenceApp.Client.Services.ApiV2.Modules;
using AbsenceApp.Core.DTOs;

namespace AbsenceApp.Client.ViewModels.V2;

public sealed class UserListViewModelV2
{
    // =========================================================================
    // Construction
    // =========================================================================

    private readonly UserManagementApiServiceV2 _svc;

    public UserListViewModelV2(UserManagementApiServiceV2 svc) => _svc = svc;

    // =========================================================================
    // Full dataset cached after first load
    // =========================================================================

    private IReadOnlyList<UserListItemDto> _all = [];

    /// <summary>Exposes the full unfiltered dataset for building dynamic filter options.</summary>
    public IReadOnlyList<UserListItemDto> AllItems => _all;

    // =========================================================================
    // State
    // =========================================================================

    public List<UserListItemDto> Items     { get; private set; } = [];
    public int    TotalCount               { get; private set; }
    public int    Page                     { get; private set; } = 1;
    public int    PageSize                 { get; private set; } = 10;
    public string SearchTerm               { get; private set; } = string.Empty;
    public string SortColumn               { get; private set; } = string.Empty;
    public bool   SortAscending            { get; private set; } = true;
    public bool   IsLoading                { get; private set; } = true;
    public string? Error                   { get; private set; }

    private Dictionary<string, string> _activeFilters = new();

    // =========================================================================
    // Column schema
    // =========================================================================

    public static List<TableColumnModel> Columns =>
    [
        new() { Key = "staffName",     Label = "Staff",      Visible = true,  Sortable = true,  Order = 0 },
        new() { Key = "fullName",      Label = "Full Name",  Visible = true,  Sortable = true,  Order = 1 },
        new() { Key = "username",      Label = "Username",   Visible = true,  Sortable = true,  Order = 2 },
        new() { Key = "email",         Label = "Email",      Visible = true,  Sortable = false, Order = 3 },
        new() { Key = "roleTypeName",  Label = "Role",       Visible = true,  Sortable = true,  Order = 4, Width = "140px" },
        new() { Key = "status",        Label = "Status",     Visible = true,  Sortable = true,  Order = 5, Width = "100px" },
        new() { Key = "createdAt",     Label = "Created",    Visible = false, Sortable = true,  Order = 6, Width = "110px" },
    ];

    // =========================================================================
    // Data loading
    // =========================================================================

    public async Task LoadAsync(CancellationToken ct = default)
    {
        // Reset all filter/search/sort/page state so every navigation starts fresh.
        // Without this reset the Scoped ViewModel retains state from the previous
        // visit and can silently hide all rows on second navigation.
        SearchTerm     = string.Empty;
        _activeFilters = new Dictionary<string, string>();
        SortColumn     = string.Empty;
        SortAscending  = true;
        Page           = 1;

        IsLoading = true;
        Error     = null;
        try
        {
            _all = await _svc.GetUsersAsync(ct);
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
            q = q.Where(u =>
                u.FullName.Contains(t, StringComparison.OrdinalIgnoreCase) ||
                u.Username.Contains(t, StringComparison.OrdinalIgnoreCase) ||
                u.Email.Contains(t, StringComparison.OrdinalIgnoreCase));
        }

        // Active filters
        foreach (var (field, value) in _activeFilters)
        {
            q = field switch
            {
                "roleTypeName" => q.Where(u => u.RoleTypeName == value),
                "status"       => q.Where(u => u.Status == value),
                _              => q,
            };
        }

        // Sort
        q = SortColumn switch
        {
            "staffName"    => SortAscending ? q.OrderBy(u => u.StaffName)    : q.OrderByDescending(u => u.StaffName),
            "fullName"     => SortAscending ? q.OrderBy(u => u.FullName)     : q.OrderByDescending(u => u.FullName),
            "username"     => SortAscending ? q.OrderBy(u => u.Username)     : q.OrderByDescending(u => u.Username),
            "roleTypeName" => SortAscending ? q.OrderBy(u => u.RoleTypeName) : q.OrderByDescending(u => u.RoleTypeName),
            "status"       => SortAscending ? q.OrderBy(u => u.Status)       : q.OrderByDescending(u => u.Status),
            "createdAt"    => SortAscending ? q.OrderBy(u => u.CreatedAt)    : q.OrderByDescending(u => u.CreatedAt),
            _              => q.OrderBy(u => u.FullName),
        };

        var list = q.ToList();
        TotalCount = list.Count;
        Items = list.Skip((Page - 1) * PageSize).Take(PageSize).ToList();
    }

    // =========================================================================
    // Delete
    // =========================================================================

    public async Task DeleteUserAsync(long userId, CancellationToken ct = default)
    {
        Error = null;
        try
        {
            await _svc.DeleteUserAsync((int)userId, ct);
            await LoadAsync(ct);
        }
        catch (Exception ex)
        {
            Error = ex.Message;
        }
    }
}
