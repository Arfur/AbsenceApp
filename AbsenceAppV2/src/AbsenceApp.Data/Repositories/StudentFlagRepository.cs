/*
===============================================================================
 File        : StudentFlagRepository.cs
 Namespace   : AbsenceApp.Data.Repositories
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-05-05
 Updated     : 2026-05-05
-------------------------------------------------------------------------------
 Purpose     : EF Core repository for StudentFlag records.
               Provides read-only access to student flag data (SEN, FSM, etc.).
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-05-05  Initial creation.
===============================================================================
*/
using AbsenceApp.Data.Context;
using AbsenceApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AbsenceApp.Data.Repositories;

public interface IStudentFlagRepository
{
    Task<IEnumerable<StudentFlag>> GetByStudentAsync(int studentId, CancellationToken ct = default);
    Task<StudentFlag?>             GetByIdAsync(int id, CancellationToken ct = default);
}

public class StudentFlagRepository : IStudentFlagRepository
{
    private readonly AppDbContext _context;
    public StudentFlagRepository(AppDbContext context) => _context = context;

    public async Task<IEnumerable<StudentFlag>> GetByStudentAsync(int studentId, CancellationToken ct = default) =>
        await _context.StudentFlags
            .AsNoTracking()
            .Where(f => f.StudentId == studentId)
            .OrderByDescending(f => f.AssignedAt)
            .ToListAsync(ct);

    public async Task<StudentFlag?> GetByIdAsync(int id, CancellationToken ct = default) =>
        await _context.StudentFlags
            .AsNoTracking()
            .FirstOrDefaultAsync(f => f.Id == id, ct);
}
