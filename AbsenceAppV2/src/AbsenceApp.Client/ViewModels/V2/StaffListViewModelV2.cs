/*
===============================================================================
 File        : StaffListViewModelV2.cs
 Namespace   : AbsenceApp.Client.ViewModels.V2
 Author      : Michael
 Version     : 1.2.0
 Created     : 2026-03-21
 Updated     : 2026-04-06
-------------------------------------------------------------------------------
 Purpose     : ViewModel for StaffListPageV2. Manages paged staff data,
               search, sort, filter, and the column schema for the V2 table
               component.  Uses IStaffFullViewService (EF Core direct) for
               data access — required because HTTP calls are unreachable from
               native C# in MAUI Blazor Hybrid context.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-21  Initial implementation (Phase 7).
   - 1.2.0  2026-04-06  Phase 3 V1 Parity Issue 4: replaced StaffApiServiceV2
                        (HTTP) with IStaffFullViewService (EF Core direct).
                        Implemented in-memory search, sort, filter, and paging.
-------------------------------------------------------------------------------
 Notes       :
   - Register as Scoped in DI (Phase 10).
   - All data operations are performed in-memory after a single EF Core load.
===============================================================================
*/

using AbsenceApp.Client.Models.DataV2;
using AbsenceApp.Client.Models.TableV2;
using AbsenceApp.Core.DTOs;
using AbsenceApp.Core.Interfaces;

namespace AbsenceApp.Client.ViewModels.V2;

/// <summary>
/// Drives the Staff list page V2. Loads all staff via EF Core
/// (IStaffFullViewService) and applies search/filter/sort/paging in memory.
/// Register as Scoped (Phase 10).
/// </summary>
public sealed class StaffListViewModelV2
{
    private readonly IStaffFullViewService _svc;

    public StaffListViewModelV2(IStaffFullViewService svc) => _svc = svc;

    // -------------------------------------------------------------------------
    // Full dataset cached after first load
    // -------------------------------------------------------------------------

    private IReadOnlyList<StaffFullViewDto> _all = [];

    // -------------------------------------------------------------------------
    // Data state
    // -------------------------------------------------------------------------

    public List<StaffFullViewDto> Items { get; private set; } = [];
    public int TotalCount { get; private set; }
    public int Page { get; private set; } = 1;
    public int PageSize { get; private set; } = 10;
    public string SearchTerm { get; private set; } = string.Empty;
    public string SortColumn { get; private set; } = string.Empty;
    public bool SortAscending { get; private set; } = true;
    public bool IsLoading { get; private set; }
    public string? Error { get; private set; }

    // -------------------------------------------------------------------------
    // Filter state — keyed by field name
    // -------------------------------------------------------------------------

    private Dictionary<string, string> _activeFilters = new();

    // -------------------------------------------------------------------------
    // Column schema — static, shared across all renders
    // -------------------------------------------------------------------------

    public static List<TableColumnModel> Columns =>
    [
        new() { Key = "staffNumber",    Label = "Staff No.",  Visible = true, Sortable = true,  Order = 0, Width = "100px" },
        new() { Key = "title",          Label = "Title",      Visible = true, Sortable = true,  Order = 1, Width = "70px" },
        new() { Key = "firstName",      Label = "First Name", Visible = true, Sortable = true,  Order = 2 },
        new() { Key = "lastName",       Label = "Last Name",  Visible = true, Sortable = true,  Order = 3 },
        new() { Key = "workEmail",      Label = "Email",      Visible = true, Sortable = false, Order = 4 },
        new() { Key = "jobTitleName",   Label = "Job Title",  Visible = true, Sortable = true,  Order = 5 },
        new() { Key = "departmentName", Label = "Department", Visible = true, Sortable = true,  Order = 6 },
        new() { Key = "accountStatus",  Label = "Status",     Visible = true, Sortable = true,  Order = 7, Width = "100px" },
    ];

    // -------------------------------------------------------------------------
    // Data loading
    // -------------------------------------------------------------------------

    public async Task LoadAsync(CancellationToken ct = default)
    {
        IsLoading = true;
        Error = null;
        try
        {
            _all = await _svc.GetAllAsync();
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

    public Task GoToPageAsync(int page, CancellationToken ct = default)
    {
        Page = page;
        ApplyView();
        return Task.CompletedTask;
    }

    public Task ChangePageSizeAsync(int size, CancellationToken ct = default)
    {
        PageSize = size;
        Page = 1;
        ApplyView();
        return Task.CompletedTask;
    }

    public Task SearchAsync(string term, CancellationToken ct = default)
    {
        SearchTerm = term;
        Page = 1;
        ApplyView();
        return Task.CompletedTask;
    }

    public Task SortAsync(string column, bool ascending, CancellationToken ct = default)
    {
        SortColumn = column;
        SortAscending = ascending;
        ApplyView();
        return Task.CompletedTask;
    }

    public Task ApplyFiltersAsync(Dictionary<string, string> filters, CancellationToken ct = default)
    {
        _activeFilters = new Dictionary<string, string>(filters);
        Page = 1;
        ApplyView();
        return Task.CompletedTask;
    }

    public Task ClearFiltersAsync(CancellationToken ct = default)
    {
        _activeFilters.Clear();
        Page = 1;
        ApplyView();
        return Task.CompletedTask;
    }

    // -------------------------------------------------------------------------
    // In-memory view computation
    // -------------------------------------------------------------------------

    private void ApplyView()
    {
        IEnumerable<StaffFullViewDto> query = _all;

        // Search across key text fields (staff_code, first_name, last_name, email)
        if (!string.IsNullOrWhiteSpace(SearchTerm))
        {
            var term = SearchTerm.Trim();
            query = query.Where(s =>
                s.StaffNumber.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                s.FirstName.Contains(term, StringComparison.OrdinalIgnoreCase)   ||
                s.LastName.Contains(term, StringComparison.OrdinalIgnoreCase)    ||
                s.WorkEmail.Contains(term, StringComparison.OrdinalIgnoreCase));
        }

        // Dropdown filters (filterable fields: job_title, department, status)
        foreach (var (key, value) in _activeFilters.Where(f => !string.IsNullOrWhiteSpace(f.Value)))
        {
            query = key switch
            {
                "job_title"   => query.Where(s => s.JobTitleName == value),
                "department"  => query.Where(s => s.DepartmentName == value),
                "status"      => query.Where(s => s.AccountStatus == value),
                _             => query
            };
        }

        // Sort
        query = SortColumn switch
        {
            "staff_code"  => SortAscending ? query.OrderBy(s => s.StaffNumber)   : query.OrderByDescending(s => s.StaffNumber),
            "first_name"  => SortAscending ? query.OrderBy(s => s.FirstName)     : query.OrderByDescending(s => s.FirstName),
            "last_name"   => SortAscending ? query.OrderBy(s => s.LastName)      : query.OrderByDescending(s => s.LastName),
            "job_title"   => SortAscending ? query.OrderBy(s => s.JobTitleName)  : query.OrderByDescending(s => s.JobTitleName),
            "department"  => SortAscending ? query.OrderBy(s => s.DepartmentName): query.OrderByDescending(s => s.DepartmentName),
            "status"      => SortAscending ? query.OrderBy(s => s.AccountStatus) : query.OrderByDescending(s => s.AccountStatus),
            _             => query.OrderBy(s => s.LastName)
        };

        var list = query.ToList();
        TotalCount = list.Count;
        Items = list.Skip((Page - 1) * PageSize).Take(PageSize).ToList();
    }
}

