using AbsenceApp.Core.DTOs;
using AbsenceApp.Core.Interfaces;
using AbsenceApp.Data.Mappers;
using AbsenceApp.Data.Repositories;

namespace AbsenceApp.Data.Services;

public class YearGroupService : IYearGroupService
{
    private readonly IYearGroupRepository _repo;
    public YearGroupService(IYearGroupRepository repo) => _repo = repo;

    public async Task<IEnumerable<YearGroupDto>> GetAllAsync()
    {
        var entities = await _repo.GetAllAsync();
        return entities.Select(YearGroupMapper.ToDto);
    }

    public async Task<YearGroupDto?> GetByIdAsync(long id)
    {
        var entity = await _repo.GetByIdAsync(id);
        return entity is null ? null : YearGroupMapper.ToDto(entity);
    }
}
