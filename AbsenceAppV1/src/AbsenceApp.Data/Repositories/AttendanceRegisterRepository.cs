using AbsenceApp.Data.Context;
using AbsenceApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AbsenceApp.Data.Repositories;

public interface IAttendanceRegisterRepository
{
    Task<IEnumerable<AttendanceRegister>> GetByClassAsync(long classId);
    Task<AttendanceRegister?>             GetByIdAsync(long id);
    Task<IEnumerable<AttendanceMark>>     GetMarksAsync(long registerId);
}

public class AttendanceRegisterRepository : IAttendanceRegisterRepository
{
    private readonly AppDbContext _context;
    public AttendanceRegisterRepository(AppDbContext context) => _context = context;

    public async Task<IEnumerable<AttendanceRegister>> GetByClassAsync(long classId) =>
        await _context.AttendanceRegisters.Where(r => r.ClassId == classId).ToListAsync();

    public async Task<AttendanceRegister?> GetByIdAsync(long id) =>
        await _context.AttendanceRegisters.FindAsync(id);

    public async Task<IEnumerable<AttendanceMark>> GetMarksAsync(long registerId) =>
        await _context.AttendanceMarks.Where(m => m.AttendanceRegisterId == registerId).ToListAsync();
}
