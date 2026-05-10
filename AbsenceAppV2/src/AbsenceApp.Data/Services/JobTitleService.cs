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
        var titles = await _db.JobTitles
            .AsNoTracking()
            .OrderBy(t => t.Title)
            .ToListAsync();

        return titles.Select(t => new JobTitleDto
        {
            Id = t.Id,
            Title = t.Title,
            Code = t.Code,
        });
    }

    public async Task<JobTitleDto?> GetByIdAsync(long id)
    {
        var t = await _db.JobTitles
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        return t is null
            ? null
            : new JobTitleDto
            {
                Id = t.Id,
                Title = t.Title,
                Code = t.Code,
            };
    }
}
