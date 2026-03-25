using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AbsenceApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTablePageSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "table_page_settings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PageName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FieldName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DisplayLabel = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    IsSortable = table.Column<bool>(type: "bit", nullable: false),
                    IsFilterable = table.Column<bool>(type: "bit", nullable: false),
                    IsSearchable = table.Column<bool>(type: "bit", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_table_page_settings", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_table_page_settings_PageName_FieldName",
                table: "table_page_settings",
                columns: new[] { "PageName", "FieldName" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "table_page_settings");
        }
    }
}
