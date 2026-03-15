/*
===============================================================================
 File        : ClassDto.cs
 Namespace   : AbsenceApp.Core.DTOs
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-13
 Updated     : 2026-03-13
-------------------------------------------------------------------------------
 Purpose     : Data transfer object for passing class data across layer
               boundaries without exposing the EF Core Class entity directly.
-------------------------------------------------------------------------------
 Description :
   Carries the class identifier, name, and optional description for a
   teaching group or cohort. Used as the return type from class-related
   repository and service calls.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-13  Initial creation.
-------------------------------------------------------------------------------
 Notes       :
   - Description is nullable; not all classes require a description.
   - ClassName defaults to string.Empty to satisfy nullable analysis.
===============================================================================
*/

namespace AbsenceApp.Core.DTOs;

public class ClassDto
{
    // =========================================================================
    // Properties — data fields transferred across layer boundaries
    // =========================================================================

    public int ClassId { get; set; }
    public string ClassName { get; set; } = string.Empty;
    public string? Description { get; set; }
}
