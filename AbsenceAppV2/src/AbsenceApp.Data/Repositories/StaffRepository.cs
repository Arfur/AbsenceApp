using AbsenceApp.Data.Context;
using AbsenceApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AbsenceApp.Data.Repositories;

public interface IStaffRepository
{
    Task<IEnumerable<Staff>> GetAllAsync();
    Task<Staff?> GetByIdAsync(long id);
}

public class StaffRepository : IStaffRepository
{
    private readonly AppDbContext _context;

    public StaffRepository(AppDbContext context) => _context = context;

    public async Task<IEnumerable<Staff>> GetAllAsync() =>
        await _context.Staff.ToListAsync();

    public async Task<Staff?> GetByIdAsync(long id) =>
        await _context.Staff.FindAsync(id);
}
