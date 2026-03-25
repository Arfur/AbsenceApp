using AbsenceApp.Core.DTOs;
using AbsenceApp.Core.Interfaces;
using AbsenceApp.Data.Mappers;
using AbsenceApp.Data.Repositories;

namespace AbsenceApp.Data.Services;

public class HouseService : IHouseService
{
    private readonly IHouseRepository _repo;
    public HouseService(IHouseRepository repo) => _repo = repo;

    public async Task<IEnumerable<HouseDto>> GetAllAsync()
    {
        var entities = await _repo.GetAllAsync();
        return entities.Select(HouseMapper.ToDto);
    }

    public async Task<HouseDto?> GetByIdAsync(long id)
    {
        var entity = await _repo.GetByIdAsync(id);
        return entity is null ? null : HouseMapper.ToDto(entity);
    }
}
