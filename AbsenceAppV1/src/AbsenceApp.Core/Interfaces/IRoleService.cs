/*
===============================================================================
 File        : IRoleService.cs
 Namespace   : AbsenceApp.Core.Interfaces
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-13
 Updated     : 2026-03-13
-------------------------------------------------------------------------------
 Purpose     : Defines the async CRUD contract for role operations.
               Implemented by RoleService in AbsenceApp.Data.Services.
-------------------------------------------------------------------------------
 Description :
   All methods return DTOs (RoleDto) rather than EF Core entities, ensuring
   the Core layer has no dependency on the Data or EF Core packages.
   Registered against this interface in DataServiceRegistration for DI.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-13  Initial interface defined.
-------------------------------------------------------------------------------
 Notes       :
   - GetByIdAsync returns null if the requested role does not exist.
   - CreateAsync returns the persisted DTO including the DB-generated RoleId.
===============================================================================
*/

using AbsenceApp.Core.DTOs;

namespace AbsenceApp.Core.Interfaces;

public interface IRoleService
{
    // =========================================================================
    // Interface methods — async CRUD contract for Role entities
    // =========================================================================

    Task<IEnumerable<RoleDto>> GetAllAsync();
    Task<RoleDto?> GetByIdAsync(int id);
    Task<RoleDto> CreateAsync(RoleDto dto);
    Task UpdateAsync(RoleDto dto);
    Task DeleteAsync(int id);
}
