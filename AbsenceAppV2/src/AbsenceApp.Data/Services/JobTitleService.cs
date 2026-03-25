using AbsenceApp.Core.DTOs;
using AbsenceApp.Core.Interfaces;
using AbsenceApp.Data.Mappers;
using AbsenceApp.Data.Repositories;

namespace AbsenceApp.Data.Services;

public class JobTitleService : IJobTitleService
{
    private readonly IJobTitleRepository _repo;
    public JobTitleService(IJobTitleRepository repo) => _repo = repo;

    public async Task<IEnumerable<JobTitleDto>> GetAllAsync()
    {
        var entities = await _repo.GetAllAsync();
        return entities.Select(JobTitleMapper.ToDto);
    }

    public async Task<JobTitleDto?> GetByIdAsync(long id)
    {
        var entity = await _repo.GetByIdAsync(id);
        return entity is null ? null : JobTitleMapper.ToDto(entity);
    }
}
