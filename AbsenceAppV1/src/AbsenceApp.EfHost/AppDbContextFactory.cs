/*
===============================================================================
 File        : AppDbContextFactory.cs
 Namespace   : AbsenceApp.EfHost
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-13
 Updated     : 2026-03-13
-------------------------------------------------------------------------------
 Purpose     : Implements IDesignTimeDbContextFactory<AppDbContext> so that
               'dotnet ef migrations' and 'dotnet ef database update' can
               resolve AppDbContext at design time without a running host.
-------------------------------------------------------------------------------
 Description :
   Used exclusively as the --startup-project for EF Core CLI commands.
   Creates an AppDbContext configured with a SQL Server LocalDB connection
   string suitable for local development and migration generation.
   At runtime the application uses the connection string from appsettings.json
   registered via DataServiceRegistration.AddDataLayer().
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-13  Initial creation.
-------------------------------------------------------------------------------
 Notes       :
   - This project is not intended to be executed; its sole purpose is to
     provide a design-time DbContext factory for the EF Core tooling.
   - Update the LocalDB connection string if a different SQL Server instance
     is used for local development.
===============================================================================
*/

using AbsenceApp.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AbsenceApp.EfHost;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    // =========================================================================
    // IDesignTimeDbContextFactory<AppDbContext> implementation
    // =========================================================================

    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

        // Uses SQL Server LocalDB for local development migrations.
        // Adjust the connection string for other environments.
        optionsBuilder.UseSqlServer(
            "Server=(localdb)\\mssqllocaldb;Database=AbsenceApp;Trusted_Connection=True;MultipleActiveResultSets=true");

        return new AppDbContext(optionsBuilder.Options);
    }
}
