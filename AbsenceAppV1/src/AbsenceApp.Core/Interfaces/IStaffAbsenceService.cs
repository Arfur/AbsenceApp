using AbsenceApp.Core.DTOs;

namespace AbsenceApp.Core.Interfaces;

public interface IStaffAbsenceService
{
    Task<IEnumerable<StaffAbsenceDto>> GetByStaffAsync(long staffId);
    Task<StaffAbsenceDto?>             GetByIdAsync(long id);
}
