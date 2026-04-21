/*
===============================================================================
 File        : Absence.cs
 Namespace   : AbsenceApp.Data.Models
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-04-21
 Updated     : 2026-04-21
-------------------------------------------------------------------------------
 Purpose     : EF Core entity for the Absences table. Unified model covering
               both Staff and Student absences via PersonType discriminator.
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
