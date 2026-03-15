/*
===============================================================================
 File        : AttendanceDto.cs
 Namespace   : AbsenceApp.Core.DTOs
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-13
 Updated     : 2026-03-13
-------------------------------------------------------------------------------
 Purpose     : Data transfer object for passing attendance event data across
               layer boundaries without exposing the EF Core Attendance entity.
-------------------------------------------------------------------------------
 Description :
   Carries the attendance identifier, user, optional class, status string,
   timestamp, and recorder reference for a single attendance event.
   Used as the return type from attendance-related repository and service calls.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-13  Initial creation.
-------------------------------------------------------------------------------
 Notes       :
   - ClassId is nullable; not all attendance events are associated with a class.
   - Status is a free-text string (e.g. "Present", "Absent", "Late").
===============================================================================
*/

namespace AbsenceApp.Core.DTOs;

public class AttendanceDto
{
    // =========================================================================
    // Properties — data fields transferred across layer boundaries
    // =========================================================================

    public int AttendanceId { get; set; }
    public int UserId { get; set; }
    public int? ClassId { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public int RecordedBy { get; set; }
}
