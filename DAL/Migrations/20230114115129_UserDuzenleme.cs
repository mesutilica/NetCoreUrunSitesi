using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class UserDuzenleme : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "RefreshTokenExpireDate",
                table: "AppUsers",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "RefreshToken",
                table: "AppUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<Guid>(
                name: "UserGuid",
                table: "AppUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreateDate", "RefreshToken", "RefreshTokenExpireDate" },
                values: new object[] { new DateTime(2023, 1, 14, 14, 51, 28, 643, DateTimeKind.Local).AddTicks(6543), "f5ea4739-16d9-42c2-97d2-a987f8e461d6", new DateTime(2023, 1, 14, 15, 21, 28, 643, DateTimeKind.Local).AddTicks(6624) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserGuid",
                table: "AppUsers");

            migrationBuilder.AlterColumn<DateTime>(
                name: "RefreshTokenExpireDate",
                table: "AppUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RefreshToken",
                table: "AppUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreateDate", "RefreshToken", "RefreshTokenExpireDate" },
                values: new object[] { new DateTime(2022, 9, 11, 17, 53, 58, 419, DateTimeKind.Local).AddTicks(8100), "", new DateTime(2022, 9, 11, 17, 53, 58, 419, DateTimeKind.Local).AddTicks(8116) });
        }
    }
}
