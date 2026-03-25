using AbsenceApp.Data.Context;
using AbsenceApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AbsenceApp.Data.Repositories;

public interface IStudentAbsenceRepository
{
    Task<IEnumerable<StudentAbsence>> GetByStudentAsync(long studentId);
    Task<StudentAbsence?>             GetByIdAsync(long id);
    Task                              AddAsync(StudentAbsence entity);
}

public class StudentAbsenceRepository : IStudentAbsenceRepository
{
    private readonly AppDbContext _context;

    public StudentAbsenceRepository(AppDbContext context) => _context = context;

    public async Task<IEnumerable<StudentAbsence>> GetByStudentAsync(long studentId) =>
        await _context.StudentAbsences.Where(a => a.StudentId == studentId).ToListAsync();

    public async Task<StudentAbsence?> GetByIdAsync(long id) =>
        await _context.StudentAbsences.FindAsync(id);

    public async Task AddAsync(StudentAbsence entity)
    {
        _context.StudentAbsences.Add(entity);
        await _context.SaveChangesAsync();
    }
}
