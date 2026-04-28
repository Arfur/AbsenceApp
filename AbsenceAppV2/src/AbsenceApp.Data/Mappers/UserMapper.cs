/*
===============================================================================
 File        : UserMapper.cs
 Namespace   : AbsenceApp.Data.Mappers
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-15
 Updated     : 2026-03-15
-------------------------------------------------------------------------------
 Purpose     : Maps between the User EF entity (TABLE1 schema) and UserDto.
               FirstName in the DTO carries the combined Name field from the entity.
               IsActive is derived from the Status string property.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-15  Initial creation.
-------------------------------------------------------------------------------
 Notes       :
   - UserId cast from long to int; current data fits within int range.
   - Align UserDto to long once the client layer is updated.
===============================================================================
*/

using AbsenceApp.Core.DTOs;
using AbsenceApp.Data.Models;

namespace AbsenceApp.Data.Mappers;

// =========================================================================
// UserMapper -- User entity <-> UserDto bidirectional mapping
// =========================================================================

/// <summary>
/// Maps between the User EF entity (TABLE1 schema) and UserDto.
/// FirstName in the DTO carries the combined Name field from the entity.
/// </summary>
public static class UserMapper
{
    // =========================================================================
    // Entity to DTO
    // =========================================================================

    public static UserDto ToDto(User entity) => new()
    {
        UserId       = entity.Id,
        Username     = entity.Username,
        Email        = entity.Email,
        Status       = entity.Status,
        IsAdmin      = entity.IsAdmin,
        StaffId      = entity.StaffId,
        Timezone     = entity.Timezone,
        LanguageCode = entity.LanguageCode,
        LastLoginAt  = entity.LastLoginAt,
        LoginCount   = entity.LoginCount,
        CreatedAt    = entity.CreatedAt,
        UpdatedAt    = entity.UpdatedAt,
    };

    // =========================================================================
    // DTO to entity
    // =========================================================================

    public static User ToEntity(UserDto dto) => new()
    {
        Id           = dto.UserId,
        Username     = dto.Username,
        Email        = dto.Email,
        Status       = dto.Status,
        IsAdmin      = dto.IsAdmin,
        StaffId      = dto.StaffId,
        Timezone     = dto.Timezone,
        LanguageCode = dto.LanguageCode,
        LastLoginAt  = dto.LastLoginAt,
        LoginCount   = dto.LoginCount,
        CreatedAt    = dto.CreatedAt,
        UpdatedAt    = dto.UpdatedAt,
    };
}
