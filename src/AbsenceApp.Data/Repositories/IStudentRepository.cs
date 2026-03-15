/*
===============================================================================
 File        : IStudentRepository.cs
 Namespace   : AbsenceApp.Data.Repositories
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-13
 Updated     : 2026-03-13
-------------------------------------------------------------------------------
 Purpose     : Defines the async query contract for Student domain models.
               Implemented by StudentRepository (in-memory store).
-------------------------------------------------------------------------------
 Description :
   Used by StudentService to decouple the service from the concrete
   StudentRepository class, allowing alternative implementations (e.g.
   an EF Core or REST API-backed repository) to be swapped without
   changing any calling code.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-13  Initial interface extracted from StudentRepository.
-------------------------------------------------------------------------------
 Notes       :
   - Student is an AbsenceApp.Core.Models domain model, not an EF entity;
     this interface may safely be referenced from anywhere in the Data layer.
===============================================================================
*/

using AbsenceApp.Core.Models;

namespace AbsenceApp.Data.Repositories;

public interface IStudentRepository
{
    // =========================================================================
    // Interface methods — async query contract for Student domain model
    // =========================================================================

    Task<IEnumerable<Student>> GetAllStudentsAsync();
    Task<Student?> GetStudentByIdAsync(string id);
}
