using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SaveIt.Data.Migrations
{
    public partial class adaugareVideo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "mediaType",
                table: "Pins",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "mediaType",
                table: "Pins");
        }
    }
}
