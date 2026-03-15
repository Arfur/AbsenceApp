/*
===============================================================================
 File        : RoleDto.cs
 Namespace   : AbsenceApp.Core.DTOs
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-13
 Updated     : 2026-03-13
-------------------------------------------------------------------------------
 Purpose     : Data transfer object for passing role data across layer
               boundaries without exposing the EF Core Role entity directly.
-------------------------------------------------------------------------------
 Description :
   Carries the role identifier and name for a system permission grouping.
   Used as the return type from role-related repository and service calls.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-13  Initial creation.
-------------------------------------------------------------------------------
 Notes       :
   - RoleName defaults to string.Empty to satisfy nullable analysis.
===============================================================================
*/

namespace AbsenceApp.Core.DTOs;

public class RoleDto
{
    // =========================================================================
    // Properties — data fields transferred across layer boundaries
    // =========================================================================

    public int RoleId { get; set; }
    public string RoleName { get; set; } = string.Empty;
}
