using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TecCore.Migrations.TecCore
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Devices",
                columns: table => new
                {
                    Uid = table.Column<Guid>(type: "TEXT", nullable: false),
                    DisplayName = table.Column<string>(type: "TEXT", nullable: false),
                    DeviceId = table.Column<string>(type: "TEXT", nullable: false),
                    OperatingSystem = table.Column<string>(type: "TEXT", nullable: false),
                    OperatingSystemVersion = table.Column<string>(type: "TEXT", nullable: false),
                    IsManaged = table.Column<bool>(type: "INTEGER", nullable: false),
                    EnrolledDateTime = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    LastSyncDateTime = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    DeviceCategory = table.Column<string>(type: "TEXT", nullable: false),
                    Manufacturer = table.Column<string>(type: "TEXT", nullable: false),
                    Model = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => x.Uid);
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    Uid = table.Column<Guid>(type: "TEXT", nullable: false),
                    RoomNumber = table.Column<string>(type: "TEXT", maxLength: 5, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    RoomLocationUid = table.Column<Guid>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Uid);
                    table.ForeignKey(
                        name: "FK_Rooms_Rooms_RoomLocationUid",
                        column: x => x.RoomLocationUid,
                        principalTable: "Rooms",
                        principalColumn: "Uid");
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Uid = table.Column<Guid>(type: "TEXT", nullable: false),
                    PersonType = table.Column<int>(type: "INTEGER", nullable: false),
                    Id = table.Column<string>(type: "TEXT", nullable: true),
                    DisplayName = table.Column<string>(type: "TEXT", nullable: false),
                    GivenName = table.Column<string>(type: "TEXT", nullable: false),
                    Surname = table.Column<string>(type: "TEXT", nullable: false),
                    JobTitle = table.Column<string>(type: "TEXT", nullable: false),
                    Department = table.Column<string>(type: "TEXT", nullable: false),
                    EmployeeType = table.Column<string>(type: "TEXT", nullable: false),
                    UserPrincipalName = table.Column<string>(type: "TEXT", nullable: false),
                    MailNickname = table.Column<string>(type: "TEXT", nullable: false),
                    Mail = table.Column<string>(type: "TEXT", nullable: false),
                    LegalAgeGroupClassification = table.Column<string>(type: "TEXT", nullable: false),
                    City = table.Column<string>(type: "TEXT", nullable: false),
                    CompanyName = table.Column<string>(type: "TEXT", nullable: false),
                    AgeGroup = table.Column<string>(type: "TEXT", nullable: false),
                    OfficeLocation = table.Column<string>(type: "TEXT", nullable: false),
                    AccountEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    ConsentProvidedForMinor = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Uid);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_RoomLocationUid",
                table: "Rooms",
                column: "RoomLocationUid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Devices");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
