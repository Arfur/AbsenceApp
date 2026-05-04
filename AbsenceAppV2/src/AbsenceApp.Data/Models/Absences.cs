/*
===============================================================================
 File        : Absences.cs
 Namespace   : AbsenceApp.Data.Models
 Author      : Michael
 Version     : 1.1.0
 Created     : 2026-04-21
 Updated     : 2026-05-04
-------------------------------------------------------------------------------
 Purpose     : EF Core entity for the Absences table. Unified model covering
               both Staff and Student absences via PersonType discriminator.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-04-21  Initial creation.
   - 1.1.0  2026-05-04  Phase 2 type alignment: changed all ulong/ulong? PK and
                         FK properties to long/long?. The absences DB table uses
                         bigint (signed) as confirmed by the EF migration snapshot.
                         Applies to: Id, PersonId, AbsenceTypeId, StatusId,
                         RecordedBy, ApprovedBy.
===============================================================================
*/
namespace AbsenceApp.Data.Models;

public class Absence
{
    public long     Id            { get; set; }
    public string   PersonType    { get; set; } = string.Empty;   // "Staff" | "Student"
    public long     PersonId      { get; set; }
    public long     AbsenceTypeId { get; set; }
    public long     StatusId      { get; set; }
    public DateOnly StartDate     { get; set; }
    public DateOnly EndDate       { get; set; }
    public int      DurationDays  { get; set; }
    public string   ReportedVia   { get; set; } = "Manual";       // ENUM: Manual|Email|Phone|MIS
    public string?  Notes         { get; set; }
    public long?    RecordedBy    { get; set; }
    public long?    ApprovedBy    { get; set; }
    public DateTime? ApprovedAt   { get; set; }
    public DateTime CreatedAt     { get; set; }
    public DateTime UpdatedAt     { get; set; }

    // Navigation
    public AbsenceType?   AbsenceType { get; set; }
    public AbsenceStatus? Status      { get; set; }
}
