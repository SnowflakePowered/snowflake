using Microsoft.EntityFrameworkCore.Migrations;

namespace Snowflake.Model.Database.Migrations
{
    public partial class WithValueType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ValueType",
                table: "ConfigurationValues",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ValueType",
                table: "ConfigurationValues");
        }
    }
}
