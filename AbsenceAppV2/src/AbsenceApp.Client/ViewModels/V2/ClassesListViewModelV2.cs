/*
===============================================================================
 File        : ClassesListViewModelV2.cs
 Namespace   : AbsenceApp.Client.ViewModels.V2
 Author      : Michael
 Version     : 1.2.0
 Created     : 2026-03-21
 Updated     : 2026-04-06
-------------------------------------------------------------------------------
 Purpose     : ViewModel for ClassesListPageV2. Manages paged class data,
               search, sort, and the column schema for the V2 table component.
               Uses IClassService (EF Core direct) for data access — required
               because HTTP calls are unreachable from native C# in MAUI
               Blazor Hybrid context.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-21  Initial implementation (Phase 7).
   - 1.2.0  2026-04-06  Phase 3 V1 Parity Issue 4: replaced ClassesApiServiceV2
                        (HTTP) with IClassService (EF Core direct).
                        Implemented in-memory search, sort, and paging.
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
/// Drives the Classes list page V2. Loads all classes via EF Core
/// (IClassService) and applies search/sort/paging in memory.
/// Register as Scoped (Phase 10).
/// </summary>
public sealed class ClassesListViewModelV2
{
    private readonly IClassService _svc;

    public ClassesListViewModelV2(IClassService svc) => _svc = svc;

    // -------------------------------------------------------------------------
    // Full dataset cached after first load
    // -------------------------------------------------------------------------

    private IReadOnlyList<ClassDto> _all = [];

    // -------------------------------------------------------------------------
    // Data state
    // -------------------------------------------------------------------------

    public List<ClassDto> Items { get; private set; } = [];
    public int TotalCount { get; private set; }
    public int Page { get; private set; } = 1;
    public int PageSize { get; private set; } = 10;
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
        new() { Key = "name",        Label = "Class Name",  Visible = true, Sortable = true,  Order = 0 },
        new() { Key = "description", Label = "Description", Visible = true, Sortable = false, Order = 1 },
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
            _all = (await _svc.GetAllAsync()).ToList();
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

    // -------------------------------------------------------------------------
    // In-memory view computation
    // -------------------------------------------------------------------------

    private void ApplyView()
    {
        IEnumerable<ClassDto> query = _all;

        // Search across Name and Description
        if (!string.IsNullOrWhiteSpace(SearchTerm))
        {
            var term = SearchTerm.Trim();
            query = query.Where(c =>
                c.Name.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                (c.Description ?? "").Contains(term, StringComparison.OrdinalIgnoreCase));
        }

        // Sort
        query = SortColumn switch
        {
            "name" => SortAscending ? query.OrderBy(c => c.Name) : query.OrderByDescending(c => c.Name),
            _      => query.OrderBy(c => c.Name)
        };

        var list = query.ToList();
        TotalCount = list.Count;
        Items = list.Skip((Page - 1) * PageSize).Take(PageSize).ToList();
    }
}

