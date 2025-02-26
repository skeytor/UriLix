using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UriLix.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastLoginAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShortenedUrl",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OriginalUrl = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ShortCode = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    Alias = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShortenedUrl", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShortenedUrl_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClickStatistic",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShortenedUrlId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    Device = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Country = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Browser = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    UserAgent = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Referrer = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    VisitedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClickStatistic", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClickStatistic_ShortenedUrl_ShortenedUrlId",
                        column: x => x.ShortenedUrlId,
                        principalTable: "ShortenedUrl",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClickStatistic_Id",
                table: "ClickStatistic",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ClickStatistic_ShortenedUrlId",
                table: "ClickStatistic",
                column: "ShortenedUrlId");

            migrationBuilder.CreateIndex(
                name: "IX_ShortenedUrl_Alias",
                table: "ShortenedUrl",
                column: "Alias",
                unique: true,
                filter: "[Alias] IS NOT NULL AND [Alias] <> ''");

            migrationBuilder.CreateIndex(
                name: "IX_ShortenedUrl_Id",
                table: "ShortenedUrl",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ShortenedUrl_ShortCode",
                table: "ShortenedUrl",
                column: "ShortCode",
                unique: true,
                filter: "[ShortCode] IS NOT NULL AND [ShortCode] <> ''");

            migrationBuilder.CreateIndex(
                name: "IX_ShortenedUrl_UserId",
                table: "ShortenedUrl",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Id",
                table: "Users",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClickStatistic");

            migrationBuilder.DropTable(
                name: "ShortenedUrl");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
