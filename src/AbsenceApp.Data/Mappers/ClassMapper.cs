/*
===============================================================================
 File        : ClassMapper.cs
 Namespace   : AbsenceApp.Data.Mappers
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-15
 Updated     : 2026-03-15
-------------------------------------------------------------------------------
 Purpose     : Maps between the Class EF entity (TABLE9 schema) and ClassDto.
               ClassDto.ClassId / ClassDto.ClassName map to Class.Id / Class.Name.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-15  Initial creation -- updated for TABLE9 entity shape.
-------------------------------------------------------------------------------
 Notes       :
   - ClassId cast from long to int; current data fits within int range.
   - Align ClassDto to long once the client layer is updated.
===============================================================================
*/

using AbsenceApp.Core.DTOs;
using AbsenceApp.Data.Models;

namespace AbsenceApp.Data.Mappers;

// ===============================================================================
// ClassMapper -- Class entity <-> ClassDto bidirectional mapping
// ===============================================================================

public static class ClassMapper
{
    // ===============================================================================
    // Entity to DTO
    // ===============================================================================

    public static ClassDto ToDto(Class entity) => new()
    {
        ClassId     = (int)entity.Id,
        ClassName   = entity.Name,
        Description = entity.Description,
    };

    // ===============================================================================
    // DTO to entity
    // ===============================================================================

    public static Class ToEntity(ClassDto dto) => new()
    {
        Id          = dto.ClassId,
        Name        = dto.ClassName,
        Description = dto.Description,
    };
}
