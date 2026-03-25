/*
===============================================================================
 File        : Attendance.cs
 Namespace   : AbsenceApp.Data.Models
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-15
 Updated     : 2026-03-15
-------------------------------------------------------------------------------
 Purpose     : Legacy stub entity retained for backward compatibility only.
               Active attendance data is held in AttendanceRegister / AttendanceMark.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-15  Initial creation.
-------------------------------------------------------------------------------
 Notes       :
   - All properties mirror the original schema; do not add new properties.
   - Remove this stub once all legacy references have been migrated.
===============================================================================
*/
namespace AbsenceApp.Data.Models;

// Legacy stub retained so AttendanceConfiguration.cs still compiles.
// Active attendance data is now in AttendanceRegister.cs and AttendanceMark.cs.
public class Attendance
{
    public int AttendanceId { get; set; }
    public int UserId { get; set; }
    public int? ClassId { get; set; }
    public string Status { get; set; } = default!;
    public DateTime Timestamp { get; set; }
    public int RecordedBy { get; set; }
    public User User { get; set; } = default!;
    public Class? Class { get; set; }
    public User Recorder { get; set; } = default!;
}
