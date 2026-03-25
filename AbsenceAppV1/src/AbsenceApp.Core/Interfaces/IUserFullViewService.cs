/*
===============================================================================
 File        : IUserFullViewService.cs
 Namespace   : AbsenceApp.Core.Interfaces
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-17
 Updated     : 2026-03-17
-------------------------------------------------------------------------------
 Purpose     : Read-only contract for fetching flattened user projections
               with FK IDs replaced by human-readable names and all sensitive
               credential fields excluded.
               Implemented by UserFullViewService in AbsenceApp.Data.Services.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-17  Initial creation.
-------------------------------------------------------------------------------
 Notes       :
   - Returns UserFullViewDto, not UserDto — broader surface, no credentials.
   - Registered for DI in MauiProgram alongside IUserService.
===============================================================================
*/

using AbsenceApp.Core.DTOs;

namespace AbsenceApp.Core.Interfaces;

// =========================================================================
// IUserFullViewService — read-only contract for full-view user data
// =========================================================================

public interface IUserFullViewService
{
    /// <summary>Returns all users as flattened full-view projections.</summary>
    Task<IReadOnlyList<UserFullViewDto>> GetAllAsync();
}
