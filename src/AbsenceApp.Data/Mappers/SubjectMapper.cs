/*
===============================================================================
 File        : SubjectMapper.cs
 Namespace   : AbsenceApp.Data.Mappers
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-17
 Updated     : 2026-03-17
-------------------------------------------------------------------------------
 Purpose     : Maps between the Class EF entity and SubjectDto.
               Subjects are stored in the classes table; this mapper provides
               a clean domain boundary until a dedicated subjects table exists.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-17  Initial creation.
===============================================================================
*/

using AbsenceApp.Core.DTOs;
using AbsenceApp.Data.Models;

namespace AbsenceApp.Data.Mappers;

public static class SubjectMapper
{
    public static SubjectDto ToDto(Class entity) => new()
    {
        Id          = (int)entity.Id,
        Name        = entity.Name,
        Description = entity.Description,
    };

    public static Class ToEntity(SubjectDto dto) => new()
    {
        Id          = dto.Id,
        Name        = dto.Name,
        Description = dto.Description,
    };
}
