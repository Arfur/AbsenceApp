using AbsenceApp.Core.DTOs;
using AbsenceApp.Core.Interfaces;
using AbsenceApp.Data.Mappers;
using AbsenceApp.Data.Repositories;

namespace AbsenceApp.Data.Services;

/// <summary>EF Core implementation of IStaffService.</summary>
public class StaffService : IStaffService
{
    private readonly IStaffRepository _repository;

    public StaffService(IStaffRepository repository) => _repository = repository;

    public async Task<IEnumerable<StaffDto>> GetAllAsync()
    {
        var entities = await _repository.GetAllAsync();
        return entities.Select(StaffMapper.ToDto);
    }

    public async Task<StaffDto?> GetByIdAsync(long id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return entity is null ? null : StaffMapper.ToDto(entity);
    }
}
