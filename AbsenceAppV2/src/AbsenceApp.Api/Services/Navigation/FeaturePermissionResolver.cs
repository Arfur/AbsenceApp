/*
===============================================================================
 File        : FeaturePermissionResolver.cs
 Namespace   : AbsenceApp.Api.Services.Navigation
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-04-06
 Updated     : 2026-04-06
-------------------------------------------------------------------------------
 Purpose     : Executes dbo.fn_IsFeatureAllowed(@RoleType, @FeatureId) to
               determine whether a given role may access a named feature.

               The API endpoint accepts a feature key (string), looks up the
               FeatureId from the features table, then calls the function.
               Returns false if the feature key does not exist or is inactive.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-04-06  Initial implementation (Phase 3 — Feature Permission
                         Boundary).
-------------------------------------------------------------------------------
 Notes       :
   - Raw SQL uses SqlParameter to prevent SQL injection.
   - Registered as Scoped in Program.cs.
===============================================================================
*/

using AbsenceApp.Data.Context;
using AbsenceApp.Data.Models;
using Microsoft.Data.SqlClient;
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
    // Looks up the feature by key and calls fn_IsFeatureAllowed.
    // ===========================================================================

    public async Task<bool> IsAllowedAsync(
        int roleType,
        string featureKey,
        CancellationToken ct = default)
    {
        // -----------------------------------------------------------------------
        // Resolve feature ID — returns null if unknown or inactive
        // -----------------------------------------------------------------------
        var feature = await _db.Set<Feature>()
            .AsNoTracking()
            .Where(f => f.Key == featureKey && f.IsActive)
            .Select(f => new { f.FeatureId })
            .FirstOrDefaultAsync(ct);

        if (feature is null)
            return false;

        // -----------------------------------------------------------------------
        // Call fn_IsFeatureAllowed(@RoleType, @FeatureId)
        // The function returns a BIT (mapped to bool via SELECT wrapper).
        // -----------------------------------------------------------------------
        const string sql = """
            SELECT dbo.fn_IsFeatureAllowed(@RoleType, @FeatureId) AS IsAllowed;
            """;

        var result = await _db.Database
            .SqlQueryRaw<FeatureAllowedRow>(sql,
                new SqlParameter("@RoleType",  roleType),
                new SqlParameter("@FeatureId", feature.FeatureId))
            .FirstOrDefaultAsync(ct);

        return result?.IsAllowed ?? false;
    }

    // ---------------------------------------------------------------------------
    // Private projection record for the scalar SQL result
    // ---------------------------------------------------------------------------

    private sealed class FeatureAllowedRow
    {
        public bool IsAllowed { get; set; }
    }
}
