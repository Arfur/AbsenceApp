using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AbsenceApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCardDesignTokens : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // The DesignTokens table and all btn tokens (IDs 10-73) already exist
            // in the database (created manually in Phase A outside EF migrations).
            // This migration only inserts the 6 new card design token rows.
            migrationBuilder.InsertData(
                table: "DesignTokens",
                columns: new[] { "Id", "Category", "ComponentGroup", "CreatedAt", "CssVariable", "CurrentValue", "DefaultValue", "Description", "IsActive", "SortOrder", "TokenKey", "UpdatedAt" },
                values: new object[,]
                {
                    { 100, "color",     "card", new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Utc), "--ds-card-bg",          null, "#ffffff",                     "Card background colour",        true, 100, "bg",           new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 101, "color",     "card", new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Utc), "--ds-card-border-color", null, "#dee2e6",                     "Card border colour",            true, 101, "border-color", new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 102, "structure", "card", new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Utc), "--ds-card-radius",       null, "8px",                         "Card border radius",            true, 102, "radius",       new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 103, "structure", "card", new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Utc), "--ds-card-shadow",       null, "0 1px 4px rgba(0,0,0,0.06)", "Card box shadow",               true, 103, "shadow",       new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 104, "color",     "card", new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Utc), "--ds-card-header-bg",    null, "#f8f9fa",                     "Card header background colour", true, 104, "header-bg",    new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 105, "structure", "card", new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Utc), "--ds-card-padding",      null, "1.25rem",                     "Card body padding",             true, 105, "padding",      new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Utc) },
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(table: "DesignTokens", keyColumn: "Id", keyValue: 100);
            migrationBuilder.DeleteData(table: "DesignTokens", keyColumn: "Id", keyValue: 101);
            migrationBuilder.DeleteData(table: "DesignTokens", keyColumn: "Id", keyValue: 102);
            migrationBuilder.DeleteData(table: "DesignTokens", keyColumn: "Id", keyValue: 103);
            migrationBuilder.DeleteData(table: "DesignTokens", keyColumn: "Id", keyValue: 104);
            migrationBuilder.DeleteData(table: "DesignTokens", keyColumn: "Id", keyValue: 105);
        }
    }
}
