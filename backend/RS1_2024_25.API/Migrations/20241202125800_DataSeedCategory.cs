using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RS1_2024_25.API.Migrations
{
    /// <inheritdoc />
    public partial class DataSeedCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "CategoryName" },
                values: new object[,]
                {
                    { 1, "Brakes" },
                    { 2, "Suspension" },
                    { 3, "Engine Parts" },
                    { 4, "Exhaust Systems" },
                    { 5, "Oil and Fluids" },
                    { 6, "Filters" },
                    { 7, "Electrical Components" },
                    { 8, "Cooling Systems" },
                    { 9, "Interior Accessories" },
                    { 10, "Tires and Wheels" }
                });

            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "ID",
                keyValue: 4,
                column: "Name",
                value: "Bosanska Krupa");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 10);

            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "ID",
                keyValue: 4,
                column: "Name",
                value: "Bosnaska Krupa");
        }
    }
}
