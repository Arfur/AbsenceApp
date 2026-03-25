using AbsenceApp.Core.DTOs;
using AbsenceApp.Data.Models;

namespace AbsenceApp.Data.Mappers;

public static class AbsenceTypeMapper
{
    public static AbsenceTypeDto ToDto(AbsenceType e) => new()
    {
        Id          = e.Id,
        Name        = e.Name,
        Code        = e.Code,
        Description = e.Description,
        IsPaid      = e.IsPaid
    };

    public static AbsenceType ToEntity(AbsenceTypeDto dto) => new()
    {
        Id          = dto.Id,
        Name        = dto.Name,
        Code        = dto.Code,
        Description = dto.Description,
        IsPaid      = dto.IsPaid
    };
}
