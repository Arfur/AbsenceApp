/*
===============================================================================
 File        : FeaturePermissionApiServiceV2.cs
 Namespace   : AbsenceApp.Client.Services.ApiV2.Modules
 Author      : Michael
 Version     : 5.0.0
 Created     : 2026-04-06
 Updated     : 2026-04-19
-------------------------------------------------------------------------------
 Purpose     : Client-side feature permission service for AbsenceApp V2.
               Originally executed SQL Server function dbo.fn_IsFeatureAllowed
               via SqlQueryRaw. As of v3.0.0 the service no longer uses any
               Microsoft.Data.SqlClient or SQL Server-specific APIs. The
               permission check is now performed using the MySQL-backed EF Core
               rolefeature table (FeatureCode + RoleId + IsEnabled).

               In MAUI Blazor Hybrid the C# HttpClient cannot reach
               http://localhost/ (that scheme exists only inside the WebView2
               browser context). This service therefore resolves permissions
               directly using IServiceScopeFactory + AppDbContext instead of
               making an HTTP call to AbsenceApp.Api.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-04-06  Initial implementation (Phase 3 — Feature Permission
                         Boundary). Used HttpClient to call GET /api/features/allowed.

   - 2.0.0  2026-04-06  Breaking fix: replaced HttpClient with direct DB call
                         via IServiceScopeFactory + AppDbContext. HttpClient
                         cannot reach http://localhost/ in MAUI C# context.
                         Used SQL Server function dbo.fn_IsFeatureAllowed.

   - 2.1.0  2026-04-07  Debug instrumentation: added AppLog.Write calls at
                         entry, after roleTypeId resolution, after feature
                         lookup, after SQL result, and in the catch block.

   - 3.0.0  2026-04-19  Major cleanup: removed all Microsoft.Data.SqlClient and
                         SQL Server-specific dependencies. Migrated to MySQL-
                         compatible EF Core logic using rolefeature table
                         (RoleId + FeatureCode + IsEnabled). Eliminated
                         SqlQueryRaw and dbo.fn_IsFeatureAllowed usage.
   - 5.0.0  2026-04-19  RoleId resolution fix: replaced users.RoleTypeId lookup
                         with raw SQL through userrole → roles chain. rolefeature
                         check now uses RoleId from userrole, not RoleTypeId
                         from users. Eliminates "Unknown column 'u.RoleTypeId'"
                         startup crash.
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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MySqlConnector;

namespace AbsenceApp.Client.Services.ApiV2.Modules;

// ===========================================================================
// FeaturePermissionApiServiceV2
// ===========================================================================

public sealed class FeaturePermissionApiServiceV2
{
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
    // IsAllowedAsync (MySQL version)
    // Uses RoleFeature table instead of SQL Server fn_IsFeatureAllowed
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
            // Resolve current user's RoleId via userrole table
            // -----------------------------------------------------------------------
            var userId = _appState.CurrentUserId;

            AppLog.Write("FeaturePermissionApiServiceV2.cs", "IsAllowedAsync",
                $"Resolving RoleId for userId={userId}");

            var roleIds = await db.Database
                .SqlQueryRaw<int>(
                    "SELECT RoleId FROM userrole WHERE UserId = @UserId LIMIT 1",
                    new MySqlParameter("@UserId", userId))
                .ToListAsync(ct);
            var roleId = roleIds.FirstOrDefault();

            AppLog.Write("FeaturePermissionApiServiceV2.cs", "IsAllowedAsync",
                $"roleId={roleId} (userId={userId})");

            // -----------------------------------------------------------------------
            // Look up the feature — return false if unknown or disabled
            // -----------------------------------------------------------------------
            var featureEnabled = await db.Set<Feature>()
                .AsNoTracking()
                .AnyAsync(f => f.Code == featureKey && f.IsEnabled, ct);

            if (!featureEnabled)
            {
                AppLog.Write("FeaturePermissionApiServiceV2.cs", "IsAllowedAsync",
                    $"Feature '{featureKey}' not found or disabled — returning false");
                return false;
            }

            AppLog.Write("FeaturePermissionApiServiceV2.cs", "IsAllowedAsync",
                $"Feature found and enabled: featureKey='{featureKey}'");

            // -----------------------------------------------------------------------
            // MySQL permission check via rolefeature table
            // -----------------------------------------------------------------------
            var allowed = await db.Set<RoleFeature>()
                .AsNoTracking()
                .AnyAsync(rf =>
                    rf.RoleId       == roleId &&
                    rf.FeatureCode  == featureKey &&
                    rf.IsEnabled,
                    ct);

            AppLog.Write("FeaturePermissionApiServiceV2.cs", "IsAllowedAsync",
                $"rolefeature lookup result={allowed} — returning {allowed} for featureKey='{featureKey}' userId={userId} roleId={roleId}");

            return allowed;
        }
        catch (Exception ex)
        {
            AppLog.Write("FeaturePermissionApiServiceV2.cs", "IsAllowedAsync",
                $"ERROR {ex.GetType().Name}: {ex.Message} — returning false for featureKey='{featureKey}' userId={_appState.CurrentUserId}");
            return false;
        }
    }
}
