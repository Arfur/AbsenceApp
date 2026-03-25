using AbsenceApp.Core.DTOs;
using DataStudent = AbsenceApp.Data.Models.Student;

namespace AbsenceApp.Data.Mappers;

/// <summary>Maps between the EF Student entity (TABLE29) and StudentDto.</summary>
public static class StudentMapper
{
    public static StudentDto ToDto(DataStudent entity) => new()
    {
        Id              = entity.Id,
        AdmissionNumber = entity.AdmissionNumber,
        FirstName       = entity.FirstName,
        MiddleNames     = entity.MiddleNames,
        LastName        = entity.LastName,
        LegalFirstName  = entity.LegalFirstName,
        LegalLastName   = entity.LegalLastName,
        Gender          = entity.Gender,
        DateOfBirth     = entity.DateOfBirth,
        YearGroupId     = entity.YearGroupId,
        ClassId         = entity.ClassId,
        HouseId         = entity.HouseId,
        Username        = entity.Username,
        Upn             = entity.Upn,
        SchoolId        = entity.SchoolId,
        AdmissionDate   = entity.AdmissionDate,
        Status          = entity.Status,
        CreatedAt       = entity.CreatedAt,
        UpdatedAt       = entity.UpdatedAt,
    };

    public static DataStudent ToEntity(StudentDto dto) => new()
    {
        Id              = dto.Id,
        AdmissionNumber = dto.AdmissionNumber,
        FirstName       = dto.FirstName,
        MiddleNames     = dto.MiddleNames,
        LastName        = dto.LastName,
        LegalFirstName  = dto.LegalFirstName,
        LegalLastName   = dto.LegalLastName,
        Gender          = dto.Gender,
        DateOfBirth     = dto.DateOfBirth,
        YearGroupId     = dto.YearGroupId,
        ClassId         = dto.ClassId,
        HouseId         = dto.HouseId,
        Username        = dto.Username,
        Upn             = dto.Upn,
        SchoolId        = dto.SchoolId,
        AdmissionDate   = dto.AdmissionDate,
        Status          = dto.Status,
        CreatedAt       = dto.CreatedAt,
        UpdatedAt       = dto.UpdatedAt,
    };
}
