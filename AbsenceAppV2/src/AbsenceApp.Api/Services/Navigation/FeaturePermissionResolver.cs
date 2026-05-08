/*
===============================================================================
 File        : FeaturePermissionResolver.cs
 Namespace   : AbsenceApp.Api.Services.Navigation
 Author      : Michael
 Version     : 5.0.0
 Created     : 2026-04-06
 Updated     : 2026-05-07
-------------------------------------------------------------------------------
 Purpose     : Determines whether a given role may access a named feature.

               The API endpoint accepts a feature code (string), verifies the
               feature is enabled in the features table, then queries the
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

   - 4.0.0  2026-04-28  Database rename: underlying table renamed from
                         `feature` → `features`. Entity class remains `Feature`.
                         Updated documentation and comments to reflect the new
                         table name. No behavioural changes.

   - 5.0.0  2026-05-07  Role schema consolidation: replaced legacy int roleType
                         parameter with string roleCode. Feature permission
                         resolution now joins RoleFeature→Role and filters by
                         Role.Code (e.g., 'SUPERADMIN', 'ADMIN', 'USER').
                         Updated interface and implementation to align with
                         Program.cs and the new Role.Code-based permission model.
-------------------------------------------------------------------------------
 Notes       :
   - Registered as Scoped in Program.cs.
   - Entity class `Feature` is mapped to table `features` via EF Core.
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
    Task<bool> IsAllowedAsync(string roleCode, string featureKey, CancellationToken ct = default);
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
        string roleCode,
        string featureKey,
        CancellationToken ct = default)
    {
        // -----------------------------------------------------------------------
        // Verify feature exists and is enabled (Feature entity → features table)
        // -----------------------------------------------------------------------
        var featureEnabled = await _db.Set<Feature>()
            .AsNoTracking()
            .AnyAsync(f => f.Code == featureKey && f.IsEnabled, ct);

        if (!featureEnabled)
            return false;

        // -----------------------------------------------------------------------
        // Query rolefeature table joined to roles table using Role.Code
        // -----------------------------------------------------------------------
        return await (
            from rf in _db.Set<RoleFeature>().AsNoTracking()
            join r in _db.Set<Role>().AsNoTracking()
                on rf.RoleId equals r.Id
            where r.Code == roleCode
               && rf.FeatureCode == featureKey
               && rf.IsEnabled
            select rf
        ).AnyAsync(ct);
    }
}
