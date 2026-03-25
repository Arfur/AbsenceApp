using AbsenceApp.Data.Context;
using AbsenceApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AbsenceApp.Data.Repositories;

public interface IHouseRepository
{
    Task<IEnumerable<House>> GetAllAsync();
    Task<House?>             GetByIdAsync(long id);
}

public class HouseRepository : IHouseRepository
{
    private readonly AppDbContext _context;
    public HouseRepository(AppDbContext context) => _context = context;

    public async Task<IEnumerable<House>> GetAllAsync() =>
        await _context.Houses.ToListAsync();

    public async Task<House?> GetByIdAsync(long id) =>
        await _context.Houses.FindAsync(id);
}
