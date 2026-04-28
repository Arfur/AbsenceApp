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

    public static ClassDto ToDto(TeachingGroup entity) => new()
    {
        Id          = entity.Id,
        Name        = entity.Name,
        Code        = entity.Code,
        Description = entity.Description,
    };

    // ===============================================================================
    // DTO to entity
    // ===============================================================================

    public static TeachingGroup ToEntity(ClassDto dto) => new()
    {
        Id          = dto.Id,
        Name        = dto.Name,
        Code        = dto.Code,
        Description = dto.Description,
    };
}
