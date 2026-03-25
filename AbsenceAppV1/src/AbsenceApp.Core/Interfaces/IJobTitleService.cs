using AbsenceApp.Core.DTOs;

namespace AbsenceApp.Core.Interfaces;

public interface IJobTitleService
{
    Task<IEnumerable<JobTitleDto>> GetAllAsync();
    Task<JobTitleDto?>             GetByIdAsync(long id);
}
