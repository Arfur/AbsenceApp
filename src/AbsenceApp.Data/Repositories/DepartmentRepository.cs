using AbsenceApp.Data.Context;
using AbsenceApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AbsenceApp.Data.Repositories;

public interface IDepartmentRepository
{
    Task<IEnumerable<Department>> GetAllAsync();
    Task<Department?>             GetByIdAsync(long id);
}

public class DepartmentRepository : IDepartmentRepository
{
    private readonly AppDbContext _context;
    public DepartmentRepository(AppDbContext context) => _context = context;

    public async Task<IEnumerable<Department>> GetAllAsync() =>
        await _context.Departments.ToListAsync();

    public async Task<Department?> GetByIdAsync(long id) =>
        await _context.Departments.FindAsync(id);
}
