/*
===============================================================================
 File        : StaffFullViewMapper.cs
 Namespace   : AbsenceApp.Data.Mappers
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-17
 Updated     : 2026-03-17
-------------------------------------------------------------------------------
 Purpose     : Maps a Staff entity plus resolved lookup names to a
               StaffFullViewDto.  Lookup names (JobTitleName, JobGroupName,
               DepartmentName, ReportingManagerName) must be resolved by the
               calling service before invoking ToDto.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-17  Initial creation.
-------------------------------------------------------------------------------
 Notes       :
   - One-way mapping only (read projection — no ToEntity required).
   - PhoneHome and PhoneEmergency are included here even though StaffMapper
     omits them, because the full-view surface should expose all contact fields.
===============================================================================
*/

using AbsenceApp.Core.DTOs;
using AbsenceApp.Data.Models;

namespace AbsenceApp.Data.Mappers;

// =========================================================================
// StaffFullViewMapper — Staff entity → StaffFullViewDto
// =========================================================================

public static class StaffFullViewMapper
{
    /// <summary>
    /// Projects a Staff entity to a StaffFullViewDto, substituting
    /// resolved lookup names for FK IDs.
    /// </summary>
    /// <param name="entity">The raw Staff EF entity.</param>
    /// <param name="jobTitleName">Resolved name from JobTitle.Title.</param>
    /// <param name="jobGroupName">Resolved name from JobGroup.Name.</param>
    /// <param name="departmentName">Resolved name from Department.Name.</param>
    /// <param name="managerName">Full name of the reporting manager, or null.</param>
    public static StaffFullViewDto ToDto(
        Staff   entity,
        string  jobTitleName,
        string  jobGroupName,
        string  departmentName,
        string? managerName) => new()
    {
        StaffNumber           = entity.StaffNumber,
        FirstName             = entity.FirstName,
        LastName              = entity.LastName,
        PreferredName         = entity.PreferredName,
        Title                 = entity.Title,
        DateOfBirth           = entity.DateOfBirth,
        Gender                = entity.Gender,
        WorkEmail             = entity.WorkEmail,
        AltEmail              = entity.AltEmail,
        PhoneHome             = entity.PhoneHome,
        PhoneMobile           = entity.PhoneMobile,
        PhoneEmergency        = entity.PhoneEmergency,
        EmploymentType        = entity.EmploymentType,
        ContractType          = entity.ContractType,
        HireDate              = entity.HireDate,
        EndDate               = entity.EndDate,
        WorkLocation          = entity.WorkLocation,
        ReportingManagerName  = managerName,
        JobTitleName          = jobTitleName,
        JobGroupName          = jobGroupName,
        DepartmentName        = departmentName,
        ProfilePhotoUrl       = entity.ProfilePhotoUrl,
        AccountStatus         = entity.AccountStatus,
    };
}
