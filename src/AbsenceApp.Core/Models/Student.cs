/*
===============================================================================
 File        : Student.cs
 Namespace   : AbsenceApp.Core.Models
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-13
 Updated     : 2026-03-13
-------------------------------------------------------------------------------
 Purpose     : Domain model representing a student enrolled in the school.
-------------------------------------------------------------------------------
 Description :
   Properties:
     Id          — unique identifier.
     FirstName   — given name.
     LastName    — family name.
     YearGroup   — school year group (e.g. "Year 7").
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-13  Initial model created.
-------------------------------------------------------------------------------
 Notes       :
   - Id is a plain string; the in-memory seed data uses simple numeric
     strings ("1", "2", …) for readability.
   - YearGroup is free text — no enumeration enforced at the model level.
===============================================================================
*/

namespace AbsenceApp.Core.Models;

// =========================================================================
// Domain model — student; one row per enrolled student
// =========================================================================

public class Student
{
    public string Id { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string YearGroup { get; set; } = string.Empty;
}