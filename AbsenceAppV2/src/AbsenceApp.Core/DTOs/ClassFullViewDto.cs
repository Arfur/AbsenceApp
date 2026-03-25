/*
===============================================================================
 File        : ClassFullViewDto.cs
 Namespace   : AbsenceApp.Core.DTOs
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-17
 Updated     : 2026-03-17
-------------------------------------------------------------------------------
 Purpose     : Flattened read-only projection of a Class record for Table
               Settings display. Class has no FK IDs to resolve; this DTO
               simply omits system fields (Id, CreatedAt, UpdatedAt) and adds
               Code which was omitted from ClassDto.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-17  Initial creation.
-------------------------------------------------------------------------------
 Notes       :
   - Not an EF Core entity — never tracked or persisted.
   - Code is included here (ClassDto omits it) to expose the full display surface.
===============================================================================
*/

namespace AbsenceApp.Core.DTOs;

// =========================================================================
// ClassFullViewDto — flattened class projection for Table Settings display
// =========================================================================

public class ClassFullViewDto
{
    public string  Name        { get; set; } = string.Empty;
    public string? Code        { get; set; }
    public string? Description { get; set; }
}
