using AbsenceApp.Core.DTOs;
using AbsenceApp.Core.Interfaces;
using AbsenceApp.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace AbsenceApp.Data.Services;

public class JobTitleService : IJobTitleService
{
    private readonly AppDbContext _db;

    public JobTitleService(AppDbContext db) => _db = db;

    public async Task<IEnumerable<JobTitleDto>> GetAllAsync()
    {
        var ids = await _db.Staff
            .AsNoTracking()
            .Where(s => s.JobTitleId > 0)
            .Select(s => s.JobTitleId)
            .Distinct()
            .OrderBy(id => id)
            .ToListAsync();

        return ids.Select(id => new JobTitleDto
        {
            Id = id,
            Title = $"Job Title {id}",
            Code = $"JT{id}",
        });
    }

    public async Task<JobTitleDto?> GetByIdAsync(long id)
    {
        var exists = await _db.Staff
            .AsNoTracking()
            .AnyAsync(s => s.JobTitleId == id);

        return exists
            ? new JobTitleDto { Id = (int)id, Title = $"Job Title {id}", Code = $"JT{id}" }
            : null;
    }
}
