using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RS1_2024_25.API.Migrations
{
    /// <inheritdoc />
    public partial class DataSeedFAQ : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "FAQs",
                columns: new[] { "FAQId", "Answer", "Question", "UserId" },
                values: new object[,]
                {
                    { 1, "Koristite našu pretragu po modelu vozila ili kontaktirajte podršku za pomoć.", "Kako da pronađem pravi dio za moje vozilo?", 2 },
                    { 2, "You can track your order using the tracking number sent to your email.", "How can I track my order?", 3 },
                    { 3, "Da, povrat novca je moguć unutar 30 dana uz dostavljen dokaz o kupovini.", "Da li nudite povrat novca za neispravne dijelove?", 2 },
                    { 4, "We accept card payments, cash on delivery, and bank transfers.", "What payment methods are available?", 3 },
                    { 5, "Dostava obično traje 3-5 radnih dana, u zavisnosti od lokacije.", "Koliko traje dostava?", 2 },
                    { 6, "Yes, you can return unused parts within 15 days of delivery.", "Can I return a part if it doesn't fit my vehicle?", 3 },
                    { 7, "Da, popusti su dostupni za narudžbe veće od 500 BAM. Kontaktirajte nas za detalje.", "Da li nudite popuste za veće narudžbe?", 2 },
                    { 8, "Please contact our support team immediately, and we will arrange for a replacement.", "What should I do if I receive the wrong part?", 3 },
                    { 9, "Nažalost, trenutno nudimo samo online naručivanje i dostavu.", "Da li je moguće preuzimanje dijelova u prodavnici?", 2 },
                    { 10, "Currently, we only ship within Bosnia and Herzegovina.", "Do you ship internationally?", 3 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "FAQs",
                keyColumn: "FAQId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "FAQs",
                keyColumn: "FAQId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "FAQs",
                keyColumn: "FAQId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "FAQs",
                keyColumn: "FAQId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "FAQs",
                keyColumn: "FAQId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "FAQs",
                keyColumn: "FAQId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "FAQs",
                keyColumn: "FAQId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "FAQs",
                keyColumn: "FAQId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "FAQs",
                keyColumn: "FAQId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "FAQs",
                keyColumn: "FAQId",
                keyValue: 10);
        }
    }
}
