using AbsenceApp.Data.Context;
using AbsenceApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AbsenceApp.Data.Repositories;

public interface IAbsenceStatusRepository
{
    Task<IEnumerable<AbsenceStatus>> GetAllAsync();
    Task<AbsenceStatus?>             GetByIdAsync(long id);
}

public class AbsenceStatusRepository : IAbsenceStatusRepository
{
    private readonly AppDbContext _context;
    public AbsenceStatusRepository(AppDbContext context) => _context = context;

    public async Task<IEnumerable<AbsenceStatus>> GetAllAsync() =>
        await _context.AbsenceStatuses.ToListAsync();

    public async Task<AbsenceStatus?> GetByIdAsync(long id) =>
        await _context.AbsenceStatuses.FindAsync(id);
}
