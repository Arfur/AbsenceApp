using AbsenceApp.Core.DTOs;
using AbsenceApp.Core.Interfaces;
using AbsenceApp.Data.Mappers;
using AbsenceApp.Data.Repositories;

namespace AbsenceApp.Data.Services;

public class DepartmentService : IDepartmentService
{
    private readonly IDepartmentRepository _repo;
    public DepartmentService(IDepartmentRepository repo) => _repo = repo;

    public async Task<IEnumerable<DepartmentDto>> GetAllAsync()
    {
        var entities = await _repo.GetAllAsync();
        return entities.Select(DepartmentMapper.ToDto);
    }

    public async Task<DepartmentDto?> GetByIdAsync(long id)
    {
        var entity = await _repo.GetByIdAsync(id);
        return entity is null ? null : DepartmentMapper.ToDto(entity);
    }
}
