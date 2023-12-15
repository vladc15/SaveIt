using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SaveIt.Data.Migrations
{
    public partial class Mig3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PinTags",
                table: "PinTags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PinBoards",
                table: "PinBoards");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PinTags",
                table: "PinTags",
                columns: new[] { "Id", "PinId", "TagId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_PinBoards",
                table: "PinBoards",
                columns: new[] { "Id", "PinId", "BoardId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PinTags",
                table: "PinTags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PinBoards",
                table: "PinBoards");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PinTags",
                table: "PinTags",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PinBoards",
                table: "PinBoards",
                column: "Id");
        }
    }
}
