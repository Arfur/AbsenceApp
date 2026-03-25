using AbsenceApp.Core.DTOs;
using AbsenceApp.Data.Models;

namespace AbsenceApp.Data.Mappers;

public static class AttendanceRegisterMapper
{
    public static AttendanceRegisterDto ToDto(AttendanceRegister e) => new()
    {
        Id        = e.Id,
        ClassId   = e.ClassId,
        Date      = e.Date,
        Session   = e.Session,
        OpenedBy  = e.OpenedBy,
        ClosedBy  = e.ClosedBy,
        Status    = e.Status,
        CreatedAt = e.CreatedAt,
        UpdatedAt = e.UpdatedAt
    };

    public static AttendanceMarkDto MarkToDto(AttendanceMark e) => new()
    {
        Id                   = e.Id,
        AttendanceRegisterId = e.AttendanceRegisterId,
        StudentId            = e.StudentId,
        MarkCode             = e.MarkCode,
        IsLate               = e.IsLate,
        MinutesLate          = e.MinutesLate,
        Notes                = e.Notes,
        RecordedBy           = e.RecordedBy,
        CreatedAt            = e.CreatedAt,
        UpdatedAt            = e.UpdatedAt
    };
}
