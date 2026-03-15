/*
===============================================================================
 File        : IAbsenceRepository.cs
 Namespace   : AbsenceApp.Data.Repositories
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-13
 Updated     : 2026-03-13
-------------------------------------------------------------------------------
 Purpose     : Defines the async persistence contract for AbsenceRecord domain
               models. Implemented by AbsenceRepository (in-memory store).
-------------------------------------------------------------------------------
 Description :
   Used by AbsenceService to decouple the service from the concrete
   AbsenceRepository class, allowing alternative implementations (e.g.
   an EF Core or REST API-backed repository) to be swapped without
   changing any calling code.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-13  Initial interface extracted from AbsenceRepository.
-------------------------------------------------------------------------------
 Notes       :
   - AbsenceRecord is an AbsenceApp.Core.Models domain model, not an EF entity;
     this interface may safely be referenced from anywhere in the Data layer.
===============================================================================
*/

using AbsenceApp.Core.Models;

namespace AbsenceApp.Data.Repositories;

public interface IAbsenceRepository
{
    // =========================================================================
    // Interface methods — async persistence contract for AbsenceRecord
    // =========================================================================

    Task<IEnumerable<AbsenceRecord>> GetAbsencesForStudentAsync(string studentId);
    Task AddAbsenceAsync(AbsenceRecord record);
}
