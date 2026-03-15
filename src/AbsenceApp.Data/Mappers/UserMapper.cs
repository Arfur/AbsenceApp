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
        UserId    = (int)entity.Id,
        Username  = entity.Username,
        FirstName = entity.Name,
        LastName  = string.Empty,
        Email     = entity.Email,
        IsActive  = entity.Status == "Active",
        CreatedAt = entity.CreatedAt,
    };

    // =========================================================================
    // DTO to entity
    // =========================================================================

    public static User ToEntity(UserDto dto) => new()
    {
        Id        = dto.UserId,
        Name      = $"{dto.FirstName} {dto.LastName}".Trim(),
        Username  = dto.Username,
        Email     = dto.Email ?? string.Empty,
        Status    = dto.IsActive ? "Active" : "Inactive",
        CreatedAt = dto.CreatedAt,
        UpdatedAt = dto.CreatedAt,
    };
}
