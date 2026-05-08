/*
===============================================================================
 File        : EntitlementsResolver.cs
 Namespace   : AbsenceApp.Api.Services.Entitlements
 Author      : Michael
 Version     : 3.0.0
 Created     : 2026-04-05
 Updated     : 2026-05-07
-------------------------------------------------------------------------------
 Purpose     : Resolves the effective set of entitlement feature codes for a user.

               Entitlements are resolved deterministically using the following
               precedence rules:

                 1) Explicit per-user overrides
                 2) Role-based default entitlements
                 3) Implicit deny

               This service contains no UI or navigation knowledge and is
               intended to be consumed by API endpoints only.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-04-05  Initial implementation.
   - 1.1.0  2026-04-05  Changed using from AbsenceApp.Api.Data.Entities to
                         AbsenceApp.Data.Models so that Set<Feature>(),
                         Set<RoleFeature>(), and Set<UserFeatureOverride>()
                         resolve to the types registered in AppDbContext via
                         ConfigureEntitlements(). The prior namespace caused
                         an InvalidOperationException at runtime.
   - 2.0.0  2026-04-19  Schema alignment: updated all field references to match
                         CSV. Feature.IsActive→IsEnabled, Feature.Key→Code,
                         RoleFeature.RoleType→RoleId, RoleFeature.FeatureId→
                         FeatureCode, RoleFeature.IsAllowed→IsEnabled.
                         UserFeatureOverride.FeatureId→FeatureCode,
                         UserFeatureOverride.IsAllowed→IsEnabled.
                         JOIN predicate changed from int FeatureId to string
                         FeatureCode equals feature.Code.
   - 3.0.0  2026-05-07  Role schema consolidation: replaced legacy int roleType
                         parameter with string roleCode. Role-based entitlement
                         resolution now joins RoleFeature→Role and filters by
                         Role.Code (e.g., 'SUPERADMIN', 'ADMIN', 'USER').
                         Updated LINQ pipeline and interface signature to use
                         code-based lookup consistent with the new roles table.
-------------------------------------------------------------------------------
 Notes       :
   - No fallback behaviour is implemented.
   - User provisioning guarantees entitlements exist.
===============================================================================
*/

using AbsenceApp.Data.Context;
using AbsenceApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AbsenceApp.Api.Services.Entitlements;

public interface IEntitlementsResolver
{
    Task<HashSet<string>> GetEffectiveAllowedKeysAsync(
        Guid userId,
        int rolecode,
        CancellationToken ct = default);
}

public sealed class EntitlementsResolver : IEntitlementsResolver
{
    // =========================================================================
    // Dependencies
    // =========================================================================
    private readonly AppDbContext _db;

    public EntitlementsResolver(AppDbContext db)
    {
        _db = db;
    }

    // =========================================================================
    // Resolve effective allowed entitlement codes for a user
    // =========================================================================
    public async Task<HashSet<string>> GetEffectiveAllowedKeysAsync(
        Guid userId,
        int rolecode,
        CancellationToken ct = default)
    {
        // ---------------------------------------------------------------------
        // Active features only
        // ---------------------------------------------------------------------
        var activeFeatures =
            _db.Set<Feature>()
               .AsNoTracking()
               .Where(f => f.IsEnabled);

        // ---------------------------------------------------------------------
        // Role-based default entitlements
        // ---------------------------------------------------------------------
        var roleDefaults =
            from rf in _db.Set<RoleFeature>().AsNoTracking()
            join r in _db.Set<Role>().AsNoTracking()
                on rf.RoleId equals r.Id
            where r.Code == roleCode && rf.IsEnabled
            join f in activeFeatures on rf.FeatureCode equals f.Code
            select f.Code;

        // ---------------------------------------------------------------------
        // Per-user overrides (authoritative)
        // ---------------------------------------------------------------------
        var userOverrides =
            from ufo in _db.Set<UserFeatureOverride>().AsNoTracking()
            where ufo.UserId == userId
            join f in activeFeatures on ufo.FeatureCode equals f.Code
            select new
            {
                f.Code,
                ufo.IsEnabled
            };

        // ---------------------------------------------------------------------
        // Resolve effective set
        // ---------------------------------------------------------------------
        var allowed = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var code in await roleDefaults.ToListAsync(ct))
        {
            allowed.Add(code);
        }

        foreach (var ov in await userOverrides.ToListAsync(ct))
        {
            if (ov.IsEnabled)
                allowed.Add(ov.Code);
            else
                allowed.Remove(ov.Code);
        }

        return allowed;
    }
}
