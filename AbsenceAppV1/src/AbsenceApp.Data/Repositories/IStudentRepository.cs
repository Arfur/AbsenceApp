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

using DataStudent = AbsenceApp.Data.Models.Student;

namespace AbsenceApp.Data.Repositories;

/// <summary>EF Core repository contract for the students table.</summary>
public interface IStudentRepository
{
    Task<IEnumerable<DataStudent>> GetAllAsync();
    Task<DataStudent?> GetByIdAsync(long id);
}
