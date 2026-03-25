using AbsenceApp.Core.DTOs;
using AbsenceApp.Data.Models;

namespace AbsenceApp.Data.Mappers;

public static class StaffAbsenceMapper
{
    public static StaffAbsenceDto ToDto(StaffAbsence e) => new()
    {
        Id            = e.Id,
        StaffId       = e.StaffId,
        AbsenceTypeId = e.AbsenceTypeId,
        StartDate     = e.StartDate,
        EndDate       = e.EndDate,
        Notes         = e.Notes,
        CreatedAt     = e.CreatedAt,
        UpdatedAt     = e.UpdatedAt
    };

    public static StaffAbsence ToEntity(StaffAbsenceDto dto) => new()
    {
        Id            = dto.Id,
        StaffId       = dto.StaffId,
        AbsenceTypeId = dto.AbsenceTypeId,
        StartDate     = dto.StartDate,
        EndDate       = dto.EndDate,
        Notes         = dto.Notes,
        CreatedAt     = dto.CreatedAt,
        UpdatedAt     = dto.UpdatedAt
    };
}
