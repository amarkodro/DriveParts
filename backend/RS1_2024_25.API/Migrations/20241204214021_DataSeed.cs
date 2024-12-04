using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RS1_2024_25.API.Migrations
{
    /// <inheritdoc />
    public partial class DataSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Cars",
                columns: new[] { "CarId", "Brand", "Model", "Type", "Year" },
                values: new object[,]
                {
                    { 1, "Volkswagen", "Golf", "Hatchback", 2022 },
                    { 2, "Audi", "Q5", "SUV", 2021 },
                    { 3, "Renault", "Clio", "Hatchback", 2023 },
                    { 4, "Peugeot", "508", "Sedan", 2020 },
                    { 5, "Fiat", "Panda", "Compact", 2019 }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "Name" },
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

            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "ID", "Name" },
                values: new object[] { 1, "Bosna i Hercegovina" });

            migrationBuilder.InsertData(
                table: "Engines",
                columns: new[] { "EngineId", "Displacement", "FuelType", "Name", "Power" },
                values: new object[,]
                {
                    { 1, 2000.0, "Diesel", "TDI 2.0", 150 },
                    { 2, 1500.0, "Petrol", "TSI 1.5", 130 },
                    { 3, 0.0, "Electric", "Electric R100", 100 },
                    { 4, 1600.0, "Hybrid", "HY 1.6", 180 },
                    { 5, 1200.0, "Petrol", "FIAT Petrol 1.2", 90 }
                });

            migrationBuilder.InsertData(
                table: "Genders",
                columns: new[] { "GenderId", "GenderName" },
                values: new object[,]
                {
                    { 1, "Male" },
                    { 2, "Female" }
                });

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

            migrationBuilder.InsertData(
                table: "Payments",
                columns: new[] { "PaymentId", "IsCard", "IsCash" },
                values: new object[,]
                {
                    { 1, true, false },
                    { 2, false, true }
                });

            migrationBuilder.InsertData(
                table: "Statuses",
                columns: new[] { "StatusId", "Name" },
                values: new object[,]
                {
                    { 1, "Pending" },
                    { 2, "Approved" },
                    { 3, "Rejected" },
                    { 4, "In Progress" },
                    { 5, "Completed" },
                    { 6, "Cancelled" },
                    { 7, "On Hold" },
                    { 8, "Failed" },
                    { 9, "Draft" },
                    { 10, "Submitted" }
                });

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

            migrationBuilder.InsertData(
                table: "UserAccounts",
                columns: new[] { "Id", "Address", "AdminLevel", "Discriminator", "Email", "IsAdmin", "Name", "Password", "PhoneNumber", "Surname", "Username", "is2FActive", "isUser" },
                values: new object[] { 1, "Masline-Kocine bb", "Moderator", "Admin", "amar.kodro@edu.fit.ba", true, "Amar", "driveparts", "0623331233", "Kodro", "amar.kodro", true, false });

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

            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "ID", "CountryId", "Name" },
                values: new object[,]
                {
                    { 1, 1, "Banja Luka" },
                    { 2, 1, "Bihać" },
                    { 3, 1, "Bijeljina" },
                    { 4, 1, "Bosanska Krupa" },
                    { 5, 1, "Cazin" },
                    { 6, 1, "Čapljina" },
                    { 7, 1, "Drventa" },
                    { 8, 1, "Doboj" },
                    { 9, 1, "Goražde" },
                    { 10, 1, "Gračanica" },
                    { 11, 1, "Cityačac" },
                    { 12, 1, "Cityiška" },
                    { 13, 1, "Konjic" },
                    { 14, 1, "Laktaši" },
                    { 15, 1, "Livno" },
                    { 16, 1, "Lukavac" },
                    { 17, 1, "Ljubuški" },
                    { 18, 1, "Mostar" },
                    { 19, 1, "Orašje" },
                    { 20, 1, "Prijedor" },
                    { 21, 1, "Prnjavor" },
                    { 22, 1, "Sarajevo" },
                    { 23, 1, "Srebrenik" },
                    { 24, 1, "Stolac" },
                    { 25, 1, "Široki Brijeg" },
                    { 26, 1, "Travnik" },
                    { 27, 1, "Tuzla" },
                    { 28, 1, "Visoko" },
                    { 29, 1, "Zavidovići" },
                    { 30, 1, "Zenica" },
                    { 31, 1, "Zvornik" },
                    { 32, 1, "Živinice" },
                    { 33, 1, "Donji Vakuf" },
                    { 34, 1, "Zavidovići" }
                });

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

            migrationBuilder.InsertData(
                table: "UserAccounts",
                columns: new[] { "Id", "Address", "CityId", "Discriminator", "Email", "GenderId", "IsAdmin", "Name", "Password", "PhoneNumber", "Surname", "Username", "is2FActive", "isUser" },
                values: new object[,]
                {
                    { 2, "useraddress", 18, "User", "testuser@example.com", 1, false, "Test", "testuser123", "0602213312", "User", "TestUser1", false, true },
                    { 3, "useraddress2", 16, "User", "testuser2@example.com", 2, false, "Test2", "testuser2", "0602234312", "User2", "TestUser2", false, true }
                });

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

            migrationBuilder.InsertData(
                table: "OrderItems",
                columns: new[] { "OrderItemId", "OrderId", "PartId", "Price", "Quantity" },
                values: new object[,]
                {
                    { 1, 1, 1, 45.0, 1 },
                    { 2, 1, 2, 90.0, 2 },
                    { 3, 2, 3, 15.0, 1 },
                    { 4, 2, 4, 70.0, 1 },
                    { 5, 3, 5, 25.0, 1 },
                    { 6, 3, 6, 20.0, 1 },
                    { 7, 4, 7, 50.0, 2 },
                    { 8, 4, 8, 40.0, 4 },
                    { 9, 5, 9, 150.0, 1 },
                    { 10, 5, 10, 30.0, 1 },
                    { 11, 6, 11, 100.0, 2 },
                    { 12, 6, 12, 35.0, 1 },
                    { 13, 7, 13, 200.0, 1 },
                    { 14, 8, 14, 250.0, 1 },
                    { 15, 9, 15, 20.0, 1 }
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

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "ID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "ID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "ID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "ID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "ID",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "ID",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "ID",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "ID",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "ID",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "ID",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "ID",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "ID",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "ID",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "ID",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "ID",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "ID",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "ID",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "ID",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "ID",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "ID",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "ID",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "ID",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "ID",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "ID",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "ID",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "ID",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "ID",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "ID",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "ID",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "ID",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "ID",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "ID",
                keyValue: 34);

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

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumn: "OrderItemId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumn: "OrderItemId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumn: "OrderItemId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumn: "OrderItemId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumn: "OrderItemId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumn: "OrderItemId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumn: "OrderItemId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumn: "OrderItemId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumn: "OrderItemId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumn: "OrderItemId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumn: "OrderItemId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumn: "OrderItemId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumn: "OrderItemId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumn: "OrderItemId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumn: "OrderItemId",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: 10);

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

            migrationBuilder.DeleteData(
                table: "UserAccounts",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Engines",
                keyColumn: "EngineId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Engines",
                keyColumn: "EngineId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Engines",
                keyColumn: "EngineId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Engines",
                keyColumn: "EngineId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Engines",
                keyColumn: "EngineId",
                keyValue: 5);

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
                table: "Statuses",
                keyColumn: "StatusId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Suppliers",
                keyColumn: "SupplierId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "CarId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "CarId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "CarId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "CarId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "CarId",
                keyValue: 5);

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

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Statuses",
                keyColumn: "StatusId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Statuses",
                keyColumn: "StatusId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Statuses",
                keyColumn: "StatusId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Statuses",
                keyColumn: "StatusId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Statuses",
                keyColumn: "StatusId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Statuses",
                keyColumn: "StatusId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Statuses",
                keyColumn: "StatusId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Statuses",
                keyColumn: "StatusId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Statuses",
                keyColumn: "StatusId",
                keyValue: 9);

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

            migrationBuilder.DeleteData(
                table: "UserAccounts",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "UserAccounts",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "ID",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "ID",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Genders",
                keyColumn: "GenderId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Genders",
                keyColumn: "GenderId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "ID",
                keyValue: 1);
        }
    }
}
