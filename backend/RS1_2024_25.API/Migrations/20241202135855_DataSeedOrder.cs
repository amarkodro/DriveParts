using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RS1_2024_25.API.Migrations
{
    /// <inheritdoc />
    public partial class DataSeedOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "OrderId", "Date", "PaymentId", "StatusId", "SupplierId", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, 1, 2 },
                    { 2, new DateTime(2024, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 2, 2, 3 },
                    { 3, new DateTime(2024, 12, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 3, 4, 2 },
                    { 4, new DateTime(2024, 12, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 4, 5, 3 },
                    { 5, new DateTime(2024, 12, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 5, 6, 2 },
                    { 6, new DateTime(2024, 12, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 6, 7, 3 },
                    { 7, new DateTime(2024, 12, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 7, 8, 2 },
                    { 8, new DateTime(2024, 12, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 8, 9, 3 },
                    { 9, new DateTime(2024, 12, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 9, 10, 2 },
                    { 10, new DateTime(2024, 12, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 10, 3, 3 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: 10);
        }
    }
}
