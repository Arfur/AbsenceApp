using AbsenceApp.Core.DTOs;
using AbsenceApp.Data.Models;

namespace AbsenceApp.Data.Mappers;

/// <summary>Maps between the EF Staff entity and StaffDto.</summary>
public static class StaffMapper
{
    public static StaffDto ToDto(Staff entity) => new()
    {
        Id             = entity.Id,
        StaffNumber    = entity.StaffNumber,
        FirstName      = entity.FirstName,
        LastName       = entity.LastName,
        PreferredName  = entity.PreferredName,
        Title          = entity.Title,
        DateOfBirth    = entity.DateOfBirth,
        Gender         = entity.Gender,
        WorkEmail      = entity.WorkEmail,
        AltEmail       = entity.AltEmail,
        PhoneMobile    = entity.PhoneMobile,
        EmploymentType = entity.EmploymentType,
        ContractType   = entity.ContractType,
        HireDate       = entity.HireDate,
        EndDate        = entity.EndDate,
        WorkLocation   = entity.WorkLocation,
        JobTitleId     = entity.JobTitleId,
        JobGroupId     = entity.JobGroupId,
        DepartmentId   = entity.DepartmentId,
        ProfilePhotoUrl = entity.ProfilePhotoUrl,
        AccountStatus  = entity.AccountStatus,
        CreatedAt      = entity.CreatedAt,
        UpdatedAt      = entity.UpdatedAt,
    };

    public static Staff ToEntity(StaffDto dto) => new()
    {
        Id             = dto.Id,
        StaffNumber    = dto.StaffNumber,
        FirstName      = dto.FirstName,
        LastName       = dto.LastName,
        PreferredName  = dto.PreferredName,
        Title          = dto.Title,
        DateOfBirth    = dto.DateOfBirth,
        Gender         = dto.Gender,
        WorkEmail      = dto.WorkEmail,
        AltEmail       = dto.AltEmail,
        PhoneMobile    = dto.PhoneMobile,
        EmploymentType = dto.EmploymentType,
        ContractType   = dto.ContractType,
        HireDate       = dto.HireDate,
        EndDate        = dto.EndDate,
        WorkLocation   = dto.WorkLocation,
        JobTitleId     = dto.JobTitleId,
        JobGroupId     = dto.JobGroupId,
        DepartmentId   = dto.DepartmentId,
        ProfilePhotoUrl = dto.ProfilePhotoUrl,
        AccountStatus  = dto.AccountStatus,
        CreatedAt      = dto.CreatedAt,
        UpdatedAt      = dto.UpdatedAt,
    };
}
