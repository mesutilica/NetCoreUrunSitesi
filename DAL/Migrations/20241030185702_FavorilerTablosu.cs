using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class FavorilerTablosu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Favorites",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: true),
                    AppUserId = table.Column<int>(type: "int", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Favorites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Favorites_AppUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Favorites_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id");
                });

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreateDate", "RefreshToken", "RefreshTokenExpireDate", "UserGuid" },
                values: new object[] { new DateTime(2024, 10, 30, 21, 57, 2, 180, DateTimeKind.Local).AddTicks(4188), "da18d0f7-0e78-4656-b306-99d5b6f953f4", new DateTime(2024, 10, 30, 22, 27, 2, 180, DateTimeKind.Local).AddTicks(4262), new Guid("e016db41-deee-4cd7-831e-c186c2d034b2") });


            migrationBuilder.CreateIndex(
                name: "IX_Favorites_AppUserId",
                table: "Favorites",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_ProductId",
                table: "Favorites",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Favorites");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreateDate", "RefreshToken", "RefreshTokenExpireDate", "UserGuid" },
                values: new object[] { new DateTime(2023, 6, 15, 21, 48, 32, 282, DateTimeKind.Local).AddTicks(3167), "18ff1a5a-a497-43ae-9018-eafaa4d35d44", new DateTime(2023, 6, 15, 22, 18, 32, 282, DateTimeKind.Local).AddTicks(3198), new Guid("2b674007-375a-4f49-b1d5-1e587609135a") });
        }
    }
}
