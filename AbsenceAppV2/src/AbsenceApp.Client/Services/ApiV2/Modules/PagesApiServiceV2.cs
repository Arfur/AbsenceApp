/*
===============================================================================
 File        : PagesApiServiceV2.cs
 Namespace   : AbsenceApp.Client.Services.ApiV2.Modules
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-04-11
 Updated     : 2026-04-11
-------------------------------------------------------------------------------
 Purpose     : Client-side Pages Registry API service. Delegates to
               IPagesService via EF Core direct DB access (IServiceScopeFactory
               pattern) — HTTP is not available in MAUI Blazor Hybrid C# layer.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-04-11  Initial creation (E16 Pages Registry).
-------------------------------------------------------------------------------
 Notes       :
   - Registered as Scoped in V2ServiceCollectionExtensions.cs.
   - Returns false / null / empty on any error (fail-safe pattern).
===============================================================================
*/

using AbsenceApp.Client.Shared;
using AbsenceApp.Core.DTOs;
using AbsenceApp.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace AbsenceApp.Client.Services.ApiV2.Modules;

public sealed class PagesApiServiceV2
{
    // =========================================================================
    // Dependencies
    // =========================================================================

    private readonly IServiceScopeFactory _scopeFactory;

    public PagesApiServiceV2(IServiceScopeFactory scopeFactory)
        => _scopeFactory = scopeFactory;

    // =========================================================================
    // List / read
    // =========================================================================

    public async Task<IReadOnlyList<PageListItemDto>> GetAllPagesAsync(CancellationToken ct = default)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var svc = scope.ServiceProvider.GetRequiredService<IPagesService>();
            return await svc.GetAllPagesAsync(ct);
        }
        catch (Exception ex)
        {
            AppLog.Write("PagesApiServiceV2.cs", "GetAllPagesAsync", $"ERROR {ex.Message}");
            return [];
        }
    }

    public async Task<PageEditDto?> GetPageForEditAsync(int id, CancellationToken ct = default)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var svc = scope.ServiceProvider.GetRequiredService<IPagesService>();
            return await svc.GetPageForEditAsync(id, ct);
        }
        catch (Exception ex)
        {
            AppLog.Write("PagesApiServiceV2.cs", "GetPageForEditAsync", $"ERROR {ex.Message}");
            return null;
        }
    }

    // =========================================================================
    // Mutations
    // =========================================================================

    public async Task<(bool Success, string? Error, int PageId)> CreatePageAsync(
        PageCreateDto dto, CancellationToken ct = default)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var svc = scope.ServiceProvider.GetRequiredService<IPagesService>();
            var id  = await svc.CreatePageAsync(dto, ct);
            return (true, null, id);
        }
        catch (Exception ex)
        {
            AppLog.Write("PagesApiServiceV2.cs", "CreatePageAsync", $"ERROR {ex.Message}");
            return (false, ex.Message, 0);
        }
    }

    public async Task<(bool Success, string? Error)> UpdatePageAsync(
        PageUpdateDto dto, CancellationToken ct = default)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var svc = scope.ServiceProvider.GetRequiredService<IPagesService>();
            await svc.UpdatePageAsync(dto, ct);
            return (true, null);
        }
        catch (Exception ex)
        {
            AppLog.Write("PagesApiServiceV2.cs", "UpdatePageAsync", $"ERROR {ex.Message}");
            return (false, ex.Message);
        }
    }

    public async Task<(bool Success, string? Error)> DeactivatePageAsync(
        int id, CancellationToken ct = default)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var svc = scope.ServiceProvider.GetRequiredService<IPagesService>();
            await svc.DeactivatePageAsync(id, ct);
            return (true, null);
        }
        catch (Exception ex)
        {
            AppLog.Write("PagesApiServiceV2.cs", "DeactivatePageAsync", $"ERROR {ex.Message}");
            return (false, ex.Message);
        }
    }

    public async Task<(bool Success, string? Error)> DeletePageAsync(
        int id, CancellationToken ct = default)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var svc = scope.ServiceProvider.GetRequiredService<IPagesService>();
            await svc.DeletePageAsync(id, ct);
            return (true, null);
        }
        catch (Exception ex)
        {
            AppLog.Write("PagesApiServiceV2.cs", "DeletePageAsync", $"ERROR {ex.Message}");
            return (false, ex.Message);
        }
    }
}
