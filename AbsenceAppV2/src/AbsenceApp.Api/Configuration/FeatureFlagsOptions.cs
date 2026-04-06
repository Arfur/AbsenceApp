/*
===============================================================================
 File        : FeatureFlagsOptions.cs
 Namespace   : AbsenceApp.Api.Configuration
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-04-05
 Updated     : 2026-04-05
-------------------------------------------------------------------------------
 Purpose     : Strongly-typed configuration model for API feature flags.
               This class provides a controlled, auditable mechanism for
               enabling or disabling application features without altering
               runtime logic.

               It is introduced as part of a phased migration from static
               JSON-based sidebar configuration to database-driven role and
               user-based menu entitlements.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-04-05  Initial implementation.
-------------------------------------------------------------------------------
 Notes       :
   - This class must remain free of business logic.
   - Feature flags are additive and must be non-breaking.
   - All flags default to FALSE unless explicitly enabled.
   - Flags are bound from the "Features" section in appsettings.json.
===============================================================================
*/

namespace AbsenceApp.Api.Configuration
{
    // =========================================================================
    // Feature flag definitions — used to control optional application behaviour
    // =========================================================================
    public class FeatureFlagsOptions
    {
        // ---------------------------------------------------------------------
        // Sidebar / Navigation feature flags
        // ---------------------------------------------------------------------

        /// <summary>
        /// Enables database-driven sidebar and menu entitlements.
        ///
        /// FALSE:
        ///   - Existing JSON-based sidebar configuration is used.
        ///   - No role or user-specific menu filtering is applied.
        ///
        /// TRUE:
        ///   - Sidebar visibility is determined by effective entitlements
        ///     derived from RoleType defaults and per-user overrides.
        ///
        /// Default: false
        /// </summary>
        public bool UseDbMenuEntitlements { get; set; }
    }
}
