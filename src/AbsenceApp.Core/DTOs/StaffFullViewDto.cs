/*
===============================================================================
 File        : StaffFullViewDto.cs
 Namespace   : AbsenceApp.Core.DTOs
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-17
 Updated     : 2026-03-17
-------------------------------------------------------------------------------
 Purpose     : Flattened read-only projection of a Staff record for Table
               Settings display. FK IDs are replaced with human-readable names
               resolved via lookup tables at the service layer.
               System fields (Id, CreatedAt, UpdatedAt) and raw FK IDs
               (JobTitleId, JobGroupId, DepartmentId, ReportingManagerId) excluded.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-17  Initial creation.
-------------------------------------------------------------------------------
 Notes       :
   - Not an EF Core entity — never tracked or persisted.
   - ReportingManagerName resolved from Staff table via self-join in service.
   - JobTitleName, JobGroupName, DepartmentName resolved by StaffFullViewService.
===============================================================================
*/

namespace AbsenceApp.Core.DTOs;

// =========================================================================
// StaffFullViewDto — flattened staff projection for Table Settings display
// =========================================================================

public class StaffFullViewDto
{
    // -------------------------------------------------------------------------
    // Identity
    // -------------------------------------------------------------------------
    public string  StaffNumber    { get; set; } = string.Empty;
    public string  FirstName      { get; set; } = string.Empty;
    public string  LastName       { get; set; } = string.Empty;
    public string? PreferredName  { get; set; }
    public string  Title          { get; set; } = string.Empty;
    public DateOnly DateOfBirth   { get; set; }
    public string? Gender         { get; set; }

    // -------------------------------------------------------------------------
    // Contact
    // -------------------------------------------------------------------------
    public string  WorkEmail      { get; set; } = string.Empty;
    public string? AltEmail       { get; set; }
    public string? PhoneHome      { get; set; }
    public string? PhoneMobile    { get; set; }
    public string? PhoneEmergency { get; set; }

    // -------------------------------------------------------------------------
    // Employment
    // -------------------------------------------------------------------------
    public string   EmploymentType { get; set; } = string.Empty;
    public string   ContractType   { get; set; } = string.Empty;
    public DateOnly HireDate       { get; set; }
    public DateOnly? EndDate       { get; set; }
    public string   WorkLocation   { get; set; } = string.Empty;

    // -------------------------------------------------------------------------
    // Resolved lookups (replacing FK IDs)
    // -------------------------------------------------------------------------
    public string? ReportingManagerName { get; set; }
    public string  JobTitleName         { get; set; } = string.Empty;
    public string  JobGroupName         { get; set; } = string.Empty;
    public string  DepartmentName       { get; set; } = string.Empty;

    // -------------------------------------------------------------------------
    // Profile / status
    // -------------------------------------------------------------------------
    public string? ProfilePhotoUrl { get; set; }
    public string  AccountStatus   { get; set; } = string.Empty;

    // -------------------------------------------------------------------------
    // Computed convenience
    // -------------------------------------------------------------------------
    public string FullName => $"{FirstName} {LastName}".Trim();
    public bool IsActive => AccountStatus == "Active";
}
