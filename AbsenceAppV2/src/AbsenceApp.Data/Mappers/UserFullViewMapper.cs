/*
===============================================================================
 File        : UserFullViewMapper.cs
 Namespace   : AbsenceApp.Data.Mappers
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-17
 Updated     : 2026-03-17
-------------------------------------------------------------------------------
 Purpose     : Maps a User entity plus resolved lookup names to a
               UserFullViewDto.  Sensitive credential fields are never copied.
               RoleTypeName and DepartmentName are resolved by the calling
               service.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-17  Initial creation.
-------------------------------------------------------------------------------
 Notes       :
   - One-way mapping only (read projection — no ToEntity required).
   - Excluded intentionally: Password, RememberToken, TwoFactorSecret,
     BackupCodes, LastLoginIp — these must never reach the display layer.
===============================================================================
*/

using AbsenceApp.Core.DTOs;
using AbsenceApp.Data.Models;

namespace AbsenceApp.Data.Mappers;

// =========================================================================
// UserFullViewMapper — User entity → UserFullViewDto
// =========================================================================

public static class UserFullViewMapper
{
    /// <summary>
    /// Projects a User entity to a UserFullViewDto, substituting resolved
    /// lookup names for FK IDs and omitting all credential fields.
    /// </summary>
    /// <param name="entity">The raw User EF entity.</param>
    /// <param name="roleTypeName">Resolved display name from RoleType.DisplayName.</param>
    /// <param name="departmentName">Resolved name from Department.Name, or null.</param>
    public static UserFullViewDto ToDto(
        User    entity,
        string  roleTypeName,
        string? departmentName) => new()
    {
        Username           = entity.Username,
        Email              = entity.Email,
        EmailVerifiedAt    = entity.EmailVerifiedAt,
        Status             = entity.Status,
        IsAdmin            = entity.IsAdmin,
        LastLoginAt        = entity.LastLoginAt,
        LoginCount         = entity.LoginCount,
        RoleName           = roleTypeName,
        DepartmentName     = departmentName,
        IsTwoFactorEnabled = entity.IsTwoFactorEnabled,
        Timezone           = entity.Timezone,
        LanguageCode       = entity.LanguageCode,
    };
}
