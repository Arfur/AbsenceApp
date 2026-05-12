/*
===============================================================================
 File        : DesignTokenApiServiceV2.cs
 Namespace   : AbsenceApp.Client.Services.ApiV2.Modules
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-05-12
 Updated     : 2026-05-12
-------------------------------------------------------------------------------
 Purpose     : Singleton service that manages runtime design token state and
               generates a CSS :root { } override block from the DesignTokens
               table.

               Registered as a Singleton; uses IServiceScopeFactory to resolve
               the Scoped AppDbContext for all DB operations.

               CSS generation contract:
                 - Only active tokens (IsActive = true) are emitted.
                 - Rows are ordered by ComponentGroup ASC, SortOrder ASC.
                 - Each row emits one CSS declaration using CurrentValue when
                   non-null, otherwise DefaultValue.
                 - Output is wrapped in a single :root { } block.

               The generated CSS string is cached in memory (_cssCache).
               Any write operation calls InvalidateCache() then fires
               OnTokensChanged so all subscribed components re-render.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-05-12  Initial creation (Phase A — Design Token System).
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

/// <summary>
/// Singleton service that loads design tokens from the database, generates a
/// CSS :root override block, and notifies subscribers when tokens change.
/// </summary>
public sealed class DesignTokenApiServiceV2
{
    // =========================================================================
    // Dependencies
    // =========================================================================

    private readonly IServiceScopeFactory _scopeFactory;

    public DesignTokenApiServiceV2(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    // =========================================================================
    // State
    // =========================================================================

    /// <summary>Cached CSS string. Null means the cache is dirty.</summary>
    private string? _cssCache;

    /// <summary>Lock object for thread-safe cache access.</summary>
    private readonly object _cacheLock = new();

    // =========================================================================
    // Events
    // =========================================================================

    /// <summary>
    /// Raised after any token update or reset that invalidates the CSS cache.
    /// Subscribers should re-fetch the CSS and call StateHasChanged.
    /// </summary>
    public event Action? OnTokensChanged;

    // =========================================================================
    // GetGeneratedCssAsync
    // Returns the :root { } CSS block for all active tokens.
    // Uses the in-memory cache; queries the DB on first call or after
    // InvalidateCache().
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
    // Returns all tokens for a specific component group (active and inactive).
    // =========================================================================

    public async Task<List<DesignToken>> GetGroupAsync(string componentGroup)
    {
        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        return await db.DesignTokens
            .Where(t => t.ComponentGroup == componentGroup)
            .OrderBy(t => t.SortOrder)
            .ToListAsync();
    }

    // =========================================================================
    // UpdateTokensAsync
    // Accepts a dictionary of { CssVariable → newValue } pairs and persists
    // CurrentValue for each matched token.  Pass null as the value to clear
    // the override and revert to DefaultValue.
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
    // Clears all CurrentValue overrides for the specified component group,
    // reverting every token to its DefaultValue.
    // =========================================================================

    public async Task ResetGroupAsync(string componentGroup)
    {
        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var tokens = await db.DesignTokens
            .Where(t => t.ComponentGroup == componentGroup && t.CurrentValue != null)
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
    // Marks the CSS cache as dirty so the next GetGeneratedCssAsync() call
    // re-queries the database.
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

    /// <summary>
    /// Queries the DB for all active tokens ordered by ComponentGroup ASC,
    /// SortOrder ASC.
    /// </summary>
    private async Task<List<DesignToken>> LoadActiveTokensAsync()
    {
        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        return await db.DesignTokens
            .Where(t => t.IsActive)
            .OrderBy(t => t.ComponentGroup)
            .ThenBy(t => t.SortOrder)
            .ToListAsync();
    }

    /// <summary>
    /// Builds the CSS :root { } block from a list of token rows.
    /// Uses CurrentValue when non-null, otherwise DefaultValue.
    /// </summary>
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
