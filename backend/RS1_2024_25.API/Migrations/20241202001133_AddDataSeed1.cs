using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RS1_2024_25.API.Migrations
{
    /// <inheritdoc />
    public partial class AddDataSeed1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "CarEngines",
                columns: new[] { "CarEngineId", "CarId", "EngineId" },
                values: new object[,]
                {
                    { 1, 1, 1 },
                    { 2, 2, 2 },
                    { 3, 3, 3 },
                    { 4, 4, 4 },
                    { 5, 5, 5 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CarEngines",
                keyColumn: "CarEngineId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "CarEngines",
                keyColumn: "CarEngineId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "CarEngines",
                keyColumn: "CarEngineId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "CarEngines",
                keyColumn: "CarEngineId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "CarEngines",
                keyColumn: "CarEngineId",
                keyValue: 5);
        }
    }
}
