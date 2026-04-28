using AbsenceApp.Core.DTOs;
using AbsenceApp.Data.Models;

namespace AbsenceApp.Data.Mappers;

public static class HouseMapper
{
    public static HouseDto ToDto(House e) => new()
    {
        Id          = e.Id,
        Name        = e.Name,
        Code        = e.Code,
        Description = e.Description,
        CreatedAt   = e.CreatedAt,
        UpdatedAt   = e.UpdatedAt
    };

    public static House ToEntity(HouseDto dto) => new()
    {
        Id          = dto.Id,
        Name        = dto.Name,
        Code        = dto.Code,
        Description = dto.Description,
        CreatedAt   = dto.CreatedAt,
        UpdatedAt   = dto.UpdatedAt
    };
}
