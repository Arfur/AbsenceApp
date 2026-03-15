/*
===============================================================================
 File        : AbsenceService.cs
 Namespace   : AbsenceApp.Data.Services
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-13
 Updated     : 2026-03-13
-------------------------------------------------------------------------------
 Purpose     : Implementation of IAbsenceService.
               Delegates persistence to AbsenceRepository.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-13  Initial implementation.
   - 1.1.0  2026-03-13  Moved from AbsenceApp.Core.Services to
                        AbsenceApp.Data.Services to remove the Core→Data
                        circular dependency.
-------------------------------------------------------------------------------
 Notes       :
   - AbsenceRepository is injected; swap it for an EF Core repository
     without changing any calling code.
   - Registered against IAbsenceService in MauiProgram for DI resolution.
===============================================================================
*/

using AbsenceApp.Core.Interfaces;
using AbsenceApp.Core.Models;
using AbsenceApp.Data.Repositories;

namespace AbsenceApp.Data.Services;

// ============================================================================
// Absence service — IAbsenceService implementation backed by AbsenceRepository
// ============================================================================
public class AbsenceService : IAbsenceService
{
    // =========================================================================
    // Dependencies — repository injected via constructor
    // =========================================================================

    private readonly IAbsenceRepository _repository;

    public AbsenceService(IAbsenceRepository repository)
    {
        _repository = repository;
    }

    // =========================================================================
    // IAbsenceService implementation — delegates each operation to the repository
    // =========================================================================

    public Task<IEnumerable<AbsenceRecord>> GetAbsencesForStudentAsync(string studentId)
    {
        return _repository.GetAbsencesForStudentAsync(studentId);
    }

    public Task AddAbsenceAsync(AbsenceRecord record)
    {
        return _repository.AddAbsenceAsync(record);
    }
}
