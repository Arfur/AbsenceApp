using AbsenceApp.Core.DTOs;

namespace AbsenceApp.Core.Interfaces;

public interface IStaffService
{
    Task<IEnumerable<StaffDto>> GetAllAsync();
    Task<StaffDto?> GetByIdAsync(long id);
}
