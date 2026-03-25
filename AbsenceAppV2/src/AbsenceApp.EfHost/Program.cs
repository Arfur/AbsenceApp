/*
===============================================================================
 File        : Program.cs
 Namespace   : AbsenceApp.EfHost
 Author      : Michael
 Version     : 1.1.0
 Created     : 2026-03-13
 Updated     : 2026-03-13
-------------------------------------------------------------------------------
 Purpose     : Minimal console host used as the EF Core startup project for
               dotnet ef migrations / database update.  Also serves as a
               convenient entrypoint to run DatabaseSeeder during development.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-13  Initial stub created.
   - 1.1.0  2026-03-13  Wired up DatabaseSeeder and DI pipeline.
===============================================================================
*/

using AbsenceApp.Data;
using AbsenceApp.Data.Context;
using AbsenceApp.Data.Seeding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

// ---------------------------------------------------------------------------
// Build service collection
// ---------------------------------------------------------------------------
var services = new ServiceCollection();

// Use an InMemory database for local running; swap for SqlServer with a real
// connection string when running against a provisioned database.
services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        "Server=(localdb)\\mssqllocaldb;Database=AbsenceApp;Trusted_Connection=True;MultipleActiveResultSets=true"));

var provider = services.BuildServiceProvider();

// ---------------------------------------------------------------------------
// Run seeder
// ---------------------------------------------------------------------------
await using var scope = provider.CreateAsyncScope();
var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

Console.WriteLine("[EfHost] Seeding database...");
await DatabaseSeeder.SeedAsync(db, @"C:\DevAbsence1\Data CSV");
Console.WriteLine("[EfHost] Seed complete.");

