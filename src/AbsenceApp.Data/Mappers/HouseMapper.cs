using AbsenceApp.Core.DTOs;
using AbsenceApp.Data.Models;

namespace AbsenceApp.Data.Mappers;

public static class HouseMapper
{
    public static HouseDto ToDto(House e) => new()
    {
        Id        = e.Id,
        Name      = e.Name,
        Colour    = e.Colour,
        Code      = e.Code,
        SchoolId  = e.SchoolId,
        CreatedAt = e.CreatedAt,
        UpdatedAt = e.UpdatedAt
    };

    public static House ToEntity(HouseDto dto) => new()
    {
        Id        = dto.Id,
        Name      = dto.Name,
        Colour    = dto.Colour,
        Code      = dto.Code,
        SchoolId  = dto.SchoolId,
        CreatedAt = dto.CreatedAt,
        UpdatedAt = dto.UpdatedAt
    };
}
