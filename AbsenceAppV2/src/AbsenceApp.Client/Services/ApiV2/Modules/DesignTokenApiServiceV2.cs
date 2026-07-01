/*
===============================================================================
 File        : DesignTokenApiServiceV2.cs
 Namespace   : AbsenceApp.Client.Services.ApiV2.Modules
 Author      : Michael
 Version     : 1.1.1
 Created     : 2026-05-12
 Updated     : 2026-06-03
-------------------------------------------------------------------------------
 Purpose     : Singleton service that manages runtime design token state and
               generates a CSS :root { } override block from the DesignTokens
               table.

               Phase 4.1 update:
                 - ComponentGroup removed from the domain model.
                 - Component (domain) now maps to ComponentGroup (DB).
                 - All queries updated to use Component.
                 - Ordering updated to Component ASC, SortOrder ASC.

               CSS generation contract:
                 - Only active tokens (IsActive = true) are emitted.
                 - Rows are ordered by Component ASC, SortOrder ASC.
                 - Each row emits one CSS declaration using CurrentValue when
                   non-null, otherwise DefaultValue.
                 - Output is wrapped in a single :root { } block.

               The generated CSS string is cached in memory (_cssCache).
               Any write operation calls InvalidateCache() then fires
               OnTokensChanged so all subscribed components re-render.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-05-12  Initial creation (Phase A — Design Token System).
   - 1.1.0  2026-06-03  Phase 4.1 — Replaced ComponentGroup with Component,
                        updated ordering, updated group/reset queries, and
                        aligned with new EF configuration.
   - 1.1.1  2026-06-24  Added Enabled flag to active-token filter in
                        LoadActiveTokensAsync (IsActive && Enabled).
-------------------------------------------------------------------------------
 Notes       :
   - IDesignTokenApiServiceV2 interface is intentionally omitted in Phase A;
     a concrete singleton is sufficient until a test-double is required.
   - All DB writes use SaveChangesAsync inside a short-lived scope.
   - OnTokensChanged is a plain Action? to avoid IAsyncDisposable complexity
     in consumer components that only need StateHasChanged.
===============================================================================
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AbsenceApp.Data.Context;
using AbsenceApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AbsenceApp.Client.Services.ApiV2.Modules;

public sealed class DesignTokenApiServiceV2
{
    private readonly IServiceScopeFactory _scopeFactory;

    public DesignTokenApiServiceV2(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    private string? _cssCache;
    private readonly object _cacheLock = new();

    public event Action? OnTokensChanged;

    // =========================================================================
    // GetGeneratedCssAsync
    // =========================================================================

    public async Task<string> GetGeneratedCssAsync()
    {
        string? cached;
        lock (_cacheLock)
        {
            cached = _cssCache;
        }

        if (cached is not null)
            return cached;

        var tokens = await LoadActiveTokensAsync();
        var css    = BuildCss(tokens);

        lock (_cacheLock)
        {
            _cssCache = css;
        }

        return css;
    }

    // =========================================================================
    // GetGroupAsync
    // =========================================================================

    public async Task<List<DesignToken>> GetGroupAsync(string component)
    {
        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        return await db.DesignTokens
            .Where(t => t.Component == component)
            .OrderBy(t => t.SortOrder)
            .ToListAsync();
    }

    // =========================================================================
    // UpdateTokensAsync
    // =========================================================================

    public async Task UpdateTokensAsync(Dictionary<string, string?> updates)
    {
        if (updates is null || updates.Count == 0)
            return;

        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var cssVariables = updates.Keys.ToList();
        var tokens = await db.DesignTokens
            .Where(t => cssVariables.Contains(t.CssVariable))
            .ToListAsync();

        foreach (var token in tokens)
        {
            if (updates.TryGetValue(token.CssVariable, out var newValue))
            {
                token.CurrentValue = newValue;
                token.UpdatedAt    = DateTime.UtcNow;
            }
        }

        await db.SaveChangesAsync();

        InvalidateCache();
        OnTokensChanged?.Invoke();
    }

    // =========================================================================
    // ResetGroupAsync
    // =========================================================================

    public async Task ResetGroupAsync(string component)
    {
        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var tokens = await db.DesignTokens
            .Where(t => t.Component == component && t.CurrentValue != null)
            .ToListAsync();

        foreach (var token in tokens)
        {
            token.CurrentValue = null;
            token.UpdatedAt    = DateTime.UtcNow;
        }

        await db.SaveChangesAsync();

        InvalidateCache();
        OnTokensChanged?.Invoke();
    }

    // =========================================================================
    // InvalidateCache
    // =========================================================================

    public void InvalidateCache()
    {
        lock (_cacheLock)
        {
            _cssCache = null;
        }
    }

    // =========================================================================
    // Private helpers
    // =========================================================================

    private async Task<List<DesignToken>> LoadActiveTokensAsync()
    {
        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        return await db.DesignTokens
            .Where(t => t.IsActive && t.Enabled)
            .OrderBy(t => t.Component)
            .ThenBy(t => t.SortOrder)
            .ToListAsync();
    }

    private static string BuildCss(List<DesignToken> tokens)
    {
        if (tokens.Count == 0)
            return string.Empty;

        var sb = new StringBuilder();
        sb.AppendLine(":root {");

        foreach (var token in tokens)
        {
            var value = token.CurrentValue ?? token.DefaultValue;
            sb.AppendLine($"  {token.CssVariable}: {value};");
        }

        sb.Append('}');
        return sb.ToString();
    }
}
