using AbsenceApp.Core.DTOs;

namespace AbsenceApp.Core.Interfaces;

public interface IHouseService
{
    Task<IEnumerable<HouseDto>> GetAllAsync();
    Task<HouseDto?>             GetByIdAsync(long id);
}
