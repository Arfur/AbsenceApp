using AbsenceApp.Core.DTOs;
using AbsenceApp.Core.Interfaces;
using AbsenceApp.Data.Mappers;
using AbsenceApp.Data.Repositories;

namespace AbsenceApp.Data.Services;

public class AbsenceStatusService : IAbsenceStatusService
{
    private readonly IAbsenceStatusRepository _repo;
    public AbsenceStatusService(IAbsenceStatusRepository repo) => _repo = repo;

    public async Task<IEnumerable<AbsenceStatusDto>> GetAllAsync()
    {
        var entities = await _repo.GetAllAsync();
        return entities.Select(AbsenceStatusMapper.ToDto);
    }

    public async Task<AbsenceStatusDto?> GetByIdAsync(long id)
    {
        var entity = await _repo.GetByIdAsync(id);
        return entity is null ? null : AbsenceStatusMapper.ToDto(entity);
    }
}
