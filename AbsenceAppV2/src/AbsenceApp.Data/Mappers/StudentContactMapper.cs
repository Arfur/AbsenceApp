using AbsenceApp.Core.DTOs;
using AbsenceApp.Data.Models;

namespace AbsenceApp.Data.Mappers;

public static class StudentContactMapper
{
    public static StudentContactDto ToDto(StudentContact e) => new()
    {
        Id           = e.Id,
        StudentId    = e.StudentId,
        ContactName  = e.ContactName,
        Relationship = e.Relationship,
        Phone        = e.Phone,
        Email        = e.Email,
        IsPrimary    = e.IsPrimary,
        CreatedAt    = e.CreatedAt,
        UpdatedAt    = e.UpdatedAt
    };

    public static StudentContact ToEntity(StudentContactDto dto) => new()
    {
        Id           = dto.Id,
        StudentId    = dto.StudentId,
        ContactName  = dto.ContactName,
        Relationship = dto.Relationship,
        Phone        = dto.Phone,
        Email        = dto.Email,
        IsPrimary    = dto.IsPrimary,
        CreatedAt    = dto.CreatedAt,
        UpdatedAt    = dto.UpdatedAt
    };
}
