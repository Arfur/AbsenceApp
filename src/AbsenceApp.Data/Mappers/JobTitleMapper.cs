using AbsenceApp.Core.DTOs;
using AbsenceApp.Data.Models;

namespace AbsenceApp.Data.Mappers;

public static class JobTitleMapper
{
    public static JobTitleDto ToDto(JobTitle e) => new()
    {
        Id          = e.Id,
        Title       = e.Title,
        Code        = e.Code,
        Description = e.Description,
        CreatedAt   = e.CreatedAt,
        UpdatedAt   = e.UpdatedAt
    };

    public static JobTitle ToEntity(JobTitleDto dto) => new()
    {
        Id          = dto.Id,
        Title       = dto.Title,
        Code        = dto.Code,
        Description = dto.Description,
        CreatedAt   = dto.CreatedAt,
        UpdatedAt   = dto.UpdatedAt
    };
}
