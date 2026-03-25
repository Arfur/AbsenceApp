using AbsenceApp.Core.DTOs;
using AbsenceApp.Core.Interfaces;
using AbsenceApp.Data.Mappers;
using AbsenceApp.Data.Repositories;

namespace AbsenceApp.Data.Services;

public class AttendanceRegisterService : IAttendanceRegisterService
{
    private readonly IAttendanceRegisterRepository _repo;
    public AttendanceRegisterService(IAttendanceRegisterRepository repo) => _repo = repo;

    public async Task<IEnumerable<AttendanceRegisterDto>> GetByClassAsync(long classId)
    {
        var entities = await _repo.GetByClassAsync(classId);
        return entities.Select(AttendanceRegisterMapper.ToDto);
    }

    public async Task<AttendanceRegisterDto?> GetByIdAsync(long id)
    {
        var entity = await _repo.GetByIdAsync(id);
        return entity is null ? null : AttendanceRegisterMapper.ToDto(entity);
    }

    public async Task<IEnumerable<AttendanceMarkDto>> GetMarksAsync(long registerId)
    {
        var marks = await _repo.GetMarksAsync(registerId);
        return marks.Select(AttendanceRegisterMapper.MarkToDto);
    }
}
