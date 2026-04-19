/*
===============================================================================
 File        : FeaturePermissionResolver.cs
 Namespace   : AbsenceApp.Api.Services.Navigation
 Author      : Michael
 Version     : 3.0.0
 Created     : 2026-04-06
 Updated     : 2026-04-19
-------------------------------------------------------------------------------
 Purpose     : Determines whether a given role may access a named feature.

               The API endpoint accepts a feature code (string), verifies the
               feature is enabled in the feature table, then queries the
               rolefeature table directly via EF Core LINQ.
               Returns false if the feature code does not exist or is disabled.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-04-06  Initial implementation (Phase 3 — Feature Permission
                         Boundary).
   - 2.0.0  2026-04-19  MySQL migration: removed fn_IsFeatureAllowed SQL
                         function call and MySqlConnector dependency. Replaced
                         with pure EF Core LINQ query on RoleFeature table
                         (RoleType + FeatureId + IsAllowed).
   - 3.0.0  2026-04-19  Schema alignment: updated field names to match CSV.
                         Feature.Key→Code, Feature.IsActive→IsEnabled,
                         RoleFeature.RoleType→RoleId, RoleFeature.FeatureId→
                         FeatureCode (string), RoleFeature.IsAllowed→IsEnabled.
                         Table feature (was features), rolefeature (was role_features).
-------------------------------------------------------------------------------
 Notes       :
   - Registered as Scoped in Program.cs.
===============================================================================
*/

using AbsenceApp.Data.Context;
using AbsenceApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AbsenceApp.Api.Services.Navigation;

// ===========================================================================
// IFeaturePermissionResolver
// ===========================================================================

public interface IFeaturePermissionResolver
{
    Task<bool> IsAllowedAsync(int roleType, string featureKey, CancellationToken ct = default);
}

// ===========================================================================
// FeaturePermissionResolver
// ===========================================================================

public sealed class FeaturePermissionResolver : IFeaturePermissionResolver
{
    // ---------------------------------------------------------------------------
    // Dependencies
    // ---------------------------------------------------------------------------

    private readonly AppDbContext _db;

    public FeaturePermissionResolver(AppDbContext db)
    {
        _db = db;
    }

    // ===========================================================================
    // IsAllowedAsync
    // Verifies the feature is enabled, then queries rolefeature via EF Core LINQ.
    // ===========================================================================

    public async Task<bool> IsAllowedAsync(
        int roleType,
        string featureKey,
        CancellationToken ct = default)
    {
        // -----------------------------------------------------------------------
        // Verify feature exists and is enabled
        // -----------------------------------------------------------------------
        var featureEnabled = await _db.Set<Feature>()
            .AsNoTracking()
            .AnyAsync(f => f.Code == featureKey && f.IsEnabled, ct);

        if (!featureEnabled)
            return false;

        // -----------------------------------------------------------------------
        // Query rolefeature table directly — no SQL function required
        // -----------------------------------------------------------------------
        return await _db.Set<RoleFeature>()
            .AsNoTracking()
            .AnyAsync(rf =>
                rf.RoleId       == roleType &&
                rf.FeatureCode  == featureKey &&
                rf.IsEnabled,
                ct);
    }
}
