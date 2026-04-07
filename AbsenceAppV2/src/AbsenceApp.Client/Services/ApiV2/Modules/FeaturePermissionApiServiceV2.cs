/*
===============================================================================
 File        : FeaturePermissionApiServiceV2.cs
 Namespace   : AbsenceApp.Client.Services.ApiV2.Modules
 Author      : Michael
 Version     : 2.0.0
 Created     : 2026-04-06
 Updated     : 2026-04-06
-------------------------------------------------------------------------------
 Purpose     : Client-side feature permission service that executes
               dbo.fn_IsFeatureAllowed(@RoleType, @FeatureId) via EF Core
               SqlQueryRaw to determine whether the current user's role may
               access a named feature.

               In MAUI Blazor Hybrid the C# HttpClient cannot reach
               http://localhost/ (that scheme exists only inside the WebView2
               browser context). This service therefore calls the database
               directly using IServiceScopeFactory + AppDbContext instead of
               making an HTTP call to AbsenceApp.Api.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-04-06  Initial implementation (Phase 3 — Feature Permission
                         Boundary). Used HttpClient to call GET /api/features/allowed.
   - 2.0.0  2026-04-06  Breaking fix: replaced HttpClient with direct DB call
                         via IServiceScopeFactory + AppDbContext. HttpClient
                         cannot reach http://localhost/ in MAUI C# context.
                         Now resolves RoleTypeId from Users table, looks up
                         FeatureId from Features table, then calls
                         dbo.fn_IsFeatureAllowed(@RoleType, @FeatureId).
   - 2.1.0  2026-04-07  Debug instrumentation: added AppLog.Write calls at
                         entry (with featureKey, IsAuthenticated, CurrentUserId),
                         after roleTypeId resolution, after feature lookup,
                         after SQL result, and in the catch block.
-------------------------------------------------------------------------------
 Notes       :
   - Registered as Singleton in V2ServiceCollectionExtensions.cs.
   - IServiceScopeFactory used to resolve Scoped AppDbContext from Singleton.
   - Returns false on any error or if the feature key is unknown/inactive.
===============================================================================
*/

using AbsenceApp.Client.Services;
using AbsenceApp.Data.Context;
using AbsenceApp.Data.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AbsenceApp.Client.Services.ApiV2.Modules;

// ===========================================================================
// FeaturePermissionApiServiceV2
// ===========================================================================

public sealed class FeaturePermissionApiServiceV2
{
    // ---------------------------------------------------------------------------
    // Dependencies
    // ---------------------------------------------------------------------------

    private readonly IServiceScopeFactory _scopeFactory;
    private readonly AppStateService      _appState;

    public FeaturePermissionApiServiceV2(
        IServiceScopeFactory scopeFactory,
        AppStateService      appState)
    {
        _scopeFactory = scopeFactory;
        _appState     = appState;
    }

    // ===========================================================================
    // IsAllowedAsync
    // Resolves the current user's RoleTypeId, looks up the feature, and calls
    // dbo.fn_IsFeatureAllowed(@RoleType, @FeatureId).
    // Returns false on any error (fail-safe).
    // ===========================================================================

    public async Task<bool> IsAllowedAsync(string featureKey, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(featureKey))
        {
            AppLog.Write("FeaturePermissionApiServiceV2.cs", "IsAllowedAsync",
                "featureKey is null/empty — returning false immediately");
            return false;
        }

        AppLog.Write("FeaturePermissionApiServiceV2.cs", "IsAllowedAsync",
            $"ENTER IsAllowedAsync featureKey='{featureKey}' IsAuthenticated={_appState.IsAuthenticated} CurrentUserId={_appState.CurrentUserId}");

        try
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // -----------------------------------------------------------------------
            // Resolve current user's RoleTypeId
            // -----------------------------------------------------------------------
            var userId     = _appState.CurrentUserId;
            AppLog.Write("FeaturePermissionApiServiceV2.cs", "IsAllowedAsync",
                $"Resolving RoleTypeId for userId={userId} (CurrentUserId at call-time={_appState.CurrentUserId} IsAuthenticated={_appState.IsAuthenticated})");
            var roleTypeId = await db.Users
                .AsNoTracking()
                .Where(u => u.Id == userId)
                .Select(u => u.RoleTypeId)
                .FirstOrDefaultAsync(ct);
            AppLog.Write("FeaturePermissionApiServiceV2.cs", "IsAllowedAsync",
                $"roleTypeId={roleTypeId} (userId={userId})");

            // -----------------------------------------------------------------------
            // Look up the feature — return false if unknown or inactive
            // -----------------------------------------------------------------------
            var feature = await db.Set<Feature>()
                .AsNoTracking()
                .Where(f => f.Key == featureKey && f.IsActive)
                .Select(f => new { f.FeatureId })
                .FirstOrDefaultAsync(ct);

            if (feature is null)
            {
                AppLog.Write("FeaturePermissionApiServiceV2.cs", "IsAllowedAsync",
                    $"Feature '{featureKey}' not found or inactive — returning false");
                return false;
            }

            AppLog.Write("FeaturePermissionApiServiceV2.cs", "IsAllowedAsync",
                $"Feature found: featureKey='{featureKey}' featureId={feature.FeatureId} — calling fn_IsFeatureAllowed(roleType={roleTypeId}, featureId={feature.FeatureId})");

            // -----------------------------------------------------------------------
            // Call dbo.fn_IsFeatureAllowed(@RoleType, @FeatureId)
            // -----------------------------------------------------------------------
            const string sql = """
                SELECT dbo.fn_IsFeatureAllowed(@RoleType, @FeatureId) AS IsAllowed;
                """;

            var result = await db.Database
                .SqlQueryRaw<FeatureAllowedRow>(sql,
                    new SqlParameter("@RoleType",  (int)roleTypeId),
                    new SqlParameter("@FeatureId", feature.FeatureId))
                .FirstOrDefaultAsync(ct);

            var allowed = result?.IsAllowed ?? false;
            AppLog.Write("FeaturePermissionApiServiceV2.cs", "IsAllowedAsync",
                $"fn_IsFeatureAllowed result={result?.IsAllowed} — returning {allowed} for featureKey='{featureKey}' userId={userId} roleTypeId={roleTypeId}");
            return allowed;
        }
        catch (Exception ex)
        {
            AppLog.Write("FeaturePermissionApiServiceV2.cs", "IsAllowedAsync",
                $"ERROR {ex.GetType().Name}: {ex.Message} — returning false for featureKey='{featureKey}' userId={_appState.CurrentUserId}");
            return false;
        }
    }

    // ---------------------------------------------------------------------------
    // Private projection record for the scalar SQL result
    // ---------------------------------------------------------------------------

    private sealed class FeatureAllowedRow
    {
        public bool IsAllowed { get; set; }
    }
}

