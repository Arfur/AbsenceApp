using AbsenceApp.Data.Context;
using AbsenceApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AbsenceApp.Data.Repositories;

public interface IDepartmentRepository
{
    Task<IEnumerable<StaffDepartment>> GetAllAsync();
    Task<StaffDepartment?>             GetByIdAsync(long id);
}

public class DepartmentRepository : IDepartmentRepository
{
    private readonly AppDbContext _context;
    public DepartmentRepository(AppDbContext context) => _context = context;

    public async Task<IEnumerable<StaffDepartment>> GetAllAsync() =>
        await _context.StaffDepartments.ToListAsync();

    public async Task<StaffDepartment?> GetByIdAsync(long id) =>
        await _context.StaffDepartments.FindAsync((int)id);
}
