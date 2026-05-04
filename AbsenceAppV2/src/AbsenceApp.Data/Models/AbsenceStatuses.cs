/*
===============================================================================
 File        : AbsenceStatus.cs
 Namespace   : AbsenceApp.Data.Models
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-04-21
 Updated     : 2026-04-21
-------------------------------------------------------------------------------
 Purpose     : EF Core entity for the AbsenceStatuses lookup table.
===============================================================================
*/
namespace AbsenceApp.Data.Models;

public class AbsenceStatus
{
    public long     Id        { get; set; }
    public string   Code      { get; set; } = string.Empty;
    public string   Name      { get; set; } = string.Empty;
    public bool     IsFinal   { get; set; } = false;
    public DateTime CreatedAt { get; set; }
}
