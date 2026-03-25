using AbsenceApp.Data.Context;
using AbsenceApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AbsenceApp.Data.Repositories;

public interface IStudentContactRepository
{
    Task<IEnumerable<StudentContact>> GetByStudentAsync(long studentId);
    Task<StudentContact?>             GetByIdAsync(long id);
}

public class StudentContactRepository : IStudentContactRepository
{
    private readonly AppDbContext _context;
    public StudentContactRepository(AppDbContext context) => _context = context;

    public async Task<IEnumerable<StudentContact>> GetByStudentAsync(long studentId) =>
        await _context.StudentContacts.Where(c => c.StudentId == studentId).ToListAsync();

    public async Task<StudentContact?> GetByIdAsync(long id) =>
        await _context.StudentContacts.FindAsync(id);
}
