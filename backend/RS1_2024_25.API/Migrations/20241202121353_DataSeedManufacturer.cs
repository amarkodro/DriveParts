using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RS1_2024_25.API.Migrations
{
    /// <inheritdoc />
    public partial class DataSeedManufacturer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Manufacturers",
                columns: new[] { "ManufacturerId", "Address", "Contact", "Name" },
                values: new object[,]
                {
                    { 1, "Džemala Bijedića 185, Sarajevo, Bosna i Hercegovina", "+387 33 770 100", "Bosch" },
                    { 2, "Karađorđeva 120, Banja Luka, Bosna i Hercegovina", "+387 51 210 990", "Valeo" },
                    { 3, "Rudarska 33, Tuzla, Bosna i Hercegovina", "+387 35 320 870", "Delphi Technologies" },
                    { 4, "Bišće polje bb, Mostar, Bosna i Hercegovina", "+387 36 576 600", "Continental" },
                    { 5, "Industrijska zona bb, Zenica, Bosna i Hercegovina", "+387 32 450 110", "Magneti Marelli" },
                    { 6, "Pofalići bb, Sarajevo, Bosna i Hercegovina", "+387 33 210 320", "Brembo" },
                    { 7, "Aleja Svetog Save 15, Banja Luka, Bosna i Hercegovina", "+387 51 321 480", "TRW Automotive" },
                    { 8, "Zmaja od Bosne bb, Sarajevo, Bosna i Hercegovina", "+387 33 234 567", "ATE" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Manufacturers",
                keyColumn: "ManufacturerId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Manufacturers",
                keyColumn: "ManufacturerId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Manufacturers",
                keyColumn: "ManufacturerId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Manufacturers",
                keyColumn: "ManufacturerId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Manufacturers",
                keyColumn: "ManufacturerId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Manufacturers",
                keyColumn: "ManufacturerId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Manufacturers",
                keyColumn: "ManufacturerId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Manufacturers",
                keyColumn: "ManufacturerId",
                keyValue: 8);
        }
    }
}
