using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AbsenceApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameMenuItemsGlobalConfigToGlobalSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // No-op: database already uses the correct table name.
            // This migration exists only to keep EF's history in sync.
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // No-op: nothing to roll back.
        }
    }
}
