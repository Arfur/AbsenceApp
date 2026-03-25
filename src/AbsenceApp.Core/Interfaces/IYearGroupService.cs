using AbsenceApp.Core.DTOs;

namespace AbsenceApp.Core.Interfaces;

public interface IYearGroupService
{
    Task<IEnumerable<YearGroupDto>> GetAllAsync();
    Task<YearGroupDto?>             GetByIdAsync(long id);
}
