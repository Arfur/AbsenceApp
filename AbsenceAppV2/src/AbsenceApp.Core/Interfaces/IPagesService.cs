/*
===============================================================================
 File        : IPagesService.cs
 Namespace   : AbsenceApp.Core.Interfaces
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-04-11
 Updated     : 2026-04-11
-------------------------------------------------------------------------------
 Purpose     : Service contract for the E16 Pages Registry admin module.
               Supports full CRUD over the app_pages table, including soft
               deactivation and hard deletion with cascade of permission rows.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-04-11  Initial creation (E16 Pages Registry).
-------------------------------------------------------------------------------
 Notes       :
   - Implemented by PagesService in AbsenceApp.Data.Services.
   - Register as Scoped (EF Core DbContext dependency).
   - DeletePageAsync hard-deletes the row; any linked
     RoleDefaultPagePermission / UserPagePermission rows are cascade-deleted
     by the FK constraint.
   - DeactivatePageAsync sets IsActive = false — page disappears from
     permission matrix; permission rows are retained for re-activation.
===============================================================================
*/
using AbsenceApp.Core.DTOs;

namespace AbsenceApp.Core.Interfaces;

public interface IPagesService
{
    // ── List / read ───────────────────────────────────────────────────────────

    /// <summary>Returns ALL pages (active and inactive) ordered by SortOrder.</summary>
    Task<IReadOnlyList<PageListItemDto>> GetAllPagesAsync(CancellationToken ct = default);

    /// <summary>Returns the full editable field set for a single page, or null if not found.</summary>
    Task<PageEditDto?> GetPageForEditAsync(int id, CancellationToken ct = default);

    // ── Mutations ─────────────────────────────────────────────────────────────

    /// <summary>Inserts a new page and returns its generated Id.</summary>
    Task<int> CreatePageAsync(PageCreateDto dto, CancellationToken ct = default);

    /// <summary>Updates all editable fields of an existing page.</summary>
    Task UpdatePageAsync(PageUpdateDto dto, CancellationToken ct = default);

    /// <summary>
    /// Sets IsActive = false. The page disappears from the permission matrix
    /// for all users but its configured permissions are preserved.
    /// </summary>
    Task DeactivatePageAsync(int id, CancellationToken ct = default);

    /// <summary>
    /// Hard-deletes the page row. Linked permission rows are cascade-deleted
    /// by the FK constraint. Use with caution.
    /// </summary>
    Task DeletePageAsync(int id, CancellationToken ct = default);
}
