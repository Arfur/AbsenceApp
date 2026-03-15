/*
===============================================================================
 File        : AbsenceRepository.cs
 Namespace   : AbsenceApp.Data.Repositories
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-13
 Updated     : 2026-03-13
-------------------------------------------------------------------------------
 Purpose     : In-memory persistence store for absence records.
               Provides async-compatible CRUD operations consumed by
               AbsenceService.
-------------------------------------------------------------------------------
 Description :
   Uses a static in-memory list as the backing store.
   Data does not persist across application restarts.
   Replace with an EF Core or API-backed implementation for production use.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-13  Initial in-memory implementation.
-------------------------------------------------------------------------------
 Notes       :
   - The backing list is static, so all DI-resolved instances share the
     same data within an application lifetime.
   - Task.FromResult wraps synchronous results to satisfy the async contract
     without introducing unnecessary thread pool overhead.
===============================================================================
*/

using AbsenceApp.Core.Models;

namespace AbsenceApp.Data.Repositories;

// =========================================================================
// Absence repository — in-memory List<AbsenceRecord> backing store
// =========================================================================

public class AbsenceRepository : IAbsenceRepository
{
    // =========================================================================
    // Backing store — static list shared across all instances in the DI container
    // =========================================================================

    private static readonly List<AbsenceRecord> _absences = new();

    // =========================================================================
    // Query operations — read-only absence lookups
    // =========================================================================

    public Task<IEnumerable<AbsenceRecord>> GetAbsencesForStudentAsync(string studentId)
    {
        return Task.FromResult(_absences.Where(a => a.StudentId == studentId));
    }

    // =========================================================================
    // Mutation operations — write operations that modify the backing store
    // =========================================================================

    public Task AddAbsenceAsync(AbsenceRecord record)
    {
        _absences.Add(record);
        return Task.CompletedTask;
    }
}