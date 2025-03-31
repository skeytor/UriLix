using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UriLix.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class DropAliasColumnOfShortenedUrlTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ShortenedUrl_Alias",
                table: "ShortenedUrl");

            migrationBuilder.DropIndex(
                name: "IX_ShortenedUrl_ShortCode",
                table: "ShortenedUrl");

            migrationBuilder.DropColumn(
                name: "Alias",
                table: "ShortenedUrl");

            migrationBuilder.AlterColumn<string>(
                name: "ShortCode",
                table: "ShortenedUrl",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(5)",
                oldMaxLength: 5,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShortenedUrl_ShortCode",
                table: "ShortenedUrl",
                column: "ShortCode",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ShortenedUrl_ShortCode",
                table: "ShortenedUrl");

            migrationBuilder.AlterColumn<string>(
                name: "ShortCode",
                table: "ShortenedUrl",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AddColumn<string>(
                name: "Alias",
                table: "ShortenedUrl",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShortenedUrl_Alias",
                table: "ShortenedUrl",
                column: "Alias",
                unique: true,
                filter: "[Alias] IS NOT NULL AND [Alias] <> ''");

            migrationBuilder.CreateIndex(
                name: "IX_ShortenedUrl_ShortCode",
                table: "ShortenedUrl",
                column: "ShortCode",
                unique: true,
                filter: "[ShortCode] IS NOT NULL AND [ShortCode] <> ''");
        }
    }
}
