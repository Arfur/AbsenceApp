using AbsenceApp.Core.DTOs;

namespace AbsenceApp.Core.Interfaces;

public interface IAbsenceStatusService
{
    Task<IEnumerable<AbsenceStatusDto>> GetAllAsync();
    Task<AbsenceStatusDto?>             GetByIdAsync(long id);
}
