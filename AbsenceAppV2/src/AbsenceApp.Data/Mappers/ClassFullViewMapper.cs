/*
===============================================================================
 File        : ClassFullViewMapper.cs
 Namespace   : AbsenceApp.Data.Mappers
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-17
 Updated     : 2026-03-17
-------------------------------------------------------------------------------
 Purpose     : Maps a Class entity to a ClassFullViewDto. Class has no FK IDs
               to resolve; this mapper simply omits system fields (Id, CreatedAt,
               UpdatedAt) and exposes Name, Code, and Description.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-17  Initial creation.
-------------------------------------------------------------------------------
 Notes       :
   - One-way mapping only (read projection — no ToEntity required).
   - Code is included here even though ClassDto omits it.
===============================================================================
*/

using AbsenceApp.Core.DTOs;
using AbsenceApp.Data.Models;

namespace AbsenceApp.Data.Mappers;

// =========================================================================
// ClassFullViewMapper — Class entity → ClassFullViewDto
// =========================================================================

public static class ClassFullViewMapper
{
    /// <summary>
    /// Projects a Class entity to a ClassFullViewDto, omitting system fields.
    /// </summary>
    public static ClassFullViewDto ToDto(TeachingGroup entity) => new()
    {
        Name        = entity.Name,
        Code        = entity.Code,
        Description = entity.Description,
    };
}
