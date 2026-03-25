using AbsenceApp.Core.DTOs;

namespace AbsenceApp.Core.Interfaces;

public interface IStudentAbsenceService
{
    Task<IEnumerable<StudentAbsenceDto>> GetByStudentAsync(long studentId);
    Task<StudentAbsenceDto?>             GetByIdAsync(long id);
    Task                                 CreateAsync(StudentAbsenceDto dto);
}
