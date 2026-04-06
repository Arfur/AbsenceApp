/*
===============================================================================
 File        : EntitlementsResolver.cs
 Namespace   : AbsenceApp.Api.Services.Entitlements
 Author      : Michael
 Version     : 1.1.0
 Created     : 2026-04-05
 Updated     : 2026-04-05
-------------------------------------------------------------------------------
 Purpose     : Resolves the effective set of entitlement feature keys for a user.

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
        int roleType,
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
    // Resolve effective allowed entitlement keys for a user
    // =========================================================================
    public async Task<HashSet<string>> GetEffectiveAllowedKeysAsync(
        Guid userId,
        int roleType,
        CancellationToken ct = default)
    {
        // ---------------------------------------------------------------------
        // Active features only
        // ---------------------------------------------------------------------
        var activeFeatures =
            _db.Set<Feature>()
               .AsNoTracking()
               .Where(f => f.IsActive);

        // ---------------------------------------------------------------------
        // Role-based default entitlements
        // ---------------------------------------------------------------------
        var roleDefaults =
            from rf in _db.Set<RoleFeature>().AsNoTracking()
            where rf.RoleType == roleType && rf.IsAllowed
            join f in activeFeatures on rf.FeatureId equals f.FeatureId
            select f.Key;

        // ---------------------------------------------------------------------
        // Per-user overrides (authoritative)
        // ---------------------------------------------------------------------
        var userOverrides =
            from ufo in _db.Set<UserFeatureOverride>().AsNoTracking()
            where ufo.UserId == userId
            join f in activeFeatures on ufo.FeatureId equals f.FeatureId
            select new
            {
                f.Key,
                ufo.IsAllowed
            };

        // ---------------------------------------------------------------------
        // Resolve effective set
        // ---------------------------------------------------------------------
        var allowed = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var key in await roleDefaults.ToListAsync(ct))
        {
            allowed.Add(key);
        }

        foreach (var ov in await userOverrides.ToListAsync(ct))
        {
            if (ov.IsAllowed)
                allowed.Add(ov.Key);
            else
                allowed.Remove(ov.Key);
        }

        return allowed;
    }
}
