using AbsenceApp.Core.DTOs;
using AbsenceApp.Data.Models;

namespace AbsenceApp.Data.Mappers;

public static class YearGroupMapper
{
    public static YearGroupDto ToDto(YearGroup e) => new()
    {
        Id           = e.Id,
        Name         = e.Name,
        Code         = e.Code,
        NumericValue = e.NumericValue,
        PhaseId      = e.PhaseId,
        SchoolId     = e.SchoolId,
        CreatedAt    = e.CreatedAt,
        UpdatedAt    = e.UpdatedAt
    };

    public static YearGroup ToEntity(YearGroupDto dto) => new()
    {
        Id           = dto.Id,
        Name         = dto.Name,
        Code         = dto.Code,
        NumericValue = dto.NumericValue,
        PhaseId      = dto.PhaseId,
        SchoolId     = dto.SchoolId,
        CreatedAt    = dto.CreatedAt,
        UpdatedAt    = dto.UpdatedAt
    };
}
