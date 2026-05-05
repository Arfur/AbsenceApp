/*
===============================================================================
 File        : AbsenceRepository.cs
 Namespace   : AbsenceApp.Data.Repositories
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-05-04
 Updated     : 2026-05-04
-------------------------------------------------------------------------------
 Purpose     : EF Core implementation of IAbsenceRepository.
               Persists and retrieves Absence entities via AppDbContext.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-05-04  Fix Plan #2 Step 3: added file header. Changed
                         IAbsenceRepository and AbsenceRepository parameter
                         types from ulong to long in GetByPersonAsync and
                         GetByIdAsync to match Absence.PersonId / Absence.Id
                         (both long after Phase 2 type alignment).
   - 1.1.0  2026-05-05  Student Absence Management: added DeleteAsync(long id)
                         and UpdateAsync(Absence entity) to interface and class.
===============================================================================
*/
using AbsenceApp.Data.Context;
using AbsenceApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AbsenceApp.Data.Repositories;

public interface IAbsenceRepository
{
    Task<IEnumerable<Absence>> GetByPersonAsync(string personType, long personId);
    Task<Absence?>             GetByIdAsync(long id);
    Task<Absence>              AddAsync(Absence entity);
    Task                       UpdateAsync(Absence entity);
    Task                       DeleteAsync(long id);
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

    public async Task UpdateAsync(Absence entity)
    {
        _context.Absences.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(long id)
    {
        var entity = await _context.Absences.FindAsync(id);
        if (entity is null) return;
        _context.Absences.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task SaveChangesAsync() =>
        await _context.SaveChangesAsync();
}
