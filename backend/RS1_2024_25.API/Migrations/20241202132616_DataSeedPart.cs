using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RS1_2024_25.API.Migrations
{
    /// <inheritdoc />
    public partial class DataSeedPart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Parts",
                columns: new[] { "PartId", "CarId", "CategoryId", "Description", "ManufacturerId", "Name", "Price" },
                values: new object[,]
                {
                    { 1, 1, 1, "High-quality brake pads for Golf", 1, "Brake Pads", 0.0 },
                    { 2, 2, 2, "Durable suspension springs for Q5", 2, "Suspension Springs", 0.0 },
                    { 3, 3, 6, "Oil filter suitable for Clio engines", 3, "Engine Oil Filter", 0.0 },
                    { 4, 4, 4, "Rust-resistant exhaust pipe for 508", 4, "Exhaust Pipe", 0.0 },
                    { 5, 5, 8, "High-performance coolant hose for Panda", 5, "Coolant Hose", 0.0 },
                    { 6, 1, 6, "Improves air intake efficiency for Golf", 6, "Air Filter", 0.0 },
                    { 7, 2, 2, "Enhanced suspension system for Q5", 7, "Shock Absorber", 0.0 },
                    { 8, 3, 7, "High-performance spark plugs for Clio", 8, "Spark Plugs", 0.0 },
                    { 9, 4, 4, "Eco-friendly exhaust component for 508", 1, "Catalytic Converter", 0.0 },
                    { 10, 5, 5, "Premium oil for Panda engines", 2, "Engine Oil", 0.0 },
                    { 11, 1, 1, "Front brake discs for Golf", 3, "Brake Discs", 0.0 },
                    { 12, 2, 2, "Steering system component for Q5", 4, "Tie Rod Ends", 0.0 },
                    { 13, 3, 3, "Efficient fuel pump for Clio", 5, "Fuel Pump", 0.0 },
                    { 14, 4, 3, "Boost engine power for 508", 6, "Turbocharger", 0.0 },
                    { 15, 5, 9, "All-weather wipers for Panda", 7, "Windshield Wipers", 0.0 },
                    { 16, 1, 8, "High-efficiency radiator for Golf", 8, "Radiator", 0.0 },
                    { 17, 2, 2, "Stable suspension arms for Q5", 1, "Control Arms", 0.0 },
                    { 18, 3, 7, "Reliable ignition coil for Clio", 2, "Ignition Coil", 0.0 },
                    { 19, 4, 9, "Bright and durable headlights for 508", 3, "Headlight Assembly", 0.0 },
                    { 20, 5, 7, "Long-lasting battery for Panda", 4, "Battery", 0.0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Parts",
                keyColumn: "PartId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Parts",
                keyColumn: "PartId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Parts",
                keyColumn: "PartId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Parts",
                keyColumn: "PartId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Parts",
                keyColumn: "PartId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Parts",
                keyColumn: "PartId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Parts",
                keyColumn: "PartId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Parts",
                keyColumn: "PartId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Parts",
                keyColumn: "PartId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Parts",
                keyColumn: "PartId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Parts",
                keyColumn: "PartId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Parts",
                keyColumn: "PartId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Parts",
                keyColumn: "PartId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Parts",
                keyColumn: "PartId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Parts",
                keyColumn: "PartId",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Parts",
                keyColumn: "PartId",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Parts",
                keyColumn: "PartId",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Parts",
                keyColumn: "PartId",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Parts",
                keyColumn: "PartId",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Parts",
                keyColumn: "PartId",
                keyValue: 20);
        }
    }
}
