/*
===============================================================================
 File        : AttendanceMapper.cs
 Namespace   : AbsenceApp.Data.Mappers
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-13
 Updated     : 2026-03-13
-------------------------------------------------------------------------------
 Purpose     : Translates between the Attendance EF entity
               (AbsenceApp.Data.Models) and the AttendanceDto
               (AbsenceApp.Core.DTOs) across layer boundaries.
-------------------------------------------------------------------------------
 Description :
   Pure static methods — no DI, no side-effects — so the mapper can be
   called by services and tested independently without a container.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-13  Initial implementation.
-------------------------------------------------------------------------------
 Notes       :
   - Navigation properties (User, Class, Recorder) are excluded from the
     DTO; callers should load related data separately if needed.
===============================================================================
*/

using AbsenceApp.Core.DTOs;
using AbsenceApp.Data.Models;

namespace AbsenceApp.Data.Mappers;

public static class AttendanceMapper
{
    // =========================================================================
    // ToDto — Attendance entity to AttendanceDto projection
    // =========================================================================

    public static AttendanceDto ToDto(Attendance entity) => new()
    {
        AttendanceId = entity.AttendanceId,
        UserId       = entity.UserId,
        ClassId      = entity.ClassId,
        Status       = entity.Status,
        Timestamp    = entity.Timestamp,
        RecordedBy   = entity.RecordedBy,
    };

    // =========================================================================
    // ToEntity — AttendanceDto to Attendance entity projection
    // =========================================================================

    public static Attendance ToEntity(AttendanceDto dto) => new()
    {
        AttendanceId = dto.AttendanceId,
        UserId       = dto.UserId,
        ClassId      = dto.ClassId,
        Status       = dto.Status,
        Timestamp    = dto.Timestamp,
        RecordedBy   = dto.RecordedBy,
    };
}
