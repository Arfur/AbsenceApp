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

const string connectionString =
    "Server=127.0.0.1;Port=3306;Database=absenceapp;User=root;Password=Calm1309!;CharSet=utf8mb4";

services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        connectionString,
        new MariaDbServerVersion(new Version(10, 4, 32))));

var provider = services.BuildServiceProvider();

// ---------------------------------------------------------------------------
// Run seeder
// ---------------------------------------------------------------------------
await using var scope = provider.CreateAsyncScope();
var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

Console.WriteLine("[EfHost] Seeding database...");
await DatabaseSeeder.SeedAsync(db, @"C:\DevAbsence2\Data CSV");
Console.WriteLine("[EfHost] Seed complete.");

