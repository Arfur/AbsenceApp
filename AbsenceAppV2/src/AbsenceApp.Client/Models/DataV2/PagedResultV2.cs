/*
===============================================================================
 File        : PagedResultV2.cs
 Namespace   : AbsenceApp.Client.Models.DataV2
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-21
 Updated     : 2026-03-21
-------------------------------------------------------------------------------
 Purpose     : Paged result envelope returned by list endpoints. Carries Items collection, TotalCount, CurrentPage, and PageSize to support PaginationV2.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-21  Initial implementation.
-------------------------------------------------------------------------------
 Notes       :
   - Phase 6 model. No DI registration required.
===============================================================================
*/

namespace AbsenceApp.Client.Models.DataV2;

/// <summary>
/// Represents a single page of results returned by a paginated API endpoint.
/// Used with ApiResponseV2{T} where T = PagedResultV2{TItem}.
/// </summary>
/// <typeparam name="TItem">The item type in the page.</typeparam>
public sealed class PagedResultV2<TItem>
{
    /// <summary>The items in the current page.</summary>
    public List<TItem> Items { get; init; } = [];

    /// <summary>Total number of records across all pages.</summary>
    public int TotalCount { get; init; }

    /// <summary>Current one-based page number.</summary>
    public int PageNumber { get; init; }

    /// <summary>Maximum items per page.</summary>
    public int PageSize { get; init; }

    /// <summary>Total number of pages: Ceiling(TotalCount / PageSize).</summary>
    public int TotalPages => PageSize > 0
        ? (int)Math.Ceiling((double)TotalCount / PageSize)
        : 0;

    /// <summary>True when there is a page before the current one.</summary>
    public bool HasPreviousPage => PageNumber > 1;

    /// <summary>True when there is a page after the current one.</summary>
    public bool HasNextPage => PageNumber < TotalPages;

    // -------------------------------------------------------------------------

    // Factory

    // -------------------------------------------------------------------------
    /// <summary>Creates an empty paged result (e.g. for error recovery).</summary>
    public static PagedResultV2<TItem> Empty() =>
        new() { Items = [], TotalCount = 0, PageNumber = 1, PageSize = 10 };

    /// <summary>Creates a single-page result containing all provided items.</summary>
    public static PagedResultV2<TItem> SinglePage(IEnumerable<TItem> items)
    {
        var list = items.ToList();
        return new()
        {
            Items = list,
            TotalCount = list.Count,
            PageNumber = 1,
            PageSize = list.Count > 0 ? list.Count : 10,
        };
    }
}
