/*
===============================================================================
 File        : GlobalConfig.cs
 Namespace   : AbsenceApp.Data.Models
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-04-28
 Updated     : 2026-04-28
-------------------------------------------------------------------------------
 Purpose     : EF Core entity for global application configuration values.
               Backed by the `globalconfig` table.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-04-28  Initial creation to align with AppDbContext DbSet and
                         OnModelCreating mapping (globalconfig).
-------------------------------------------------------------------------------
 Notes       :
   - Additional columns in the database that are not represented here will be
     ignored by EF Core.
===============================================================================
*/

namespace AbsenceApp.Data.Models;

public sealed class GlobalConfig
{
    public int Id { get; set; }

    // Optional: a configuration key (e.g. "Menu:ShowBetaItems")
    public string Key { get; set; } = string.Empty;

    // Optional: a configuration value (JSON, string, etc.)
    public string? Value { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
