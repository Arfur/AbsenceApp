/*
===============================================================================
 File        : StudentMedicalRepository.cs
 Namespace   : AbsenceApp.Data.Repositories
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-05-05
 Updated     : 2026-05-05
-------------------------------------------------------------------------------
 Purpose     : EF Core repository for StudentMedical records.
               Provides read-only access to student medical data.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-05-05  Initial creation.
===============================================================================
*/
using AbsenceApp.Data.Context;
using AbsenceApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AbsenceApp.Data.Repositories;

public interface IStudentMedicalRepository
{
    Task<IEnumerable<StudentMedical>> GetByStudentAsync(int studentId, CancellationToken ct = default);
    Task<StudentMedical?>             GetByIdAsync(int id, CancellationToken ct = default);
}

public class StudentMedicalRepository : IStudentMedicalRepository
{
    private readonly AppDbContext _context;
    public StudentMedicalRepository(AppDbContext context) => _context = context;

    public async Task<IEnumerable<StudentMedical>> GetByStudentAsync(int studentId, CancellationToken ct = default) =>
        await _context.StudentMedical
            .AsNoTracking()
            .Where(m => m.StudentId == studentId)
            .OrderByDescending(m => m.CreatedAt)
            .ToListAsync(ct);

    public async Task<StudentMedical?> GetByIdAsync(int id, CancellationToken ct = default) =>
        await _context.StudentMedical
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == id, ct);
}
