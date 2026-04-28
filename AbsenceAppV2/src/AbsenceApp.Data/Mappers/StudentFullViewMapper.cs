/*
===============================================================================
 File        : StudentFullViewMapper.cs
 Namespace   : AbsenceApp.Data.Mappers
 Author      : Michael
 Version     : 1.1.0
 Created     : 2026-03-17
 Updated     : 2026-04-05
-------------------------------------------------------------------------------
 Purpose     : Maps a Student entity plus resolved lookup names to a
               StudentFullViewDto.  The lookup names (YearGroupName, ClassName,
               HouseName) must be resolved by the calling service before
               invoking ToDto.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-17  Initial creation.
   - 1.1.0  2026-04-05  Phase 3 Remediation Issue 2: map Id from entity.
-------------------------------------------------------------------------------
 Notes       :
   - One-way mapping only (read projection — no ToEntity required).
   - SchoolId is intentionally suppressed in the output DTO.
===============================================================================
*/

using AbsenceApp.Core.DTOs;
using DataStudent = AbsenceApp.Data.Models.Student;

namespace AbsenceApp.Data.Mappers;

// =========================================================================
// StudentFullViewMapper — Student entity → StudentFullViewDto
// =========================================================================

public static class StudentFullViewMapper
{
    /// <summary>
    /// Projects a Student entity to a StudentFullViewDto, substituting
    /// resolved lookup names for FK IDs.
    /// </summary>
    /// <param name="entity">The raw Student EF entity.</param>
    /// <param name="yearGroupName">Resolved name from YearGroup.Name.</param>
    /// <param name="className">Resolved name from Class.Name.</param>
    /// <param name="houseName">Resolved name from House.Name, or null.</param>
    public static StudentFullViewDto ToDto(
        DataStudent entity,
        string  yearGroupName,
        string  className,
        string? houseName) => new()
    {
        Id              = entity.Id,
        AdmissionNumber = entity.AdmissionNumber,
        FirstName       = entity.FirstName,
        MiddleNames     = entity.MiddleNames,
        LastName        = entity.LastName,
        LegalFirstName  = entity.LegalFirstName,
        LegalLastName   = entity.LegalLastName,
        PreferredName   = entity.PreferredName,
        Gender          = entity.Gender,
        DateOfBirth     = entity.DateOfBirth,
        YearGroupName   = yearGroupName,
        ClassName       = className,
        HouseName       = houseName,
        Username        = entity.Username,
        Upn             = entity.Upn,
        AdmissionDate   = entity.AdmissionDate,
        Status          = entity.Status ?? string.Empty,
    };
}
