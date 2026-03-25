using AbsenceApp.Core.DTOs;
using AbsenceApp.Data.Models;

namespace AbsenceApp.Data.Mappers;

public static class StudentAbsenceMapper
{
    public static StudentAbsenceDto ToDto(StudentAbsence e) => new()
    {
        Id            = e.Id,
        StudentId     = e.StudentId,
        AbsenceTypeId = e.AbsenceTypeId,
        Date          = e.Date,
        StartTime     = e.StartTime,
        EndTime       = e.EndTime,
        IsAuthorised  = e.IsAuthorised,
        Notes         = e.Notes,
        RecordedBy    = e.RecordedBy,
        CreatedAt     = e.CreatedAt,
        UpdatedAt     = e.UpdatedAt
    };

    public static StudentAbsence ToEntity(StudentAbsenceDto dto) => new()
    {
        Id            = dto.Id,
        StudentId     = dto.StudentId,
        AbsenceTypeId = dto.AbsenceTypeId,
        Date          = dto.Date,
        StartTime     = dto.StartTime,
        EndTime       = dto.EndTime,
        IsAuthorised  = dto.IsAuthorised,
        Notes         = dto.Notes,
        RecordedBy    = dto.RecordedBy,
        CreatedAt     = dto.CreatedAt,
        UpdatedAt     = dto.UpdatedAt
    };
}
