using AbsenceApp.Core.DTOs;

namespace AbsenceApp.Core.Interfaces;

public interface IAttendanceRegisterService
{
    Task<IEnumerable<AttendanceRegisterDto>> GetByClassAsync(long classId);
    Task<AttendanceRegisterDto?>             GetByIdAsync(long id);
    Task<IEnumerable<AttendanceMarkDto>>     GetMarksAsync(long registerId);
}
