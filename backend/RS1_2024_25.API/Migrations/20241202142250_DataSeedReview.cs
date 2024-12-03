using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RS1_2024_25.API.Migrations
{
    /// <inheritdoc />
    public partial class DataSeedReview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Reviews",
                columns: new[] { "ReviewId", "Date", "PartId", "Picture", "Text", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 1, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, null, "Odličan kvalitet kočnica, stigle brzo i lako ih je bilo ugraditi.", 2 },
                    { 2, new DateTime(2023, 2, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, null, "Ovjes je perfektan, poboljšana stabilnost auta. Preporučujem!", 3 },
                    { 3, new DateTime(2023, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, null, "Filter ulja je odličan, jednostavan za instalaciju i povoljan.", 2 },
                    { 4, new DateTime(2023, 4, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, null, "Cijev za auspuh savršeno odgovara. Bez problema je montirana.", 3 },
                    { 5, new DateTime(2023, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, null, "Crijevo rashladnog sistema odlično obavlja posao. Dostava na vrijeme.", 2 },
                    { 6, new DateTime(2023, 6, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, null, "Zračni filter je povećao efikasnost motora. Zadovoljan kupovinom.", 3 },
                    { 7, new DateTime(2023, 7, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 7, null, "Amortizeri su vrhunski. Auto je sada puno stabilniji.", 2 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: 7);
        }
    }
}
