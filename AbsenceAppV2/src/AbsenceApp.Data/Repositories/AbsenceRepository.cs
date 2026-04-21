using AbsenceApp.Data.Context;
using AbsenceApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AbsenceApp.Data.Repositories;

public interface IAbsenceRepository
{
    Task<IEnumerable<Absence>> GetByPersonAsync(string personType, long personId);
    Task<Absence?>             GetByIdAsync(long id);
    Task<Absence>              AddAsync(Absence entity);
    Task                       SaveChangesAsync();
}

public class AbsenceRepository : IAbsenceRepository
{
    private readonly AppDbContext _context;
    public AbsenceRepository(AppDbContext context) => _context = context;

    public async Task<IEnumerable<Absence>> GetByPersonAsync(string personType, long personId) =>
        await _context.Absences
            .Include(a => a.AbsenceType)
            .Include(a => a.Status)
            .Where(a => a.PersonType == personType && a.PersonId == personId)
            .OrderByDescending(a => a.StartDate)
            .ToListAsync();

    public async Task<Absence?> GetByIdAsync(long id) =>
        await _context.Absences
            .Include(a => a.AbsenceType)
            .Include(a => a.Status)
            .FirstOrDefaultAsync(a => a.Id == id);

    public async Task<Absence> AddAsync(Absence entity)
    {
        _context.Absences.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task SaveChangesAsync() =>
        await _context.SaveChangesAsync();
}
