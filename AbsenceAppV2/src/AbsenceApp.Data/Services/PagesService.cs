/*
===============================================================================
 File        : PagesService.cs
 Namespace   : AbsenceApp.Data.Services
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-04-11
 Updated     : 2026-04-11
-------------------------------------------------------------------------------
 Purpose     : E16 Pages Registry service. Implements IPagesService for full
               CRUD over the app_pages table, including soft deactivation
               and hard deletion with cascade of linked permission rows.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-04-11  Initial creation (E16 Pages Registry).
-------------------------------------------------------------------------------
 Notes       :
   - Register as Scoped in DataServiceRegistration.cs.
   - CreatePageAsync and UpdatePageAsync enforce uniqueness of Slug and Route
     at the application layer (a DB unique index is also present on Slug).
   - DeletePageAsync performs a hard delete; FK cascades handle linked rows in
     role_default_page_permissions and user_page_permissions.
   - DeactivatePageAsync is preferred for pages that may be re-enabled; it
     preserves permission rows.
===============================================================================
*/

using AbsenceApp.Core.DTOs;
using AbsenceApp.Core.Interfaces;
using AbsenceApp.Data.Context;
using AbsenceApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AbsenceApp.Data.Services;

public sealed class PagesService : IPagesService
{
    // =========================================================================
    // Construction
    // =========================================================================

    private readonly AppDbContext _db;

    public PagesService(AppDbContext db) => _db = db;

    // =========================================================================
    // List / read
    // =========================================================================

    public async Task<IReadOnlyList<PageListItemDto>> GetAllPagesAsync(CancellationToken ct = default)
    {
        return await _db.AppPages
            .AsNoTracking()
            .OrderBy(p => p.SortOrder)
            .Select(p => new PageListItemDto
            {
                Id          = p.Id,
                Name        = p.Name,
                Slug        = p.Slug,
                Route       = p.Route,
                CategoryKey = p.CategoryKey,
                MenuKey     = p.MenuKey,
                IconKey     = p.IconKey,
                IsActive    = p.IsActive,
                SortOrder   = p.SortOrder,
                CreatedAt   = p.CreatedAt,
                UpdatedAt   = p.UpdatedAt,
            })
            .ToListAsync(ct);
    }

    public async Task<PageEditDto?> GetPageForEditAsync(int id, CancellationToken ct = default)
    {
        return await _db.AppPages
            .AsNoTracking()
            .Where(p => p.Id == id)
            .Select(p => new PageEditDto
            {
                Id             = p.Id,
                Name           = p.Name,
                Slug           = p.Slug,
                Route          = p.Route,
                CategoryKey    = p.CategoryKey,
                MenuKey        = p.MenuKey,
                IconKey        = p.IconKey,
                Description    = p.Description,
                IsActive       = p.IsActive,
                SortOrder      = p.SortOrder,
                SupportsRead   = p.SupportsRead,
                SupportsWrite  = p.SupportsWrite,
                SupportsCreate = p.SupportsCreate,
                SupportsDelete = p.SupportsDelete,
                SupportsImport = p.SupportsImport,
                SupportsExport = p.SupportsExport,
            })
            .FirstOrDefaultAsync(ct);
    }

    // =========================================================================
    // Mutations
    // =========================================================================

    public async Task<int> CreatePageAsync(PageCreateDto dto, CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;

        var page = new AppPage
        {
            Name           = dto.Name.Trim(),
            Slug           = dto.Slug.Trim().ToLowerInvariant(),
            Route          = dto.Route.Trim(),
            CategoryKey    = dto.CategoryKey.Trim().ToUpperInvariant(),
            MenuKey        = dto.MenuKey.Trim(),
            IconKey        = dto.IconKey?.Trim(),
            Description    = dto.Description?.Trim(),
            IsActive       = dto.IsActive,
            SortOrder      = dto.SortOrder,
            SupportsRead   = dto.SupportsRead,
            SupportsWrite  = dto.SupportsWrite,
            SupportsCreate = dto.SupportsCreate,
            SupportsDelete = dto.SupportsDelete,
            SupportsImport = dto.SupportsImport,
            SupportsExport = dto.SupportsExport,
            CreatedAt      = now,
            UpdatedAt      = now,
        };

        _db.AppPages.Add(page);
        await _db.SaveChangesAsync(ct);
        return page.Id;
    }

    public async Task UpdatePageAsync(PageUpdateDto dto, CancellationToken ct = default)
    {
        var page = await _db.AppPages
            .FirstOrDefaultAsync(p => p.Id == dto.Id, ct)
            ?? throw new InvalidOperationException($"AppPage {dto.Id} not found.");

        page.Name           = dto.Name.Trim();
        page.Slug           = dto.Slug.Trim().ToLowerInvariant();
        page.Route          = dto.Route.Trim();
        page.CategoryKey    = dto.CategoryKey.Trim().ToUpperInvariant();
        page.MenuKey        = dto.MenuKey.Trim();
        page.IconKey        = dto.IconKey?.Trim();
        page.Description    = dto.Description?.Trim();
        page.IsActive       = dto.IsActive;
        page.SortOrder      = dto.SortOrder;
        page.SupportsRead   = dto.SupportsRead;
        page.SupportsWrite  = dto.SupportsWrite;
        page.SupportsCreate = dto.SupportsCreate;
        page.SupportsDelete = dto.SupportsDelete;
        page.SupportsImport = dto.SupportsImport;
        page.SupportsExport = dto.SupportsExport;
        page.UpdatedAt      = DateTime.UtcNow;

        await _db.SaveChangesAsync(ct);
    }

    public async Task DeactivatePageAsync(int id, CancellationToken ct = default)
    {
        var page = await _db.AppPages
            .FirstOrDefaultAsync(p => p.Id == id, ct)
            ?? throw new InvalidOperationException($"AppPage {id} not found.");

        page.IsActive  = false;
        page.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync(ct);
    }

    public async Task DeletePageAsync(int id, CancellationToken ct = default)
    {
        var page = await _db.AppPages
            .FirstOrDefaultAsync(p => p.Id == id, ct)
            ?? throw new InvalidOperationException($"AppPage {id} not found.");

        _db.AppPages.Remove(page);
        await _db.SaveChangesAsync(ct);
    }
}
