/*
===============================================================================
 File        : IClassFullViewService.cs
 Namespace   : AbsenceApp.Core.Interfaces
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-17
 Updated     : 2026-03-17
-------------------------------------------------------------------------------
 Purpose     : Read-only contract for fetching flattened class projections
               with system fields (Id, CreatedAt, UpdatedAt) excluded.
               Implemented by ClassFullViewService in AbsenceApp.Data.Services.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-17  Initial creation.
-------------------------------------------------------------------------------
 Notes       :
   - Returns ClassFullViewDto which also exposes Code (omitted from ClassDto).
   - Registered for DI in MauiProgram alongside IClassService.
===============================================================================
*/

using AbsenceApp.Core.DTOs;

namespace AbsenceApp.Core.Interfaces;

// =========================================================================
// IClassFullViewService — read-only contract for full-view class data
// =========================================================================

public interface IClassFullViewService
{
    /// <summary>Returns all classes as flattened full-view projections.</summary>
    Task<IReadOnlyList<ClassFullViewDto>> GetAllAsync();
}
