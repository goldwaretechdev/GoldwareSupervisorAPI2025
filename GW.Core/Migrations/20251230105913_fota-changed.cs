using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GW.Core.Migrations
{
    /// <inheritdoc />
    public partial class fotachanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "FOTA",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "FOTA");
        }
    }
}
