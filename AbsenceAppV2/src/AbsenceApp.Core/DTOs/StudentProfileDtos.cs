/*
===============================================================================
 File        : StudentProfileDtos.cs
 Namespace   : AbsenceApp.Core.DTOs
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-05-05
 Updated     : 2026-05-05
-------------------------------------------------------------------------------
 Purpose     : DTOs for the Student Absence Management system: medical records,
               flag records, and full absence update.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-05-05  Initial creation.
===============================================================================
*/

namespace AbsenceApp.Core.DTOs;

// =========================================================================
// StudentMedicalDto — flattened medical record for display
// =========================================================================

public class StudentMedicalDto
{
    public int     Id                   { get; set; }
    public int     StudentId            { get; set; }
    public string  MedicalCondition     { get; set; } = string.Empty;
    public bool    IsAllergic           { get; set; }
    public string? AllergyDetails       { get; set; }
    public string? Medication           { get; set; }
    public string? EmergencyActionPlan  { get; set; }
    public int     RecordedBy           { get; set; }
    public DateTime CreatedAt           { get; set; }
    public DateTime UpdatedAt           { get; set; }
}

// =========================================================================
// StudentFlagDto — flattened flag record for display
// =========================================================================

public class StudentFlagDto
{
    public int      Id         { get; set; }
    public int      StudentId  { get; set; }
    public string   FlagCode   { get; set; } = string.Empty;
    public bool     IsActive   { get; set; }
    public string?  Notes      { get; set; }
    public DateTime AssignedAt { get; set; }
    public int      AssignedBy { get; set; }
    public DateTime CreatedAt  { get; set; }
    public DateTime UpdatedAt  { get; set; }
}

// =========================================================================
// UpdateAbsenceDto — full-field update for an existing absence record
// =========================================================================

public class UpdateAbsenceDto
{
    public long     AbsenceTypeId { get; set; }
    public long     StatusId      { get; set; }
    public DateOnly StartDate     { get; set; }
    public DateOnly EndDate       { get; set; }
    public string   ReportedVia   { get; set; } = "Manual";
    public string?  Notes         { get; set; }
    public long?    ApprovedBy    { get; set; }
}
