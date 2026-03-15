/*
===============================================================================
 File        : IStudentService.cs
 Namespace   : AbsenceApp.Core.Interfaces
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-13
 Updated     : 2026-03-13
-------------------------------------------------------------------------------
 Purpose     : Defines the contract for student-record operations.
               Implemented by StudentService (core) and any future
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
// Student service contract — defines query operations for student records
// =========================================================================

public interface IStudentService
{
    Task<IEnumerable<Student>> GetAllStudentsAsync();
    Task<Student?> GetStudentByIdAsync(string id);
}