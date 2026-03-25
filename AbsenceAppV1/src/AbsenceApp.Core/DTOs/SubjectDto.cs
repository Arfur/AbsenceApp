/*
===============================================================================
 File        : SubjectDto.cs
 Namespace   : AbsenceApp.Core.DTOs
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-17
 Updated     : 2026-03-17
-------------------------------------------------------------------------------
 Purpose     : Data transfer object for passing subject data across layer
               boundaries without exposing the EF Core entity directly.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-17  Initial creation.
===============================================================================
*/

namespace AbsenceApp.Core.DTOs;

public class SubjectDto
{
    public int     Id          { get; set; }
    public string  Name        { get; set; } = string.Empty;
    public string? Description { get; set; }
}
