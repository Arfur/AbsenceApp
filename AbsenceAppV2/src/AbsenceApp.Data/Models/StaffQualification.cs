/*
===============================================================================
 File        : StaffQualification.cs
 Namespace   : AbsenceApp.Data.Models
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-04-28
 Updated     : 2026-04-28
-------------------------------------------------------------------------------
 Purpose     : EF Core entity for the `staffqualifications` table.
               Minimal schema representation to satisfy AppDbContext and allow
               EF Core to map the table without errors.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-04-28  Initial creation to resolve CS0246 and align with
                         AppDbContext DbSet + OnModelCreating mapping.
-------------------------------------------------------------------------------
 Notes       :
   - EF Core will ignore any extra columns in the database that are not defined
     here. This model can be expanded later once the full schema is confirmed.
===============================================================================
*/

namespace AbsenceApp.Data.Models;

public sealed class StaffQualification
{
    public int Id { get; set; }

    // Optional: FK to Staff table
    public int StaffId { get; set; }

    // Optional: qualification name (e.g., "QTS", "First Aid", etc.)
    public string? QualificationName { get; set; }

    // Optional: date awarded
    public DateTime? AwardedAt { get; set; }
}
