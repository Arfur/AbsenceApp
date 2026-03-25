using AbsenceApp.Core.DTOs;

namespace AbsenceApp.Core.Interfaces;

public interface IAbsenceTypeService
{
    Task<IEnumerable<AbsenceTypeDto>> GetAllAsync();
    Task<AbsenceTypeDto?>             GetByIdAsync(long id);
}
