using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AbsenceApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddActionButtonTokens : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Inserts 16 new action-btn design token rows (IDs 1100–1115).
            // The Phase D tokens (IDs 200–1010) were added directly to the
            // database outside EF migrations and are not tracked here.
            migrationBuilder.InsertData(
                table: "DesignTokens",
                columns: new[] { "Id", "Category", "ComponentGroup", "CreatedAt", "CssVariable", "CurrentValue", "DefaultValue", "Description", "IsActive", "SortOrder", "TokenKey", "UpdatedAt" },
                values: new object[,]
                {
                    { 1100, "color",      "action-btn", new DateTime(2026, 5, 19, 0, 0, 0, 0, DateTimeKind.Utc), "--ds-action-btn-primary-bg",         null, "TBD", "Action button primary background",         true, 1100, "primary-bg",         new DateTime(2026, 5, 19, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 1101, "color",      "action-btn", new DateTime(2026, 5, 19, 0, 0, 0, 0, DateTimeKind.Utc), "--ds-action-btn-primary-hover-bg",   null, "TBD", "Action button primary hover background",   true, 1101, "primary-hover-bg",   new DateTime(2026, 5, 19, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 1102, "color",      "action-btn", new DateTime(2026, 5, 19, 0, 0, 0, 0, DateTimeKind.Utc), "--ds-action-btn-primary-text",       null, "TBD", "Action button primary text",               true, 1102, "primary-text",       new DateTime(2026, 5, 19, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 1103, "color",      "action-btn", new DateTime(2026, 5, 19, 0, 0, 0, 0, DateTimeKind.Utc), "--ds-action-btn-primary-icon",       null, "TBD", "Action button primary icon colour",        true, 1103, "primary-icon",       new DateTime(2026, 5, 19, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 1104, "color",      "action-btn", new DateTime(2026, 5, 19, 0, 0, 0, 0, DateTimeKind.Utc), "--ds-action-btn-secondary-bg",       null, "TBD", "Action button secondary background",       true, 1104, "secondary-bg",       new DateTime(2026, 5, 19, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 1105, "color",      "action-btn", new DateTime(2026, 5, 19, 0, 0, 0, 0, DateTimeKind.Utc), "--ds-action-btn-secondary-hover-bg", null, "TBD", "Action button secondary hover background", true, 1105, "secondary-hover-bg", new DateTime(2026, 5, 19, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 1106, "color",      "action-btn", new DateTime(2026, 5, 19, 0, 0, 0, 0, DateTimeKind.Utc), "--ds-action-btn-secondary-text",     null, "TBD", "Action button secondary text",             true, 1106, "secondary-text",     new DateTime(2026, 5, 19, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 1107, "color",      "action-btn", new DateTime(2026, 5, 19, 0, 0, 0, 0, DateTimeKind.Utc), "--ds-action-btn-secondary-icon",     null, "TBD", "Action button secondary icon colour",      true, 1107, "secondary-icon",     new DateTime(2026, 5, 19, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 1108, "radius",     "action-btn", new DateTime(2026, 5, 19, 0, 0, 0, 0, DateTimeKind.Utc), "--ds-action-btn-radius",             null, "TBD", "Action button border radius",              true, 1108, "radius",             new DateTime(2026, 5, 19, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 1109, "spacing",    "action-btn", new DateTime(2026, 5, 19, 0, 0, 0, 0, DateTimeKind.Utc), "--ds-action-btn-padding-x",          null, "TBD", "Action button horizontal padding",         true, 1109, "padding-x",          new DateTime(2026, 5, 19, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 1110, "spacing",    "action-btn", new DateTime(2026, 5, 19, 0, 0, 0, 0, DateTimeKind.Utc), "--ds-action-btn-padding-y",          null, "TBD", "Action button vertical padding",           true, 1110, "padding-y",          new DateTime(2026, 5, 19, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 1111, "typography", "action-btn", new DateTime(2026, 5, 19, 0, 0, 0, 0, DateTimeKind.Utc), "--ds-action-btn-font-size",          null, "TBD", "Action button default font size",          true, 1111, "font-size",          new DateTime(2026, 5, 19, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 1112, "typography", "action-btn", new DateTime(2026, 5, 19, 0, 0, 0, 0, DateTimeKind.Utc), "--ds-action-btn-font-size-sm",       null, "TBD", "Action button small font size",            true, 1112, "font-size-sm",       new DateTime(2026, 5, 19, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 1113, "typography", "action-btn", new DateTime(2026, 5, 19, 0, 0, 0, 0, DateTimeKind.Utc), "--ds-action-btn-font-size-lg",       null, "TBD", "Action button large font size",            true, 1113, "font-size-lg",       new DateTime(2026, 5, 19, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 1114, "typography", "action-btn", new DateTime(2026, 5, 19, 0, 0, 0, 0, DateTimeKind.Utc), "--ds-action-btn-icon-size",          null, "TBD", "Action button icon size",                  true, 1114, "icon-size",          new DateTime(2026, 5, 19, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 1115, "structure",  "action-btn", new DateTime(2026, 5, 19, 0, 0, 0, 0, DateTimeKind.Utc), "--ds-action-btn-icon-only-size",     null, "TBD", "Action button icon-only square size",      true, 1115, "icon-only-size",     new DateTime(2026, 5, 19, 0, 0, 0, 0, DateTimeKind.Utc) },
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(table: "DesignTokens", keyColumn: "Id", keyValue: 1100);
            migrationBuilder.DeleteData(table: "DesignTokens", keyColumn: "Id", keyValue: 1101);
            migrationBuilder.DeleteData(table: "DesignTokens", keyColumn: "Id", keyValue: 1102);
            migrationBuilder.DeleteData(table: "DesignTokens", keyColumn: "Id", keyValue: 1103);
            migrationBuilder.DeleteData(table: "DesignTokens", keyColumn: "Id", keyValue: 1104);
            migrationBuilder.DeleteData(table: "DesignTokens", keyColumn: "Id", keyValue: 1105);
            migrationBuilder.DeleteData(table: "DesignTokens", keyColumn: "Id", keyValue: 1106);
            migrationBuilder.DeleteData(table: "DesignTokens", keyColumn: "Id", keyValue: 1107);
            migrationBuilder.DeleteData(table: "DesignTokens", keyColumn: "Id", keyValue: 1108);
            migrationBuilder.DeleteData(table: "DesignTokens", keyColumn: "Id", keyValue: 1109);
            migrationBuilder.DeleteData(table: "DesignTokens", keyColumn: "Id", keyValue: 1110);
            migrationBuilder.DeleteData(table: "DesignTokens", keyColumn: "Id", keyValue: 1111);
            migrationBuilder.DeleteData(table: "DesignTokens", keyColumn: "Id", keyValue: 1112);
            migrationBuilder.DeleteData(table: "DesignTokens", keyColumn: "Id", keyValue: 1113);
            migrationBuilder.DeleteData(table: "DesignTokens", keyColumn: "Id", keyValue: 1114);
            migrationBuilder.DeleteData(table: "DesignTokens", keyColumn: "Id", keyValue: 1115);
        }
    }
}
