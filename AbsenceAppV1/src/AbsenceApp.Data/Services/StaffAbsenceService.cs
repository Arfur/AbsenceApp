using AbsenceApp.Core.DTOs;
using AbsenceApp.Core.Interfaces;
using AbsenceApp.Data.Mappers;
using AbsenceApp.Data.Repositories;

namespace AbsenceApp.Data.Services;

public class StaffAbsenceService : IStaffAbsenceService
{
    private readonly IStaffAbsenceRepository _repo;
    public StaffAbsenceService(IStaffAbsenceRepository repo) => _repo = repo;

    public async Task<IEnumerable<StaffAbsenceDto>> GetByStaffAsync(long staffId)
    {
        var entities = await _repo.GetByStaffAsync(staffId);
        return entities.Select(StaffAbsenceMapper.ToDto);
    }

    public async Task<StaffAbsenceDto?> GetByIdAsync(long id)
    {
        var entity = await _repo.GetByIdAsync(id);
        return entity is null ? null : StaffAbsenceMapper.ToDto(entity);
    }
}
