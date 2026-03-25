/*
===============================================================================
 File        : AuditLogMapper.cs
 Namespace   : AbsenceApp.Data.Mappers
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-13
 Updated     : 2026-03-13
-------------------------------------------------------------------------------
 Purpose     : Translates between the AuditLog EF entity
               (AbsenceApp.Data.Models) and the AuditLogDto
               (AbsenceApp.Core.DTOs) across layer boundaries.
-------------------------------------------------------------------------------
 Description :
   Pure static methods — no DI, no side-effects — so the mapper can be
   called by services and tested independently without a container.
   Note: ToEntity is provided for completeness; new audit entries are
   typically constructed directly by AuditLogService.LogAsync.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-13  Initial implementation.
-------------------------------------------------------------------------------
 Notes       :
   - The User navigation property is excluded from the DTO; callers
     should look up user details separately if required.
===============================================================================
*/

using AbsenceApp.Core.DTOs;
using AbsenceApp.Data.Models;

namespace AbsenceApp.Data.Mappers;

public static class AuditLogMapper
{
    // =========================================================================
    // ToDto — AuditLog entity to AuditLogDto projection
    // =========================================================================

    public static AuditLogDto ToDto(AuditLog entity) => new()
    {
        AuditId   = entity.AuditId,
        UserId    = entity.UserId,
        Action    = entity.Action,
        Timestamp = entity.Timestamp,
    };

    // =========================================================================
    // ToEntity — AuditLogDto to AuditLog entity projection
    // =========================================================================

    public static AuditLog ToEntity(AuditLogDto dto) => new()
    {
        AuditId   = dto.AuditId,
        UserId    = dto.UserId,
        Action    = dto.Action,
        Timestamp = dto.Timestamp,
    };
}
