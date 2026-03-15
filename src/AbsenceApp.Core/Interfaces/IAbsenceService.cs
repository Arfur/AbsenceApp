/*
===============================================================================
 File        : IAbsenceService.cs
 Namespace   : AbsenceApp.Core.Interfaces
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-13
 Updated     : 2026-03-13
-------------------------------------------------------------------------------
 Purpose     : Defines the contract for absence-record operations.
               Implemented by AbsenceService (core) and any future
               alternative implementations (e.g. EF Core, REST API).
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-13  Initial interface defined.
-------------------------------------------------------------------------------
 Notes       :
   - All methods return Tasks to keep the contract async-friendly even
     for the in-memory implementation.
   - Registered against this interface in MauiProgram for DI resolution.
===============================================================================
*/

using AbsenceApp.Core.Models;

namespace AbsenceApp.Core.Interfaces;

// =========================================================================
// Absence service contract — defines CRUD operations for absence records
// =========================================================================

public interface IAbsenceService
{
    Task<IEnumerable<AbsenceRecord>> GetAbsencesForStudentAsync(string studentId);
    Task AddAbsenceAsync(AbsenceRecord record);
}