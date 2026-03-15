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

using AbsenceApp.Core.Models;

namespace AbsenceApp.Data.Repositories;

// =========================================================================
// Student repository — in-memory List<Student> with two pre-seeded rows
// =========================================================================

public class StudentRepository : IStudentRepository
{
    // =========================================================================
    // Backing store — static pre-seeded list initialised on first use
    // =========================================================================

    private static readonly List<Student> _students = new()
    {
        new Student { Id = "1", FirstName = "Alice", LastName = "Smith", YearGroup = "Year 7" },
        new Student { Id = "2", FirstName = "Bob", LastName = "Jones", YearGroup = "Year 8" }
    };

    // =========================================================================
    // Query operations — read-only student lookups; no mutations exposed
    // =========================================================================

    public Task<IEnumerable<Student>> GetAllStudentsAsync()
    {
        return Task.FromResult<IEnumerable<Student>>(_students);
    }

    public Task<Student?> GetStudentByIdAsync(string id)
    {
        return Task.FromResult(_students.FirstOrDefault(s => s.Id == id));
    }
}