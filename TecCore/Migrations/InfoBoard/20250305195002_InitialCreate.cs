using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TecCore.Migrations.InfoBoard
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdminUsers",
                columns: table => new
                {
                    Uid = table.Column<Guid>(type: "TEXT", nullable: false),
                    Username = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminUsers", x => x.Uid);
                });

            migrationBuilder.CreateTable(
                name: "InfoBoards",
                columns: table => new
                {
                    Uid = table.Column<Guid>(type: "TEXT", nullable: false),
                    SortOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    IsArchived = table.Column<bool>(type: "INTEGER", nullable: false),
                    StartAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InfoBoards", x => x.Uid);
                });

            migrationBuilder.CreateTable(
                name: "InfoBoardItems",
                columns: table => new
                {
                    Uid = table.Column<Guid>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    StartAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ItemType = table.Column<string>(type: "TEXT", maxLength: 8, nullable: false),
                    InfoBoardUid = table.Column<Guid>(type: "TEXT", nullable: true),
                    Content = table.Column<string>(type: "TEXT", nullable: true),
                    ImageUrl = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    InfoBoardItemMarkdown_Content = table.Column<string>(type: "TEXT", nullable: true),
                    RssUrl = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    InfoBoardItemText_Content = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InfoBoardItems", x => x.Uid);
                    table.ForeignKey(
                        name: "FK_InfoBoardItems_InfoBoards_InfoBoardUid",
                        column: x => x.InfoBoardUid,
                        principalTable: "InfoBoards",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CarouselItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ImageUrl = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 75, nullable: false),
                    Content = table.Column<string>(type: "TEXT", nullable: false),
                    InfoBoardItemCarouselUid = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarouselItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CarouselItem_InfoBoardItems_InfoBoardItemCarouselUid",
                        column: x => x.InfoBoardItemCarouselUid,
                        principalTable: "InfoBoardItems",
                        principalColumn: "Uid");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdminUsers_Username",
                table: "AdminUsers",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CarouselItem_InfoBoardItemCarouselUid",
                table: "CarouselItem",
                column: "InfoBoardItemCarouselUid");

            migrationBuilder.CreateIndex(
                name: "IX_InfoBoardItems_InfoBoardUid",
                table: "InfoBoardItems",
                column: "InfoBoardUid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminUsers");

            migrationBuilder.DropTable(
                name: "CarouselItem");

            migrationBuilder.DropTable(
                name: "InfoBoardItems");

            migrationBuilder.DropTable(
                name: "InfoBoards");
        }
    }
}
