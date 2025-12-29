using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GW.Core.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Company",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    ShortName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Charge = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Units",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Units", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Mobile = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Access",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FkUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FkUnitId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Access", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Access_Units_FkUnitId",
                        column: x => x.FkUnitId,
                        principalTable: "Units",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Access_Users_FkUserId",
                        column: x => x.FkUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserAndCompany",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FkUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FkCompanyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAndCompany", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAndCompany_Company_FkCompanyId",
                        column: x => x.FkCompanyId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserAndCompany_Users_FkUserId",
                        column: x => x.FkUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FkUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FkRoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_FkRoleId",
                        column: x => x.FkRoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_FkUserId",
                        column: x => x.FkUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SoftwareVersions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Version = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MicroType = table.Column<int>(type: "int", nullable: false),
                    DeviceType = table.Column<int>(type: "int", nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FkUserRoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoftwareVersions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SoftwareVersions_UserRoles_FkUserRoleId",
                        column: x => x.FkUserRoleId,
                        principalTable: "UserRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Devices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UniqueId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    ProductCategory = table.Column<int>(type: "int", nullable: false),
                    BatchNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SerialNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ProductionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HardwareVersion = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    MAC = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IMEI = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FkOwnerId = table.Column<int>(type: "int", nullable: false),
                    FkESPId = table.Column<int>(type: "int", nullable: false),
                    FkSTMId = table.Column<int>(type: "int", nullable: false),
                    FkHoltekId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Devices_Company_FkOwnerId",
                        column: x => x.FkOwnerId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Devices_SoftwareVersions_FkESPId",
                        column: x => x.FkESPId,
                        principalTable: "SoftwareVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Devices_SoftwareVersions_FkHoltekId",
                        column: x => x.FkHoltekId,
                        principalTable: "SoftwareVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Devices_SoftwareVersions_FkSTMId",
                        column: x => x.FkSTMId,
                        principalTable: "SoftwareVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FOTA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductCategory = table.Column<int>(type: "int", nullable: true),
                    BatchNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SerialNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ExpireDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProductionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastUpdate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HardwareVersion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MAC = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IMEI = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FkOwnerId = table.Column<int>(type: "int", nullable: true),
                    FkESPId = table.Column<int>(type: "int", nullable: true),
                    FkSTMId = table.Column<int>(type: "int", nullable: true),
                    FkHoltekId = table.Column<int>(type: "int", nullable: true),
                    FkUserRoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FOTA", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FOTA_Company_FkOwnerId",
                        column: x => x.FkOwnerId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FOTA_SoftwareVersions_FkESPId",
                        column: x => x.FkESPId,
                        principalTable: "SoftwareVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FOTA_SoftwareVersions_FkHoltekId",
                        column: x => x.FkHoltekId,
                        principalTable: "SoftwareVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FOTA_SoftwareVersions_FkSTMId",
                        column: x => x.FkSTMId,
                        principalTable: "SoftwareVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FOTA_UserRoles_FkUserRoleId",
                        column: x => x.FkUserRoleId,
                        principalTable: "UserRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Desc = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    FkDeviceId = table.Column<int>(type: "int", nullable: false),
                    FkUserRoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Logs_Devices_FkDeviceId",
                        column: x => x.FkDeviceId,
                        principalTable: "Devices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Logs_UserRoles_FkUserRoleId",
                        column: x => x.FkUserRoleId,
                        principalTable: "UserRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Company",
                columns: new[] { "Id", "Charge", "CreatedOn", "Icon", "Name", "ShortName", "UpdatedOn" },
                values: new object[,]
                {
                    { 1, 100000000, new DateTime(2025, 12, 11, 13, 0, 0, 0, DateTimeKind.Local), null, "Goldware", "GW", null },
                    { 2, 3, new DateTime(2025, 12, 11, 13, 0, 0, 0, DateTimeKind.Local), null, "ASA Service", "ASA", null }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Development" },
                    { 2, "Production" },
                    { 3, "Collaborators" },
                    { 4, "Support" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "FName", "LName", "Mobile", "Password", "Username" },
                values: new object[] { new Guid("d3b3c29a-4e2c-4b25-b6f4-2f8ebc4a1f05"), "s", "hasanabadi", "09155909973", "$2a$13$IJx6qkqyuUqmuM7NLKZDM.V1SnroBT0ICRHtcaS1AwYkr6z4p1xr6", "sdh" });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "Id", "FkRoleId", "FkUserId" },
                values: new object[] { 1, 1, new Guid("d3b3c29a-4e2c-4b25-b6f4-2f8ebc4a1f05") });

            migrationBuilder.InsertData(
                table: "SoftwareVersions",
                columns: new[] { "Id", "Category", "DateTime", "DeviceType", "FkUserRoleId", "MicroType", "Path", "Version" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2025, 12, 11, 13, 0, 0, 0, DateTimeKind.Local), 1, 1, 3, "", "ESP01" },
                    { 2, 1, new DateTime(2025, 12, 11, 13, 0, 0, 0, DateTimeKind.Local), 1, 1, 3, "", "ESP02" },
                    { 3, 1, new DateTime(2025, 12, 11, 13, 0, 0, 0, DateTimeKind.Local), 1, 1, 1, "", "HT01" },
                    { 4, 1, new DateTime(2025, 12, 11, 13, 0, 0, 0, DateTimeKind.Local), 1, 1, 1, "", "HT02" },
                    { 5, 1, new DateTime(2025, 12, 11, 13, 0, 0, 0, DateTimeKind.Local), 1, 1, 2, "", "STM01" },
                    { 6, 1, new DateTime(2025, 12, 11, 13, 0, 0, 0, DateTimeKind.Local), 1, 1, 2, "", "STM02" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Access_FkUnitId",
                table: "Access",
                column: "FkUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Access_FkUserId",
                table: "Access",
                column: "FkUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_FkESPId",
                table: "Devices",
                column: "FkESPId");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_FkHoltekId",
                table: "Devices",
                column: "FkHoltekId");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_FkOwnerId",
                table: "Devices",
                column: "FkOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_FkSTMId",
                table: "Devices",
                column: "FkSTMId");

            migrationBuilder.CreateIndex(
                name: "IX_FOTA_FkESPId",
                table: "FOTA",
                column: "FkESPId");

            migrationBuilder.CreateIndex(
                name: "IX_FOTA_FkHoltekId",
                table: "FOTA",
                column: "FkHoltekId");

            migrationBuilder.CreateIndex(
                name: "IX_FOTA_FkOwnerId",
                table: "FOTA",
                column: "FkOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_FOTA_FkSTMId",
                table: "FOTA",
                column: "FkSTMId");

            migrationBuilder.CreateIndex(
                name: "IX_FOTA_FkUserRoleId",
                table: "FOTA",
                column: "FkUserRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Logs_FkDeviceId",
                table: "Logs",
                column: "FkDeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_Logs_FkUserRoleId",
                table: "Logs",
                column: "FkUserRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_SoftwareVersions_FkUserRoleId",
                table: "SoftwareVersions",
                column: "FkUserRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAndCompany_FkCompanyId",
                table: "UserAndCompany",
                column: "FkCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAndCompany_FkUserId",
                table: "UserAndCompany",
                column: "FkUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_FkRoleId",
                table: "UserRoles",
                column: "FkRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_FkUserId",
                table: "UserRoles",
                column: "FkUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Access");

            migrationBuilder.DropTable(
                name: "FOTA");

            migrationBuilder.DropTable(
                name: "Logs");

            migrationBuilder.DropTable(
                name: "UserAndCompany");

            migrationBuilder.DropTable(
                name: "Units");

            migrationBuilder.DropTable(
                name: "Devices");

            migrationBuilder.DropTable(
                name: "Company");

            migrationBuilder.DropTable(
                name: "SoftwareVersions");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
