/*
===============================================================================
 File        : IUserService.cs
 Namespace   : AbsenceApp.Core.Interfaces
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-14
 Updated     : 2026-03-14
-------------------------------------------------------------------------------
 Purpose     : Defines the async read contract for user (staff) operations.
               Implemented by UserService in AbsenceApp.Data.Services.
               Returns UserDto to keep the Core layer free of EF entities and
               to prevent credential fields from leaking past the Data layer.
-------------------------------------------------------------------------------
 Description :
   Users represent staff members in the application. The interface exposes
   only read operations required by the UI. Administrative write operations
   are handled by the repository directly in the Data layer.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-14  Initial interface defined.
-------------------------------------------------------------------------------
 Notes       :
   - UserDto intentionally excludes PasswordHash; see UserMapper for details.
   - GetByIdAsync returns null when the user does not exist.
===============================================================================
*/

using AbsenceApp.Core.DTOs;

namespace AbsenceApp.Core.Interfaces;

public interface IUserService
{
    // =========================================================================
    // Interface methods — async read operations for User (staff) data
    // =========================================================================

    /// <summary>Returns all users (staff members).</summary>
    Task<IEnumerable<UserDto>> GetAllAsync();

    /// <summary>Returns a single user by their primary key.
    /// Returns null if the user does not exist.</summary>
    Task<UserDto?> GetByIdAsync(int id);
}
