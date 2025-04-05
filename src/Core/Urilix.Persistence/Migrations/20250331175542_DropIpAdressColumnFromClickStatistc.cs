using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UriLix.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class DropIpAdressColumnFromClickStatistc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IpAddress",
                table: "ClickStatistic");

            migrationBuilder.DropColumn(
                name: "Referrer",
                table: "ClickStatistic");

            migrationBuilder.AlterColumn<string>(
                name: "UserAgent",
                table: "ClickStatistic",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "Device",
                table: "ClickStatistic",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<string>(
                name: "Referer",
                table: "ClickStatistic",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Referer",
                table: "ClickStatistic");

            migrationBuilder.AlterColumn<string>(
                name: "UserAgent",
                table: "ClickStatistic",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Device",
                table: "ClickStatistic",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(25)",
                oldMaxLength: 25);

            migrationBuilder.AddColumn<string>(
                name: "IpAddress",
                table: "ClickStatistic",
                type: "nvarchar(11)",
                maxLength: 11,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Referrer",
                table: "ClickStatistic",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }
    }
}
