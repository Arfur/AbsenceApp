/*
===============================================================================
 File        : UserFullViewDto.cs
 Namespace   : AbsenceApp.Core.DTOs
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-17
 Updated     : 2026-03-17
-------------------------------------------------------------------------------
 Purpose     : Flattened read-only projection of a User record for Table
               Settings display. FK IDs replaced with human-readable names.
               Sensitive fields (Password, RememberToken, TwoFactorSecret,
               BackupCodes, LastLoginIp) excluded entirely.
               System fields (Id, CreatedAt, UpdatedAt) excluded.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-17  Initial creation.
-------------------------------------------------------------------------------
 Notes       :
   - Not an EF Core entity — never tracked or persisted.
   - RoleTypeName resolved from RoleType.DisplayName via UserFullViewService.
   - DepartmentName resolved from Department.Name via UserFullViewService.
===============================================================================
*/

namespace AbsenceApp.Core.DTOs;

// =========================================================================
// UserFullViewDto — flattened user projection for Table Settings display
// =========================================================================

public class UserFullViewDto
{
    // -------------------------------------------------------------------------
    // Identity
    // -------------------------------------------------------------------------
    public string  Username       { get; set; } = string.Empty;
    public string  Email          { get; set; } = string.Empty;
    public DateTime? EmailVerifiedAt { get; set; }
    public string  Status         { get; set; } = string.Empty;
    public bool    IsAdmin        { get; set; }

    // -------------------------------------------------------------------------
    // Login activity
    // -------------------------------------------------------------------------
    public DateTime? LastLoginAt  { get; set; }
    public int LoginCount         { get; set; }

    // -------------------------------------------------------------------------
    // Resolved lookups (replacing FK IDs)
    // -------------------------------------------------------------------------
    public string  RoleTypeName   { get; set; } = string.Empty;
    public string? DepartmentName { get; set; }

    // -------------------------------------------------------------------------
    // Preferences
    // -------------------------------------------------------------------------
    public bool    IsTwoFactorEnabled { get; set; }
    public string? Timezone       { get; set; }
    public string? LanguageCode   { get; set; }
}
