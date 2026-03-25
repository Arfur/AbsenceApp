using AbsenceApp.Core.DTOs;
using AbsenceApp.Core.Interfaces;
using AbsenceApp.Data.Mappers;
using AbsenceApp.Data.Repositories;

namespace AbsenceApp.Data.Services;

public class StudentContactService : IStudentContactService
{
    private readonly IStudentContactRepository _repo;
    public StudentContactService(IStudentContactRepository repo) => _repo = repo;

    public async Task<IEnumerable<StudentContactDto>> GetByStudentAsync(long studentId)
    {
        var entities = await _repo.GetByStudentAsync(studentId);
        return entities.Select(StudentContactMapper.ToDto);
    }

    public async Task<StudentContactDto?> GetByIdAsync(long id)
    {
        var entity = await _repo.GetByIdAsync(id);
        return entity is null ? null : StudentContactMapper.ToDto(entity);
    }
}
