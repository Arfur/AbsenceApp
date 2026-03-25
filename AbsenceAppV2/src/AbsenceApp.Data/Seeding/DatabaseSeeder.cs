/*
===============================================================================
 File        : DatabaseSeeder.cs
 Namespace   : AbsenceApp.Data.Seeding
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-15
 Updated     : 2026-03-15
-------------------------------------------------------------------------------
 Purpose     : Orchestrates database schema initialisation and CSV data import
               for the full 40-table schema.
               Delegates file loading to CsvImportPipeline.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-15  Initial creation.
-------------------------------------------------------------------------------
 Notes       :
   - Passing null or omitting csvDirectory is safe; the import step is skipped.
   - Intended to be called once on application startup or EF Host startup.
===============================================================================
*/

using AbsenceApp.Data.Context;

namespace AbsenceApp.Data.Seeding;

// =========================================================================
// DatabaseSeeder -- schema initialisation + CSV import orchestrator
// =========================================================================

public static class DatabaseSeeder
{
    // =========================================================================
    // SeedAsync -- entry point
    // =========================================================================

    /// <summary>
    /// Ensures the schema exists, then imports all CSV files found in
    /// <paramref name="csvDirectory"/> in FK-dependency order.
    /// Pass <c>null</c> (or omit) for <paramref name="csvDirectory"/> to skip
    /// the import step (useful in unit tests that seed data manually).
    /// </summary>
    public static async Task SeedAsync(AppDbContext context, string? csvDirectory = null)
    {
        await context.Database.EnsureCreatedAsync();

        if (string.IsNullOrWhiteSpace(csvDirectory))
            return;

        var pipeline = new CsvImportPipeline(context);
        await pipeline.RunAllAsync(csvDirectory);
    }
}
