/*
===============================================================================
 File        : AbsenceAudit.cs
 Namespace   : AbsenceApp.Data.Models
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-04-21
 Updated     : 2026-04-21
-------------------------------------------------------------------------------
 Purpose     : EF Core entity for the AbsenceAudit table. Records every
               state transition and change made to an Absence record.
===============================================================================
*/
namespace AbsenceApp.Data.Models;

public class AbsenceAudit
{
    public long     Id          { get; set; }
    public long     AbsenceId   { get; set; }
    public long     ChangedBy   { get; set; }
    public string   ChangeType  { get; set; } = string.Empty;   // ENUM: Created|Updated|StatusChanged|Deleted
    public long?    OldStatusId { get; set; }
    public long?    NewStatusId { get; set; }
    public string?  Notes       { get; set; }
    public DateTime ChangedAt   { get; set; }

    // Navigation
    public Absence?       Absence   { get; set; }
    public AbsenceStatus? OldStatus { get; set; }
    public AbsenceStatus? NewStatus { get; set; }
}
