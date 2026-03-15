/*
===============================================================================
 File        : StudentService.cs
 Namespace   : AbsenceApp.Data.Services
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-13
 Updated     : 2026-03-13
-------------------------------------------------------------------------------
 Purpose     : Implementation of IStudentService.
               Delegates persistence to StudentRepository.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-13  Initial implementation.
   - 1.1.0  2026-03-13  Moved from AbsenceApp.Core.Services to
                        AbsenceApp.Data.Services to remove the Core→Data
                        circular dependency.
-------------------------------------------------------------------------------
 Notes       :
   - StudentRepository is injected; swap it for an EF Core repository
     without changing any calling code.
   - Registered against IStudentService in MauiProgram for DI resolution.
===============================================================================
*/

using AbsenceApp.Core.Interfaces;
using AbsenceApp.Core.Models;
using AbsenceApp.Data.Repositories;

namespace AbsenceApp.Data.Services;

// ============================================================================
// Student service — IStudentService implementation backed by StudentRepository
// ============================================================================
public class StudentService : IStudentService
{
    // =========================================================================
    // Dependencies — repository injected via constructor
    // =========================================================================

    private readonly IStudentRepository _repository;

    public StudentService(IStudentRepository repository)
    {
        _repository = repository;
    }

    // =========================================================================
    // IStudentService implementation — delegates each operation to the repository
    // =========================================================================

    public Task<IEnumerable<Student>> GetAllStudentsAsync()
    {
        return _repository.GetAllStudentsAsync();
    }

    public Task<Student?> GetStudentByIdAsync(string id)
    {
        return _repository.GetStudentByIdAsync(id);
    }
}
