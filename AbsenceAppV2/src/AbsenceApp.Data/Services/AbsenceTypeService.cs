using AbsenceApp.Core.DTOs;
using AbsenceApp.Core.Interfaces;
using AbsenceApp.Data.Mappers;
using AbsenceApp.Data.Repositories;

namespace AbsenceApp.Data.Services;

public class AbsenceTypeService : IAbsenceTypeService
{
    private readonly IAbsenceTypeRepository _repo;
    public AbsenceTypeService(IAbsenceTypeRepository repo) => _repo = repo;

    public async Task<IEnumerable<AbsenceTypeDto>> GetAllAsync()
    {
        var entities = await _repo.GetAllAsync();
        return entities.Select(AbsenceTypeMapper.ToDto);
    }

    public async Task<AbsenceTypeDto?> GetByIdAsync(long id)
    {
        var entity = await _repo.GetByIdAsync(id);
        return entity is null ? null : AbsenceTypeMapper.ToDto(entity);
    }
}
