using AbsenceApp.Core.DTOs;
using AbsenceApp.Core.Interfaces;
using AbsenceApp.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace AbsenceApp.Data.Services;

public class DepartmentService : IDepartmentService
{
    private readonly AppDbContext _db;

    public DepartmentService(AppDbContext db) => _db = db;

    public async Task<IEnumerable<DepartmentDto>> GetAllAsync()
    {
        var ids = await _db.Staff
            .AsNoTracking()
            .Where(s => s.DepartmentId > 0)
            .Select(s => s.DepartmentId)
            .Distinct()
            .OrderBy(id => id)
            .ToListAsync();

        return ids.Select(id => new DepartmentDto
        {
            Id = id,
            Name = $"Department {id}",
            Code = $"D{id}",
            Status = "active",
        });
    }

    public async Task<DepartmentDto?> GetByIdAsync(long id)
    {
        var exists = await _db.Staff
            .AsNoTracking()
            .AnyAsync(s => s.DepartmentId == id);

        return exists
            ? new DepartmentDto { Id = (int)id, Name = $"Department {id}", Code = $"D{id}", Status = "active" }
            : null;
    }
}
