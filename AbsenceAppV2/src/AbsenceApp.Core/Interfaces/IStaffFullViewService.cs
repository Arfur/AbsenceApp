/*
===============================================================================
 File        : IStaffFullViewService.cs
 Namespace   : AbsenceApp.Core.Interfaces
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-17
 Updated     : 2026-03-17
-------------------------------------------------------------------------------
 Purpose     : Read-only contract for fetching flattened staff projections
               with FK IDs replaced by human-readable names.
               Implemented by StaffFullViewService in AbsenceApp.Data.Services.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-17  Initial creation.
-------------------------------------------------------------------------------
 Notes       :
   - Returns StaffFullViewDto, not StaffDto — different surface.
   - Registered for DI in MauiProgram alongside IStaffService.
===============================================================================
*/

using AbsenceApp.Core.DTOs;

namespace AbsenceApp.Core.Interfaces;

// =========================================================================
// IStaffFullViewService — read-only contract for full-view staff data
// =========================================================================

public interface IStaffFullViewService
{
    /// <summary>Returns all staff as flattened full-view projections.</summary>
    Task<IReadOnlyList<StaffFullViewDto>> GetAllAsync();

    /// <summary>Permanently deletes a staff record by ID.</summary>
    Task DeleteAsync(long id, CancellationToken ct = default);
}
