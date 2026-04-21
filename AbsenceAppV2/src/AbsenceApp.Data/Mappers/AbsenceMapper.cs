using AbsenceApp.Core.DTOs;
using AbsenceApp.Data.Models;

namespace AbsenceApp.Data.Mappers;

public static class AbsenceMapper
{
    public static AbsenceDto ToDto(Absence e) => new()
    {
        Id              = e.Id,
        PersonType      = e.PersonType,
        PersonId        = e.PersonId,
        AbsenceTypeId   = e.AbsenceTypeId,
        AbsenceTypeName = e.AbsenceType?.Name ?? string.Empty,
        StatusId        = e.StatusId,
        StatusName      = e.Status?.Name ?? string.Empty,
        StartDate       = e.StartDate,
        EndDate         = e.EndDate,
        DurationDays    = e.DurationDays,
        ReportedVia     = e.ReportedVia,
        Notes           = e.Notes,
        RecordedBy      = e.RecordedBy,
        ApprovedBy      = e.ApprovedBy,
        ApprovedAt      = e.ApprovedAt,
        CreatedAt       = e.CreatedAt,
        UpdatedAt       = e.UpdatedAt
    };

    public static Absence ToEntity(CreateAbsenceDto dto, long pendingStatusId) => new()
    {
        PersonType    = dto.PersonType,
        PersonId      = dto.PersonId,
        AbsenceTypeId = dto.AbsenceTypeId,
        StatusId      = pendingStatusId,
        StartDate     = dto.StartDate,
        EndDate       = dto.EndDate,
        DurationDays  = (dto.EndDate.DayNumber - dto.StartDate.DayNumber) + 1,
        ReportedVia   = dto.ReportedVia,
        Notes         = dto.Notes,
        RecordedBy    = dto.RecordedBy,
        CreatedAt     = DateTime.UtcNow,
        UpdatedAt     = DateTime.UtcNow
    };
}
