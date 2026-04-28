/*
===============================================================================
 File        : StudentsListViewModelV2.cs
 Namespace   : AbsenceApp.Client.ViewModels.V2
 Author      : Michael
 Version     : 1.2.0
 Created     : 2026-03-21
 Updated     : 2026-04-05
-------------------------------------------------------------------------------
 Purpose     : ViewModel for StudentsListPageV2. Manages paged student data,
               search, sort, filter, and the column schema for the V2 table component.
               Uses IStudentFullViewService (EF Core direct) for data access —
               required because HTTP calls to http://localhost/ are unreachable
               from native C# in MAUI Blazor Hybrid context.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-21  Initial implementation (Phase 7).
   - 1.1.0  2026-04-04  Phase 3 Stabilisation Issue 3: default PageSize changed
                         from 25 to 10 to match V1 reference; added filter state
                         and ApplyFiltersAsync/ClearFiltersAsync methods.
   - 1.2.0  2026-04-05  Phase 3 Remediation Issue 2: replaced StudentsApiServiceV2
                         (HTTP) with IStudentFullViewService (EF Core direct).
                         Implemented in-memory search, sort, filter, and paging
                         so data displays correctly in MAUI Blazor Hybrid.
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
/// Drives the Students list page V2. Loads all students via EF Core
/// (IStudentFullViewService) and applies search/filter/sort/paging in memory.
/// Register as Scoped (Phase 10).
/// </summary>
public sealed class StudentsListViewModelV2
{
    private readonly IStudentFullViewService _svc;

    public StudentsListViewModelV2(IStudentFullViewService svc) => _svc = svc;

    // -------------------------------------------------------------------------
    // Full dataset cached after first load
    // -------------------------------------------------------------------------

    private IReadOnlyList<StudentFullViewDto> _all = [];

    /// <summary>
    /// Exposes the full unfiltered dataset so the page can build distinct
    /// filter-option lists from all students, not just the current page.
    /// </summary>
    public IReadOnlyList<StudentFullViewDto> AllItems => _all;

    // -------------------------------------------------------------------------
    // Data state
    // -------------------------------------------------------------------------

    public List<StudentFullViewDto> Items { get; private set; } = [];
    public int TotalCount { get; private set; }
    public int Page { get; private set; } = 1;
    public int PageSize { get; private set; } = 10;
    public string SearchTerm { get; private set; } = string.Empty;
    public string SortColumn { get; private set; } = string.Empty;
    public bool SortAscending { get; private set; } = true;
    public bool IsLoading { get; private set; } = true;
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
        new() { Key = "admissionNumber", Label = "Adm.", Visible = true, Sortable = true, Order = 0, Width = "100px" },
        new() { Key = "firstName",       Label = "First Name", Visible = true, Sortable = true, Order = 1 },
        new() { Key = "lastName",        Label = "Last Name",  Visible = true, Sortable = true, Order = 2 },
        new() { Key = "gender",          Label = "Gender",     Visible = true, Sortable = true, Order = 3, Width = "90px" },
        new() { Key = "status",          Label = "Status",     Visible = true, Sortable = true, Order = 4, Width = "100px" },
        new() { Key = "dateOfBirth",     Label = "DOB",        Visible = true, Sortable = true, Order = 5, DataType = "date", Width = "110px" },
        new() { Key = "yearGroupName",   Label = "Year",       Visible = true, Sortable = true, Order = 6, Width = "70px" },
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
        IEnumerable<StudentFullViewDto> query = _all;

        // Search across key text fields
        if (!string.IsNullOrWhiteSpace(SearchTerm))
        {
            var term = SearchTerm.Trim().ToLowerInvariant();
            query = query.Where(s =>
                s.AdmissionNumber.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                s.FirstName.Contains(term, StringComparison.OrdinalIgnoreCase)       ||
                s.LastName.Contains(term, StringComparison.OrdinalIgnoreCase)        ||
                (s.LegalFirstName ?? "").Contains(term, StringComparison.OrdinalIgnoreCase)  ||
                (s.LegalLastName  ?? "").Contains(term, StringComparison.OrdinalIgnoreCase)   ||
                s.YearGroupName.Contains(term, StringComparison.OrdinalIgnoreCase)   ||
                s.ClassName.Contains(term, StringComparison.OrdinalIgnoreCase)       ||
                (s.HouseName ?? "").Contains(term, StringComparison.OrdinalIgnoreCase));
        }

        // Dropdown filters
        foreach (var (key, value) in _activeFilters.Where(f => !string.IsNullOrWhiteSpace(f.Value)))
        {
            query = key switch
            {
                "admission_number" => query.Where(s => s.AdmissionNumber == value),
                "first_name"       => query.Where(s => s.FirstName == value),
                "last_name"        => query.Where(s => s.LastName == value),
                "gender"           => query.Where(s => s.Gender == value),
                "date_of_birth"    => query.Where(s => s.DateOfBirth.ToString("dd/MM/yyyy") == value),
                "year_group"       => query.Where(s => s.YearGroupName == value),
                "class_name"       => query.Where(s => s.ClassName == value),
                "house_name"       => query.Where(s => s.HouseName == value),
                "admission_date"   => query.Where(s => s.AdmissionDate.ToString("dd/MM/yyyy") == value),
                "status"           => query.Where(s => s.Status == value),
                _                  => query
            };
        }

        // Sort
        query = SortColumn switch
        {
            "admission_number" => SortAscending ? query.OrderBy(s => s.AdmissionNumber)    : query.OrderByDescending(s => s.AdmissionNumber),
            "first_name"       => SortAscending ? query.OrderBy(s => s.FirstName)          : query.OrderByDescending(s => s.FirstName),
            "legal_first_name" => SortAscending ? query.OrderBy(s => s.LegalFirstName)     : query.OrderByDescending(s => s.LegalFirstName),
            "legal_last_name"  => SortAscending ? query.OrderBy(s => s.LegalLastName)      : query.OrderByDescending(s => s.LegalLastName),
            "gender"           => SortAscending ? query.OrderBy(s => s.Gender)             : query.OrderByDescending(s => s.Gender),
            "date_of_birth"    => SortAscending ? query.OrderBy(s => s.DateOfBirth)        : query.OrderByDescending(s => s.DateOfBirth),
            "year_group"       => SortAscending ? query.OrderBy(s => s.YearGroupName)      : query.OrderByDescending(s => s.YearGroupName),
            "class_name"       => SortAscending ? query.OrderBy(s => s.ClassName)          : query.OrderByDescending(s => s.ClassName),
            "house_name"       => SortAscending ? query.OrderBy(s => s.HouseName)          : query.OrderByDescending(s => s.HouseName),
            "admission_date"   => SortAscending ? query.OrderBy(s => s.AdmissionDate)      : query.OrderByDescending(s => s.AdmissionDate),
            "status"           => SortAscending ? query.OrderBy(s => s.Status)             : query.OrderByDescending(s => s.Status),
            _                  => query.OrderBy(s => s.LegalLastName)
        };

        var filtered = query.ToList();
        TotalCount = filtered.Count;
        Items = filtered.Skip((Page - 1) * PageSize).Take(PageSize).ToList();
    }
}
