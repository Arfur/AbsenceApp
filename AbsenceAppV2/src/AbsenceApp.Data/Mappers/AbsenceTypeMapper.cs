using AbsenceApp.Core.DTOs;
using AbsenceApp.Data.Models;

namespace AbsenceApp.Data.Mappers;

public static class AbsenceTypeMapper
{
    public static AbsenceTypeDto ToDto(AbsenceType e) => new()
    {
        Id           = e.Id,
        Code         = e.Code,
        Name         = e.Name,
        Category     = e.Category,
        IsAuthorised = e.IsAuthorised,
        CreatedAt    = e.CreatedAt
    };

    public static AbsenceType ToEntity(AbsenceTypeDto dto) => new()
    {
        Id           = dto.Id,
        Code         = dto.Code,
        Name         = dto.Name,
        Category     = dto.Category,
        IsAuthorised = dto.IsAuthorised,
        CreatedAt    = dto.CreatedAt
    };
}
