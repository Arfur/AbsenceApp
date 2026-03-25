using AbsenceApp.Core.DTOs;
using AbsenceApp.Data.Models;

namespace AbsenceApp.Data.Mappers;

public static class StudentContactMapper
{
    public static StudentContactDto ToDto(StudentContact e) => new()
    {
        Id                       = e.Id,
        StudentId                = e.StudentId,
        ContactName              = e.ContactName,
        Relationship             = e.Relationship,
        PhoneMobile              = e.PhoneMobile,
        PhoneHome                = e.PhoneHome,
        Email                    = e.Email,
        Priority                 = e.Priority,
        LivesWithStudent         = e.LivesWithStudent,
        HasParentalResponsibility = e.HasParentalResponsibility,
        SafeguardingFlag         = e.SafeguardingFlag,
        Notes                    = e.Notes,
        CreatedAt                = e.CreatedAt,
        UpdatedAt                = e.UpdatedAt
    };

    public static StudentContact ToEntity(StudentContactDto dto) => new()
    {
        Id                       = dto.Id,
        StudentId                = dto.StudentId,
        ContactName              = dto.ContactName,
        Relationship             = dto.Relationship,
        PhoneMobile              = dto.PhoneMobile,
        PhoneHome                = dto.PhoneHome,
        Email                    = dto.Email,
        Priority                 = dto.Priority,
        LivesWithStudent         = dto.LivesWithStudent,
        HasParentalResponsibility = dto.HasParentalResponsibility,
        SafeguardingFlag         = dto.SafeguardingFlag,
        Notes                    = dto.Notes,
        CreatedAt                = dto.CreatedAt,
        UpdatedAt                = dto.UpdatedAt
    };
}
