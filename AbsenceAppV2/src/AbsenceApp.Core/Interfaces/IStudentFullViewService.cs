/*
===============================================================================
 File        : IStudentFullViewService.cs
 Namespace   : AbsenceApp.Core.Interfaces
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-17
 Updated     : 2026-03-17
-------------------------------------------------------------------------------
 Purpose     : Read-only contract for fetching flattened student projections
               with FK IDs replaced by human-readable names.
               Implemented by StudentFullViewService in AbsenceApp.Data.Services.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-17  Initial creation.
-------------------------------------------------------------------------------
 Notes       :
   - Returns StudentFullViewDto, not StudentDto — different surface.
   - Registered for DI in MauiProgram alongside IStudentService.
===============================================================================
*/

using AbsenceApp.Core.DTOs;

namespace AbsenceApp.Core.Interfaces;

// =========================================================================
// IStudentFullViewService — read-only contract for full-view student data
// =========================================================================

public interface IStudentFullViewService
{
    /// <summary>Returns all students as flattened full-view projections.</summary>
    Task<IReadOnlyList<StudentFullViewDto>> GetAllAsync();

    /// <summary>Returns a single student as a flattened full-view projection by ID.</summary>
    Task<StudentFullViewDto?> GetByIdAsync(int id, CancellationToken ct = default);
}
