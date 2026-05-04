/*
===============================================================================
 File        : SubjectService.cs
 Namespace   : AbsenceApp.Data.Services
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-17
 Updated     : 2026-03-17
-------------------------------------------------------------------------------
 Purpose     : EF Core implementation of ISubjectService.
               Subjects are stored in the classes table; this service provides
               a clean domain boundary until a dedicated subjects table exists.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-17  Initial creation.
===============================================================================
*/

using AbsenceApp.Core.DTOs;
using AbsenceApp.Core.Interfaces;
using AbsenceApp.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace AbsenceApp.Data.Services;

public class SubjectService : ISubjectService
{
  private readonly AppDbContext _db;

  public SubjectService(AppDbContext db) => _db = db;

  public async Task<IEnumerable<SubjectDto>> GetAllAsync()
  {
    return await _db.YearGroups
      .AsNoTracking()
      .OrderBy(y => y.Id)
      .Select(y => new SubjectDto
      {
        Id = y.Id,
        Name = y.Name,
        Description = y.Description,
      })
      .ToListAsync();
  }

  public async Task<SubjectDto?> GetByIdAsync(int id)
  {
    return await _db.YearGroups
      .AsNoTracking()
      .Where(y => y.Id == id)
      .Select(y => new SubjectDto
      {
        Id = y.Id,
        Name = y.Name,
        Description = y.Description,
      })
      .FirstOrDefaultAsync();
  }

  public Task<SubjectDto> CreateAsync(SubjectDto dto)
    => throw new NotSupportedException("Create is not supported: dedicated subject entity is not present in this schema.");

  public Task UpdateAsync(SubjectDto dto)
    => throw new NotSupportedException("Update is not supported: dedicated subject entity is not present in this schema.");

  public Task DeleteAsync(int id)
    => throw new NotSupportedException("Delete is not supported: dedicated subject entity is not present in this schema.");
}
