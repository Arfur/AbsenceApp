/*
===============================================================================
 File        : RoleMapper.cs
 Namespace   : AbsenceApp.Data.Mappers
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-13
 Updated     : 2026-03-13
-------------------------------------------------------------------------------
 Purpose     : Translates between the Role EF entity (AbsenceApp.Data.Models)
               and the RoleDto (AbsenceApp.Core.DTOs) across layer boundaries.
-------------------------------------------------------------------------------
 Description :
   Pure static methods — no DI, no side-effects — so the mapper can be
   called by services and tested independently without a container.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-13  Initial implementation.
-------------------------------------------------------------------------------
 Notes       :
   - Only RoleId and RoleName are transferred; navigation collections
     (e.g. UserRoles) are intentionally excluded from the DTO.
===============================================================================
*/

using AbsenceApp.Core.DTOs;
using AbsenceApp.Data.Models;

namespace AbsenceApp.Data.Mappers;

public static class RoleMapper
{
    // =========================================================================
    // ToDto — Role entity to RoleDto projection
    // =========================================================================

    public static RoleDto ToDto(Role entity) => new()
    {
        RoleId   = entity.RoleId,
        RoleName = entity.RoleName,
    };

    // =========================================================================
    // ToEntity — RoleDto to Role entity projection
    // =========================================================================

    public static Role ToEntity(RoleDto dto) => new()
    {
        RoleId   = dto.RoleId,
        RoleName = dto.RoleName,
    };
}
