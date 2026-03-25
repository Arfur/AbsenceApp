using AbsenceApp.Core.DTOs;

namespace AbsenceApp.Core.Interfaces;

public interface IDepartmentService
{
    Task<IEnumerable<DepartmentDto>> GetAllAsync();
    Task<DepartmentDto?>             GetByIdAsync(long id);
}
