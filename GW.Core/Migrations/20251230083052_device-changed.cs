using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GW.Core.Migrations
{
    /// <inheritdoc />
    public partial class devicechanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FkUserRoleId",
                table: "Devices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Devices_FkUserRoleId",
                table: "Devices",
                column: "FkUserRoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_UserRoles_FkUserRoleId",
                table: "Devices",
                column: "FkUserRoleId",
                principalTable: "UserRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Devices_UserRoles_FkUserRoleId",
                table: "Devices");

            migrationBuilder.DropIndex(
                name: "IX_Devices_FkUserRoleId",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "FkUserRoleId",
                table: "Devices");
        }
    }
}
