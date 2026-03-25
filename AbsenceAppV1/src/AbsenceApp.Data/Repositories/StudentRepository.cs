/*
===============================================================================
 File        : StudentRepository.cs
 Namespace   : AbsenceApp.Data.Repositories
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-13
 Updated     : 2026-03-13
-------------------------------------------------------------------------------
 Purpose     : In-memory persistence store for student records.
               Provides async-compatible query operations consumed by
               StudentService.
               Pre-seeded with two sample students for development use.
-------------------------------------------------------------------------------
 Description :
   Uses a static in-memory list as the backing store.
   Data does not persist across application restarts.
   Replace with an EF Core or API-backed implementation for production use.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-13  Initial in-memory implementation with seed data.
-------------------------------------------------------------------------------
 Notes       :
   - The backing list is static so seed data is created once per app lifetime
     and survives across navigations.
   - Student data is read-only at runtime; no add/update/delete operations
     are exposed — these would require a separate management interface.
===============================================================================
*/

using AbsenceApp.Data.Context;
using Microsoft.EntityFrameworkCore;
using DataStudent = AbsenceApp.Data.Models.Student;

namespace AbsenceApp.Data.Repositories;

/// <summary>EF Core implementation of IStudentRepository.</summary>
public class StudentRepository : IStudentRepository
{
    private readonly AppDbContext _context;

    public StudentRepository(AppDbContext context) => _context = context;

    public async Task<IEnumerable<DataStudent>> GetAllAsync() =>
        await _context.Students.ToListAsync();

    public async Task<DataStudent?> GetByIdAsync(long id) =>
        await _context.Students.FindAsync(id);
}