using Microsoft.EntityFrameworkCore.Migrations;

namespace Snowflake.Migrations
{
    public partial class InputV2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MappedControllerElements_ControllerElementMappings_ControllerID_DeviceID_ProfileName",
                table: "MappedControllerElements");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MappedControllerElements",
                table: "MappedControllerElements");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ControllerElementMappings",
                table: "ControllerElementMappings");

            migrationBuilder.DropColumn(
                name: "DeviceID",
                table: "MappedControllerElements");

            migrationBuilder.DropColumn(
                name: "DeviceElement",
                table: "MappedControllerElements");

            migrationBuilder.DropColumn(
                name: "DeviceID",
                table: "ControllerElementMappings");

            migrationBuilder.AddColumn<string>(
                name: "DeviceName",
                table: "MappedControllerElements",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "VendorID",
                table: "MappedControllerElements",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DriverType",
                table: "MappedControllerElements",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "DeviceCapability",
                table: "MappedControllerElements",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "DriverType",
                table: "ControllerElementMappings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "DeviceName",
                table: "ControllerElementMappings",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "VendorID",
                table: "ControllerElementMappings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MappedControllerElements",
                table: "MappedControllerElements",
                columns: new[] { "ControllerID", "DeviceName", "VendorID", "DriverType", "ProfileName", "LayoutElement" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ControllerElementMappings",
                table: "ControllerElementMappings",
                columns: new[] { "ControllerID", "DriverType", "DeviceName", "VendorID", "ProfileName" });

            migrationBuilder.CreateIndex(
                name: "IX_MappedControllerElements_ControllerID_DriverType_DeviceName_VendorID_ProfileName",
                table: "MappedControllerElements",
                columns: new[] { "ControllerID", "DriverType", "DeviceName", "VendorID", "ProfileName" });

            migrationBuilder.AddForeignKey(
                name: "FK_MappedControllerElements_ControllerElementMappings_ControllerID_DriverType_DeviceName_VendorID_ProfileName",
                table: "MappedControllerElements",
                columns: new[] { "ControllerID", "DriverType", "DeviceName", "VendorID", "ProfileName" },
                principalTable: "ControllerElementMappings",
                principalColumns: new[] { "ControllerID", "DriverType", "DeviceName", "VendorID", "ProfileName" },
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MappedControllerElements_ControllerElementMappings_ControllerID_DriverType_DeviceName_VendorID_ProfileName",
                table: "MappedControllerElements");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MappedControllerElements",
                table: "MappedControllerElements");

            migrationBuilder.DropIndex(
                name: "IX_MappedControllerElements_ControllerID_DriverType_DeviceName_VendorID_ProfileName",
                table: "MappedControllerElements");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ControllerElementMappings",
                table: "ControllerElementMappings");

            migrationBuilder.DropColumn(
                name: "DeviceName",
                table: "MappedControllerElements");

            migrationBuilder.DropColumn(
                name: "VendorID",
                table: "MappedControllerElements");

            migrationBuilder.DropColumn(
                name: "DriverType",
                table: "MappedControllerElements");

            migrationBuilder.DropColumn(
                name: "DeviceCapability",
                table: "MappedControllerElements");

            migrationBuilder.DropColumn(
                name: "DriverType",
                table: "ControllerElementMappings");

            migrationBuilder.DropColumn(
                name: "DeviceName",
                table: "ControllerElementMappings");

            migrationBuilder.DropColumn(
                name: "VendorID",
                table: "ControllerElementMappings");

            migrationBuilder.AddColumn<string>(
                name: "DeviceID",
                table: "MappedControllerElements",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeviceElement",
                table: "MappedControllerElements",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeviceID",
                table: "ControllerElementMappings",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MappedControllerElements",
                table: "MappedControllerElements",
                columns: new[] { "ControllerID", "DeviceID", "ProfileName", "LayoutElement" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ControllerElementMappings",
                table: "ControllerElementMappings",
                columns: new[] { "ControllerID", "DeviceID", "ProfileName" });

            migrationBuilder.AddForeignKey(
                name: "FK_MappedControllerElements_ControllerElementMappings_ControllerID_DeviceID_ProfileName",
                table: "MappedControllerElements",
                columns: new[] { "ControllerID", "DeviceID", "ProfileName" },
                principalTable: "ControllerElementMappings",
                principalColumns: new[] { "ControllerID", "DeviceID", "ProfileName" },
                onDelete: ReferentialAction.Cascade);
        }
    }
}
