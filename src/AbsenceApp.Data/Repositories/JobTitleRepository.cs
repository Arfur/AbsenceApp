using AbsenceApp.Data.Context;
using AbsenceApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AbsenceApp.Data.Repositories;

public interface IJobTitleRepository
{
    Task<IEnumerable<JobTitle>> GetAllAsync();
    Task<JobTitle?>             GetByIdAsync(long id);
}

public class JobTitleRepository : IJobTitleRepository
{
    private readonly AppDbContext _context;
    public JobTitleRepository(AppDbContext context) => _context = context;

    public async Task<IEnumerable<JobTitle>> GetAllAsync() =>
        await _context.JobTitles.ToListAsync();

    public async Task<JobTitle?> GetByIdAsync(long id) =>
        await _context.JobTitles.FindAsync(id);
}
