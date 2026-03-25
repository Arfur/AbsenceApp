using AbsenceApp.Data.Context;
using AbsenceApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AbsenceApp.Data.Repositories;

public interface IAbsenceTypeRepository
{
    Task<IEnumerable<AbsenceType>> GetAllAsync();
    Task<AbsenceType?>             GetByIdAsync(long id);
}

public class AbsenceTypeRepository : IAbsenceTypeRepository
{
    private readonly AppDbContext _context;
    public AbsenceTypeRepository(AppDbContext context) => _context = context;

    public async Task<IEnumerable<AbsenceType>> GetAllAsync() =>
        await _context.AbsenceTypes.ToListAsync();

    public async Task<AbsenceType?> GetByIdAsync(long id) =>
        await _context.AbsenceTypes.FindAsync(id);
}
