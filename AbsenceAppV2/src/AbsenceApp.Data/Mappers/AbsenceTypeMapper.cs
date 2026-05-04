/*
===============================================================================
 File        : AbsenceTypeMapper.cs
 Namespace   : AbsenceApp.Data.Mappers
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-05-04
 Updated     : 2026-05-04
-------------------------------------------------------------------------------
 Purpose     : Static mapper between AbsenceType model and AbsenceTypeDto.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-05-04  Initial header added (Phase 3.9). Removed erroneous
                         (ulong) cast in ToEntity: dto.Id is long, model
                         Id is long — no cast required.
===============================================================================
*/
using AbsenceApp.Core.DTOs;
using AbsenceApp.Data.Models;

namespace AbsenceApp.Data.Mappers;

public static class AbsenceTypeMapper
{
    public static AbsenceTypeDto ToDto(AbsenceType e) => new()
    {
        Id           = (long)e.Id,
        Code         = e.Code,
        Name         = e.Name,
        Category     = e.Category,
        IsAuthorised = e.IsAuthorised,
        CreatedAt    = e.CreatedAt
    };

    public static AbsenceType ToEntity(AbsenceTypeDto dto) => new()
    {
        Id           = dto.Id,
        Code         = dto.Code,
        Name         = dto.Name,
        Category     = dto.Category,
        IsAuthorised = dto.IsAuthorised,
        CreatedAt    = dto.CreatedAt
    };
}
