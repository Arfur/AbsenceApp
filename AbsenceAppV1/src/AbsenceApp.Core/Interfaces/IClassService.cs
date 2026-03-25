/*
===============================================================================
 File        : IClassService.cs
 Namespace   : AbsenceApp.Core.Interfaces
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-13
 Updated     : 2026-03-13
-------------------------------------------------------------------------------
 Purpose     : Defines the async CRUD contract for class and cohort operations.
               Implemented by ClassService in AbsenceApp.Data.Services.
-------------------------------------------------------------------------------
 Description :
   All methods return DTOs (ClassDto) rather than EF Core entities, ensuring
   the Core layer has no dependency on the Data or EF Core packages.
   Registered against this interface in DataServiceRegistration for DI.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-13  Initial interface defined.
-------------------------------------------------------------------------------
 Notes       :
   - GetByIdAsync returns null if the requested class does not exist.
   - CreateAsync returns the persisted DTO including the DB-generated ClassId.
===============================================================================
*/

using AbsenceApp.Core.DTOs;

namespace AbsenceApp.Core.Interfaces;

public interface IClassService
{
    // =========================================================================
    // Interface methods — async CRUD contract for Class entities
    // =========================================================================

    Task<IEnumerable<ClassDto>> GetAllAsync();
    Task<ClassDto?> GetByIdAsync(int id);
    Task<ClassDto> CreateAsync(ClassDto dto);
    Task UpdateAsync(ClassDto dto);
    Task DeleteAsync(int id);
}
