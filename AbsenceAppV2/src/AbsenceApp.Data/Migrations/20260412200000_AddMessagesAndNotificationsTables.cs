// ===========================================================================
// File        : 20260412200000_AddMessagesAndNotificationsTables.cs
// Namespace   : AbsenceApp.Data.Migrations
// Author      : Michael
// Version     : 1.0.0
// Created     : 2026-04-12
// ---------------------------------------------------------------------------
// Purpose     : Creates the dbo.Messages and dbo.AppNotifications tables.
//               These entities existed in the EF model snapshot from the
//               Baseline migration, so dotnet-ef produces an empty diff.
//               This migration is therefore authored manually.
//
//               Also seeds 5 sample messages and 5 sample notifications for
//               UserId = 1 (Super Admin) so that the header dropdowns have
//               visible data on first launch.
// ===========================================================================
using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AbsenceApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMessagesAndNotificationsTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // ── Create dbo.Messages ─────────────────────────────────────────
            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id         = table.Column<long>(type: "bigint", nullable: false),
                    UserId     = table.Column<long>(type: "bigint", nullable: false),
                    SenderName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Subject    = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Preview    = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt  = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRead     = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                });

            // ── Create dbo.AppNotifications ─────────────────────────────────
            migrationBuilder.CreateTable(
                name: "AppNotifications",
                columns: table => new
                {
                    Id        = table.Column<long>(type: "bigint", nullable: false),
                    UserId    = table.Column<long>(type: "bigint", nullable: false),
                    Title     = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Body      = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRead    = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppNotifications", x => x.Id);
                });

            // ── Seed: 5 messages for UserId = 1 (Super Admin) ───────────────
            migrationBuilder.Sql(@"
INSERT INTO [dbo].[Messages] ([Id],[UserId],[SenderName],[Subject],[Preview],[CreatedAt],[IsRead]) VALUES
(1,1,'Maria Zaman',  'Item Purchase Query',  'What is the reason of buy this item. Is it usefull for me.','2026-04-12 18:30:00',0),
(2,1,'Benny Roy',    'Product Feedback',     'What is the reason of buy this item. Is it usefull for me.','2026-04-12 10:35:00',0),
(3,1,'Steven',       'Account Information',  'What is the reason of buy this item. Is it usefull for me.','2026-04-12 02:35:00',0),
(4,1,'Joshep Joe',   'Request Approval',     'What is the reason of buy this item. Is it usefull for me.','2026-04-11 12:35:00',0),
(5,1,'Emma Wilson',  'System Notification',  'What is the reason of buy this item. Is it usefull for me.','2026-04-11 09:10:00',0);
");

            // ── Seed: 5 notifications for UserId = 1 (Super Admin) ──────────
            migrationBuilder.Sql(@"
INSERT INTO [dbo].[AppNotifications] ([Id],[UserId],[Title],[Body],[CreatedAt],[IsRead]) VALUES
(1,1,'Complete Today Task',    'You have 3 pending tasks due today.',             '2026-04-12 12:59:00',0),
(2,1,'Director Meeting',       'Board meeting scheduled for 14:00 today.',        '2026-04-12 12:40:00',0),
(3,1,'Update Password',        'Your password is 90 days old. Please update it.', '2026-04-12 12:15:00',0),
(4,1,'New Student Enrolled',   'A new student has been enrolled in Year 7.',      '2026-04-11 15:30:00',0),
(5,1,'Attendance Report Ready','The weekly attendance report is ready to review.','2026-04-11 09:00:00',0);
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "AppNotifications");
            migrationBuilder.DropTable(name: "Messages");
        }
    }
}
