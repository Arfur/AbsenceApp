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
        var departments = await _db.StaffDepartments
            .AsNoTracking()
            .OrderBy(d => d.Name)
            .ToListAsync();

        return departments.Select(d => new DepartmentDto
        {
            Id = d.Id,
            Name = d.Name,
            Code = d.Code,
            Status = d.Status,
        });
    }

    public async Task<DepartmentDto?> GetByIdAsync(long id)
    {
        var d = await _db.StaffDepartments
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        return d is null
            ? null
            : new DepartmentDto
            {
                Id = d.Id,
                Name = d.Name,
                Code = d.Code,
                Status = d.Status,
            };
    }
}
