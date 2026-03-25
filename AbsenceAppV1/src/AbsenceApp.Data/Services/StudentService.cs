using AbsenceApp.Core.DTOs;
using AbsenceApp.Core.Interfaces;
using AbsenceApp.Data.Mappers;
using AbsenceApp.Data.Repositories;

namespace AbsenceApp.Data.Services;

public class StudentService : IStudentService
{
    private readonly IStudentRepository _repository;
    public StudentService(IStudentRepository repository) => _repository = repository;
    public async Task<IEnumerable<StudentDto>> GetAllAsync()
    {
        var entities = await _repository.GetAllAsync();
        return entities.Select(StudentMapper.ToDto);
    }
    public async Task<StudentDto?> GetByIdAsync(long id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return entity is null ? null : StudentMapper.ToDto(entity);
    }
}
