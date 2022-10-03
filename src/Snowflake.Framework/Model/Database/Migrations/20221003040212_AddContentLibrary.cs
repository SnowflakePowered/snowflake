using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Snowflake.Model.Database.Migrations
{
    public partial class AddContentLibrary : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ContentLibraryLibraryID",
                table: "Records",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ContentLibraries",
                columns: table => new
                {
                    LibraryID = table.Column<Guid>(type: "TEXT", nullable: false),
                    Path = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentLibraries", x => x.LibraryID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Records_ContentLibraryLibraryID",
                table: "Records",
                column: "ContentLibraryLibraryID");

            migrationBuilder.AddForeignKey(
                name: "FK_Records_ContentLibraries_ContentLibraryLibraryID",
                table: "Records",
                column: "ContentLibraryLibraryID",
                principalTable: "ContentLibraries",
                principalColumn: "LibraryID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Records_ContentLibraries_ContentLibraryLibraryID",
                table: "Records");

            migrationBuilder.DropTable(
                name: "ContentLibraries");

            migrationBuilder.DropIndex(
                name: "IX_Records_ContentLibraryLibraryID",
                table: "Records");

            migrationBuilder.DropColumn(
                name: "ContentLibraryLibraryID",
                table: "Records");
        }
    }
}
