using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AbsenceApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDropdownTokens : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Inserts 16 new dropdown design token rows (IDs 1200–1215).
            migrationBuilder.InsertData(
                table: "DesignTokens",
                columns: new[] { "Id", "Category", "ComponentGroup", "CreatedAt", "CssVariable", "CurrentValue", "DefaultValue", "Description", "IsActive", "SortOrder", "TokenKey", "UpdatedAt" },
                values: new object[,]
                {
                    { 1200, "color",      "dropdown", new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Utc), "--ds-dropdown-bg",              null, "TBD", "Dropdown background colour",               true, 1200, "bg",              new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 1201, "color",      "dropdown", new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Utc), "--ds-dropdown-hover-bg",        null, "TBD", "Dropdown trigger hover background",        true, 1201, "hover-bg",        new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 1202, "color",      "dropdown", new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Utc), "--ds-dropdown-text",            null, "TBD", "Dropdown trigger text colour",             true, 1202, "text",            new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 1203, "color",      "dropdown", new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Utc), "--ds-dropdown-icon",            null, "TBD", "Dropdown trigger icon colour",             true, 1203, "icon",            new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 1204, "color",      "dropdown", new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Utc), "--ds-dropdown-border",          null, "TBD", "Dropdown border colour",                   true, 1204, "border",          new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 1205, "radius",     "dropdown", new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Utc), "--ds-dropdown-radius",          null, "TBD", "Dropdown border radius",                   true, 1205, "radius",          new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 1206, "shadow",     "dropdown", new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Utc), "--ds-dropdown-shadow",          null, "TBD", "Dropdown menu box shadow",                 true, 1206, "shadow",          new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 1207, "spacing",    "dropdown", new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Utc), "--ds-dropdown-padding-x",       null, "TBD", "Dropdown trigger horizontal padding",      true, 1207, "padding-x",       new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 1208, "spacing",    "dropdown", new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Utc), "--ds-dropdown-padding-y",       null, "TBD", "Dropdown trigger vertical padding",        true, 1208, "padding-y",       new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 1209, "typography", "dropdown", new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Utc), "--ds-dropdown-font-size",       null, "TBD", "Dropdown default font size",               true, 1209, "font-size",       new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 1210, "typography", "dropdown", new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Utc), "--ds-dropdown-font-size-sm",    null, "TBD", "Dropdown small font size",                 true, 1210, "font-size-sm",    new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 1211, "typography", "dropdown", new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Utc), "--ds-dropdown-font-size-lg",    null, "TBD", "Dropdown large font size",                 true, 1211, "font-size-lg",    new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 1212, "color",      "dropdown", new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Utc), "--ds-dropdown-item-hover-bg",   null, "TBD", "Dropdown menu item hover background",      true, 1212, "item-hover-bg",   new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 1213, "color",      "dropdown", new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Utc), "--ds-dropdown-item-hover-text", null, "TBD", "Dropdown menu item hover text colour",     true, 1213, "item-hover-text", new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 1214, "structure",  "dropdown", new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Utc), "--ds-dropdown-zindex",          null, "TBD", "Dropdown menu z-index",                    true, 1214, "zindex",          new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 1215, "structure",  "dropdown", new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Utc), "--ds-dropdown-width",           null, "TBD", "Dropdown menu minimum width",              true, 1215, "width",           new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Utc) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(table: "DesignTokens", keyColumn: "Id", keyValue: 1200);
            migrationBuilder.DeleteData(table: "DesignTokens", keyColumn: "Id", keyValue: 1201);
            migrationBuilder.DeleteData(table: "DesignTokens", keyColumn: "Id", keyValue: 1202);
            migrationBuilder.DeleteData(table: "DesignTokens", keyColumn: "Id", keyValue: 1203);
            migrationBuilder.DeleteData(table: "DesignTokens", keyColumn: "Id", keyValue: 1204);
            migrationBuilder.DeleteData(table: "DesignTokens", keyColumn: "Id", keyValue: 1205);
            migrationBuilder.DeleteData(table: "DesignTokens", keyColumn: "Id", keyValue: 1206);
            migrationBuilder.DeleteData(table: "DesignTokens", keyColumn: "Id", keyValue: 1207);
            migrationBuilder.DeleteData(table: "DesignTokens", keyColumn: "Id", keyValue: 1208);
            migrationBuilder.DeleteData(table: "DesignTokens", keyColumn: "Id", keyValue: 1209);
            migrationBuilder.DeleteData(table: "DesignTokens", keyColumn: "Id", keyValue: 1210);
            migrationBuilder.DeleteData(table: "DesignTokens", keyColumn: "Id", keyValue: 1211);
            migrationBuilder.DeleteData(table: "DesignTokens", keyColumn: "Id", keyValue: 1212);
            migrationBuilder.DeleteData(table: "DesignTokens", keyColumn: "Id", keyValue: 1213);
            migrationBuilder.DeleteData(table: "DesignTokens", keyColumn: "Id", keyValue: 1214);
            migrationBuilder.DeleteData(table: "DesignTokens", keyColumn: "Id", keyValue: 1215);
        }
    }
}
