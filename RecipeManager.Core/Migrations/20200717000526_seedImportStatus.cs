using Microsoft.EntityFrameworkCore.Migrations;

namespace RecipeManager.Core.Migrations
{
    public partial class seedImportStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ImportStatus",
                columns: new[] { "Name" },
                values: new object[] { "Success" });

            migrationBuilder.InsertData(
                table: "ImportStatus",
                columns: new[] { "Name" },
                values: new object[] { "Working" });

            migrationBuilder.InsertData(
                table: "ImportStatus",
                columns: new[] { "Name" },
                values: new object[] { "Fail" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(table: "ImportStatus", "Name", "Success");
            migrationBuilder.DeleteData(table: "ImportStatus", "Name", "Working");
            migrationBuilder.DeleteData(table: "ImportStatus", "Name", "Fail");

        }
    }
}
