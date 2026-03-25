using AbsenceApp.Data.Context;
using AbsenceApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AbsenceApp.Data.Repositories;

public interface IStaffAbsenceRepository
{
    Task<IEnumerable<StaffAbsence>> GetByStaffAsync(long staffId);
    Task<StaffAbsence?>             GetByIdAsync(long id);
}

public class StaffAbsenceRepository : IStaffAbsenceRepository
{
    private readonly AppDbContext _context;
    public StaffAbsenceRepository(AppDbContext context) => _context = context;

    public async Task<IEnumerable<StaffAbsence>> GetByStaffAsync(long staffId) =>
        await _context.StaffAbsences.Where(a => a.StaffId == staffId).ToListAsync();

    public async Task<StaffAbsence?> GetByIdAsync(long id) =>
        await _context.StaffAbsences.FindAsync(id);
}
