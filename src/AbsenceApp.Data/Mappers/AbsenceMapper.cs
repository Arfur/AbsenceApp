/*
===============================================================================
 File        : AbsenceMapper.cs
 Namespace   : AbsenceApp.Data.Mappers
 Author      : Michael
 Version     : 1.1.0
 Created     : 2026-03-13
 Updated     : 2026-03-13
-------------------------------------------------------------------------------
 Purpose     : Maps between the Attendance EF entity (AbsenceApp.Data.Models)
               and the AbsenceRecord domain model (AbsenceApp.Core.Models)
               used by the client and service layers.
-------------------------------------------------------------------------------
 Description :
   Bridges the legacy AbsenceRecord domain model and the Attendance EF entity.
   Used by AbsenceService when the application transitions from in-memory
   storage to the EF Core repository. Pure static methods — no DI.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-13  Initial placeholder created.
   - 1.1.0  2026-03-13  Implemented ToDomain and ToEntity conversion methods.
-------------------------------------------------------------------------------
 Notes       :
   - AttendanceId (int) is stringified to populate AbsenceRecord.Id.
   - YearGroup and class/school context are not stored in Attendance; they
     must be enriched by the caller if required.
===============================================================================
*/

using AbsenceApp.Core.Models;
using AbsenceApp.Data.Models;

namespace AbsenceApp.Data.Mappers;

public static class AbsenceMapper
{
    // =========================================================================
    // ToDomain — Attendance EF entity to AbsenceRecord domain model
    // =========================================================================

    /// <summary>
    /// Projects an <see cref="Attendance"/> EF entity to the lightweight
    /// <see cref="AbsenceRecord"/> domain model used by the client layer.
    /// Status is preserved verbatim; AttendanceId is stringified for Id.
    /// </summary>
    public static AbsenceRecord ToDomain(Attendance entity) => new()
    {
        Id        = entity.AttendanceId.ToString(),
        StudentId = entity.UserId.ToString(),
        Date      = entity.Timestamp.Date,
        Reason    = entity.Status,
    };

    // =========================================================================
    // ToEntity — AbsenceRecord domain model to Attendance EF entity
    // =========================================================================

    /// <summary>
    /// Projects an <see cref="AbsenceRecord"/> domain model to an
    /// <see cref="Attendance"/> EF entity ready for persistence.
    /// AttendanceId is 0 (DB-generated) when creating a new record.
    /// </summary>
    public static Attendance ToEntity(AbsenceRecord domain, int recordedBy) => new()
    {
        UserId     = int.TryParse(domain.StudentId, out var uid) ? uid : 0,
        Status     = domain.Reason ?? string.Empty,
        Timestamp  = domain.Date,
        RecordedBy = recordedBy,
    };
}
