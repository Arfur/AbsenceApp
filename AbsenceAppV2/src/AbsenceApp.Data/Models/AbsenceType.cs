/*
===============================================================================
 File        : AbsenceType.cs
 Namespace   : AbsenceApp.Data.Models
 Author      : Michael
 Version     : 2.0.0
 Created     : 2026-03-15
 Updated     : 2026-04-21
-------------------------------------------------------------------------------
 Purpose     : EF Core entity for the AbsenceTypes lookup table.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-15  Initial creation.
   - 2.0.0  2026-04-21  Absence domain redesign: replaced Description/IsPaid
                        with Category/IsAuthorised.
===============================================================================
*/
namespace AbsenceApp.Data.Models;

public class AbsenceType
{
    public long     Id           { get; set; }
    public string   Code         { get; set; } = string.Empty;
    public string   Name         { get; set; } = string.Empty;
    public string   Category     { get; set; } = string.Empty;
    public bool     IsAuthorised { get; set; } = true;
    public DateTime CreatedAt    { get; set; }
}
