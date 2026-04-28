using AbsenceApp.Core.DTOs;
using AbsenceApp.Data.Models;

namespace AbsenceApp.Data.Mappers;

public static class DepartmentMapper
{
    public static DepartmentDto ToDto(StaffDepartment e) => new()
    {
        Id          = e.Id,
        Name        = e.Name,
        Code        = e.Code,
        Description = e.Description,
        HeadUserId  = e.HeadUserId,
        Status      = e.Status,
        CreatedAt   = e.CreatedAt,
        UpdatedAt   = e.UpdatedAt
    };

    public static StaffDepartment ToEntity(DepartmentDto dto) => new()
    {
        Id          = dto.Id,
        Name        = dto.Name,
        Code        = dto.Code,
        Description = dto.Description ?? string.Empty,
        HeadUserId  = dto.HeadUserId,
        Status      = dto.Status,
        CreatedAt   = dto.CreatedAt,
        UpdatedAt   = dto.UpdatedAt
    };
}
