/*
===============================================================================
 File        : StudentFullViewDto.cs
 Namespace   : AbsenceApp.Core.DTOs
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-17
 Updated     : 2026-03-17
-------------------------------------------------------------------------------
 Purpose     : Flattened read-only projection of a Student record for Table
               Settings display. FK IDs are replaced with human-readable names
               resolved via lookup tables at the service layer.
               System fields (Id, SchoolId, CreatedAt, UpdatedAt) are excluded.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-17  Initial creation.
-------------------------------------------------------------------------------
 Notes       :
   - Not an EF Core entity — never tracked or persisted.
   - SchoolId suppressed entirely (single-school deployment).
   - YearGroupName, ClassName, HouseName resolved by StudentFullViewService.
===============================================================================
*/

namespace AbsenceApp.Core.DTOs;

// =========================================================================
// StudentFullViewDto — flattened student projection for Table Settings display
// =========================================================================

public class StudentFullViewDto
{
    // -------------------------------------------------------------------------
    // Identity
    // -------------------------------------------------------------------------
    public string AdmissionNumber { get; set; } = string.Empty;
    public string FirstName       { get; set; } = string.Empty;
    public string? MiddleNames    { get; set; }
    public string LastName        { get; set; } = string.Empty;
    public string LegalFirstName  { get; set; } = string.Empty;
    public string LegalLastName   { get; set; } = string.Empty;
    public string Gender          { get; set; } = string.Empty;
    public DateOnly DateOfBirth   { get; set; }

    // -------------------------------------------------------------------------
    // Resolved lookups (replacing FK IDs)
    // -------------------------------------------------------------------------
    public string  YearGroupName  { get; set; } = string.Empty;
    public string  ClassName      { get; set; } = string.Empty;
    public string? HouseName      { get; set; }

    // -------------------------------------------------------------------------
    // System / login
    // -------------------------------------------------------------------------
    public string? Username       { get; set; }
    public string? Upn            { get; set; }

    // -------------------------------------------------------------------------
    // Enrolment
    // -------------------------------------------------------------------------
    public DateOnly AdmissionDate { get; set; }
    public string Status          { get; set; } = string.Empty;

    // -------------------------------------------------------------------------
    // Computed convenience
    // -------------------------------------------------------------------------
    public string FullName => $"{FirstName} {LastName}".Trim();
}
