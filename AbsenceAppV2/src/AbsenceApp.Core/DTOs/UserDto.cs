/*
===============================================================================
 File        : UserDto.cs
 Namespace   : AbsenceApp.Core.DTOs
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-13
 Updated     : 2026-03-13
-------------------------------------------------------------------------------
 Purpose     : Data transfer object for passing user data across layer
               boundaries without exposing the EF Core User entity directly.
-------------------------------------------------------------------------------
 Description :
   Carries identity and contact information for a system user.
   Used as the return type from user-related repository and service calls.
   FullName is a computed convenience property; it is not stored in the
   database and is derived at runtime from FirstName and LastName.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-13  Initial creation.
-------------------------------------------------------------------------------
 Notes       :
   - All string properties default to string.Empty to satisfy nullable
     reference-type analysis without requiring a parameterised constructor.
===============================================================================
*/

namespace AbsenceApp.Core.DTOs;

public class UserDto
{
    public int      UserId      { get; set; }
    public string   Username    { get; set; } = string.Empty;
    public string   Email       { get; set; } = string.Empty;
    public string   Status      { get; set; } = string.Empty;
    public bool     IsAdmin     { get; set; }
    public int?     StaffId     { get; set; }
    public string?  Timezone    { get; set; }
    public string?  LanguageCode { get; set; }
    public DateTime? LastLoginAt { get; set; }
    public int      LoginCount  { get; set; }
    public bool     IsActive    => Status == "Active";
    public DateTime CreatedAt   { get; set; }
    public DateTime UpdatedAt   { get; set; }
}
