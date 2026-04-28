/*
===============================================================================
 File        : StudentFullViewDto.cs
 Namespace   : AbsenceApp.Core.DTOs
 Author      : Michael
 Version     : 1.1.0
 Created     : 2026-03-17
 Updated     : 2026-04-05
-------------------------------------------------------------------------------
 Purpose     : Flattened read-only projection of a Student record for Table
               Settings display. FK IDs are replaced with human-readable names
               resolved via lookup tables at the service layer.
               System fields (Id, SchoolId, CreatedAt, UpdatedAt) are excluded.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-17  Initial creation.
   - 1.1.0  2026-04-05  Phase 3 Remediation Issue 2: added Id property so
                         list views can build navigation links (Detail/Edit).
-------------------------------------------------------------------------------
 Notes       :
   - Not an EF Core entity — never tracked or persisted.
   - SchoolId suppressed entirely (single-school deployment).
   - YearGroupName, ClassName, HouseName resolved by StudentFullViewService.
   - Id included for navigation link generation only.
===============================================================================
*/

namespace AbsenceApp.Core.DTOs;

// =========================================================================
// StudentFullViewDto — flattened student projection for Table Settings display
// =========================================================================

public class StudentFullViewDto
{
    // -------------------------------------------------------------------------
    // Primary key — needed for navigation links (Detail / Edit)
    // -------------------------------------------------------------------------
    public int Id { get; set; }

    // -------------------------------------------------------------------------
    // Identity
    // -------------------------------------------------------------------------
    public string AdmissionNumber { get; set; } = string.Empty;
    public string FirstName       { get; set; } = string.Empty;
    public string? MiddleNames    { get; set; }
    public string LastName        { get; set; } = string.Empty;
    public string? LegalFirstName { get; set; }
    public string? LegalLastName  { get; set; }
    public string? PreferredName  { get; set; }
    public string? Gender         { get; set; }
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
