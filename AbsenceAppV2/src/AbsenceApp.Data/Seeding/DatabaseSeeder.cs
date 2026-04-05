/*
===============================================================================
 File        : DatabaseSeeder.cs
 Namespace   : AbsenceApp.Data.Seeding
 Author      : Michael
 Version     : 1.1.0
 Created     : 2026-03-15
 Updated     : 2026-04-05
-------------------------------------------------------------------------------
 Purpose     : Orchestrates database schema initialisation and CSV data import
               for the full 40-table schema.
               Delegates file loading to CsvImportPipeline.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-15  Initial creation.
   - 1.1.0  2026-04-05  Phase 3 Header Nav Identity: added SeedDevMessagingAsync
                         which inserts dev-only Message and AppNotification rows
                         for user 'mbattle' when the tables are empty.
-------------------------------------------------------------------------------
 Notes       :
   - Passing null or omitting csvDirectory is safe; the import step is skipped.
   - Intended to be called once on application startup or EF Host startup.
   - Dev messaging seed runs ONLY when Messages table is empty (idempotent).
===============================================================================
*/

using AbsenceApp.Data.Context;
using AbsenceApp.Data.Models;
using Microsoft.EntityFrameworkCore;

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
        {
            await SeedDevMessagingAsync(context);
            return;
        }

        var pipeline = new CsvImportPipeline(context);
        await pipeline.RunAllAsync(csvDirectory);

        await SeedDevMessagingAsync(context);
    }

    // =========================================================================
    // SeedDevMessagingAsync — idempotent dev seed for Messages + AppNotifications
    // =========================================================================

    /// <summary>
    /// Inserts dev-only Message and AppNotification rows for user 'mbattle'.
    /// Runs only when the Messages table is empty, making it safe to call on
    /// every startup.  Does NOT run in production (CSV import replaces this).
    /// </summary>
    private static async Task SeedDevMessagingAsync(AppDbContext context)
    {
        // Resolve mbattle's user Id; skip if user does not exist yet.
        var mbattleUser = await context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Username == "mbattle");

        if (mbattleUser is null)
            return;

        // ── Messages ──────────────────────────────────────────────────────────
        if (!await context.Messages.AnyAsync())
        {
            var now = DateTime.UtcNow;
            context.Messages.AddRange(
                // 3 unread messages
                new Message
                {
                    UserId     = mbattleUser.Id,
                    SenderName = "Sarah Jensen",
                    Subject    = "Year 6 Attendance Query",
                    Preview    = "Please check the attendance for Year 6 — three students are marked absent but were here.",
                    CreatedAt  = now.AddMinutes(-10),
                    IsRead     = false,
                },
                new Message
                {
                    UserId     = mbattleUser.Id,
                    SenderName = "James Okoro",
                    Subject    = "New Schedule Upload",
                    Preview    = "The new schedule has been uploaded and is ready for review by the admin team.",
                    CreatedAt  = now.AddHours(-1),
                    IsRead     = false,
                },
                new Message
                {
                    UserId     = mbattleUser.Id,
                    SenderName = "Priya Nair",
                    Subject    = "Absence Report — Week 14",
                    Preview    = "Week 14 absence report is attached. Overall rates are within acceptable thresholds.",
                    CreatedAt  = now.AddHours(-3),
                    IsRead     = false,
                },
                // 1 read message (not surfaced in unread dropdown)
                new Message
                {
                    UserId     = mbattleUser.Id,
                    SenderName = "System",
                    Subject    = "Welcome to AbsenceApp V2",
                    Preview    = "Your account has been configured. Please review your profile settings.",
                    CreatedAt  = now.AddDays(-2),
                    IsRead     = true,
                }
            );
            await context.SaveChangesAsync();
        }

        // ── Notifications ─────────────────────────────────────────────────────
        if (!await context.AppNotifications.AnyAsync())
        {
            var now = DateTime.UtcNow;
            context.AppNotifications.AddRange(
                // 2 unread notifications
                new AppNotification
                {
                    UserId    = mbattleUser.Id,
                    Title     = "3 students absent today",
                    Body      = "Kemi Adeyemi, Luca Borrelli, and Tom Walsh are marked absent with no reason provided.",
                    CreatedAt = now.AddMinutes(-5),
                    IsRead    = false,
                },
                new AppNotification
                {
                    UserId    = mbattleUser.Id,
                    Title     = "Attendance report ready",
                    Body      = "The weekly attendance summary report is ready to download from the Reports dashboard.",
                    CreatedAt = now.AddHours(-2),
                    IsRead    = false,
                },
                // 2 read notifications
                new AppNotification
                {
                    UserId    = mbattleUser.Id,
                    Title     = "New user registration pending",
                    Body      = "A new user registration request is awaiting admin approval.",
                    CreatedAt = now.AddHours(-3),
                    IsRead    = true,
                },
                new AppNotification
                {
                    UserId    = mbattleUser.Id,
                    Title     = "System backup completed",
                    Body      = "Nightly database backup completed successfully.",
                    CreatedAt = now.AddDays(-1),
                    IsRead    = true,
                }
            );
            await context.SaveChangesAsync();
        }
    }
}
