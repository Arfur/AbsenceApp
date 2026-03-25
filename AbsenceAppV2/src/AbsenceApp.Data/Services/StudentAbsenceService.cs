using AbsenceApp.Core.DTOs;
using AbsenceApp.Core.Interfaces;
using AbsenceApp.Data.Mappers;
using AbsenceApp.Data.Repositories;

namespace AbsenceApp.Data.Services;

public class StudentAbsenceService : IStudentAbsenceService
{
    private readonly IStudentAbsenceRepository _repo;

    public StudentAbsenceService(IStudentAbsenceRepository repo) => _repo = repo;

    public async Task<IEnumerable<StudentAbsenceDto>> GetByStudentAsync(long studentId)
    {
        var entities = await _repo.GetByStudentAsync(studentId);
        return entities.Select(StudentAbsenceMapper.ToDto);
    }

    public async Task<StudentAbsenceDto?> GetByIdAsync(long id)
    {
        var entity = await _repo.GetByIdAsync(id);
        return entity is null ? null : StudentAbsenceMapper.ToDto(entity);
    }

    public async Task CreateAsync(StudentAbsenceDto dto)
    {
        var entity = StudentAbsenceMapper.ToEntity(dto);
        entity.CreatedAt = DateTime.UtcNow;
        entity.UpdatedAt = DateTime.UtcNow;
        await _repo.AddAsync(entity);
    }
}
