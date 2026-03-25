using AbsenceApp.Core.DTOs;

namespace AbsenceApp.Core.Interfaces;

public interface IStudentContactService
{
    Task<IEnumerable<StudentContactDto>> GetByStudentAsync(long studentId);
    Task<StudentContactDto?>             GetByIdAsync(long id);
}
