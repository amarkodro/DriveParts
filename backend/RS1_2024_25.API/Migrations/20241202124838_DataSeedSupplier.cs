using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RS1_2024_25.API.Migrations
{
    /// <inheritdoc />
    public partial class DataSeedSupplier : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Suppliers",
                columns: new[] { "SupplierId", "Address", "Contact", "Name" },
                values: new object[,]
                {
                    { 1, "Maršala Tita 45, Sarajevo, BiH", "+387 33 123 456", "A2B Delivery" },
                    { 2, "Kralja Petra I Karađorđevića 102, Banja Luka, BiH", "+387 51 789 123", "EuroExpress" },
                    { 3, "Hamdije Kreševljakovića 50, Mostar, BiH", "+387 61 987 654", "BH Brza Pošta" },
                    { 4, "Zagrebačka 10, Tuzla, BiH", "+387 36 456 789", "Sky Express" },
                    { 5, "Bosanska 25, Zenica, BiH", "+387 32 555 888", "Express One" },
                    { 6, "Srebrenička 7, Brčko, BiH", "+387 35 222 333", "FastTrack Logistics" },
                    { 7, "Zmaja od Bosne 12, Sarajevo, BiH", "+387 33 445 666", "DHL Bosnia" },
                    { 8, "Goranska 15, Bihać, BiH", "+387 37 777 888", "GLS BiH" },
                    { 9, "Savska 4, Bijeljina, BiH", "+387 66 123 321", "UPS Delivery" },
                    { 10, "Prijedorska 22, Prijedor, BiH", "+387 65 999 000", "PostExpress BiH" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Suppliers",
                keyColumn: "SupplierId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Suppliers",
                keyColumn: "SupplierId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Suppliers",
                keyColumn: "SupplierId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Suppliers",
                keyColumn: "SupplierId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Suppliers",
                keyColumn: "SupplierId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Suppliers",
                keyColumn: "SupplierId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Suppliers",
                keyColumn: "SupplierId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Suppliers",
                keyColumn: "SupplierId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Suppliers",
                keyColumn: "SupplierId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Suppliers",
                keyColumn: "SupplierId",
                keyValue: 10);
        }
    }
}
