using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SaveIt.Data.Migrations
{
    public partial class Mig2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PinTag_Pins_PinId",
                table: "PinTag");

            migrationBuilder.DropForeignKey(
                name: "FK_PinTag_Tags_TagId",
                table: "PinTag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PinTag",
                table: "PinTag");

            migrationBuilder.RenameTable(
                name: "PinTag",
                newName: "PinTags");

            migrationBuilder.RenameIndex(
                name: "IX_PinTag_TagId",
                table: "PinTags",
                newName: "IX_PinTags_TagId");

            migrationBuilder.RenameIndex(
                name: "IX_PinTag_PinId",
                table: "PinTags",
                newName: "IX_PinTags_PinId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PinTags",
                table: "PinTags",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PinTags_Pins_PinId",
                table: "PinTags",
                column: "PinId",
                principalTable: "Pins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PinTags_Tags_TagId",
                table: "PinTags",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PinTags_Pins_PinId",
                table: "PinTags");

            migrationBuilder.DropForeignKey(
                name: "FK_PinTags_Tags_TagId",
                table: "PinTags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PinTags",
                table: "PinTags");

            migrationBuilder.RenameTable(
                name: "PinTags",
                newName: "PinTag");

            migrationBuilder.RenameIndex(
                name: "IX_PinTags_TagId",
                table: "PinTag",
                newName: "IX_PinTag_TagId");

            migrationBuilder.RenameIndex(
                name: "IX_PinTags_PinId",
                table: "PinTag",
                newName: "IX_PinTag_PinId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PinTag",
                table: "PinTag",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PinTag_Pins_PinId",
                table: "PinTag",
                column: "PinId",
                principalTable: "Pins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PinTag_Tags_TagId",
                table: "PinTag",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
