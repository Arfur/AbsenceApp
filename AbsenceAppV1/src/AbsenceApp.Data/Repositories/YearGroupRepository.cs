using AbsenceApp.Data.Context;
using AbsenceApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AbsenceApp.Data.Repositories;

public interface IYearGroupRepository
{
    Task<IEnumerable<YearGroup>> GetAllAsync();
    Task<YearGroup?>             GetByIdAsync(long id);
}

public class YearGroupRepository : IYearGroupRepository
{
    private readonly AppDbContext _context;
    public YearGroupRepository(AppDbContext context) => _context = context;

    public async Task<IEnumerable<YearGroup>> GetAllAsync() =>
        await _context.YearGroups.ToListAsync();

    public async Task<YearGroup?> GetByIdAsync(long id) =>
        await _context.YearGroups.FindAsync(id);
}
