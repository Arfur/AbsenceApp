/*
===============================================================================
 File        : SystemEvent.cs
 Namespace   : AbsenceApp.Data.Models
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-15
 Updated     : 2026-03-15
-------------------------------------------------------------------------------
 Purpose     : EF Core entity for the system_events log table.
               Records platform-level lifecycle events triggered by system processes.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-15  Initial creation.
-------------------------------------------------------------------------------
 Notes       :
   - No navigation properties; FK integrity is enforced at the database layer.
   - All required string properties use = default! to satisfy the nullable compiler.
===============================================================================
*/
namespace AbsenceApp.Data.Models;

public class SystemEvent
{
    public long Id { get; set; }
    public string EventType { get; set; } = default!;
    public DateTime EventTime { get; set; }
    public string TriggeredBy { get; set; } = default!;
    public string? Description { get; set; }
    public string? Metadata { get; set; }
}
