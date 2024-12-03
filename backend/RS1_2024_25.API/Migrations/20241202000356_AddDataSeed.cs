using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RS1_2024_25.API.Migrations
{
    /// <inheritdoc />
    public partial class AddDataSeed : Migration
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
                table: "UserAccounts",
                columns: new[] { "Id", "Address", "AdminLevel", "Discriminator", "Email", "IsAdmin", "Name", "Password", "PhoneNumber", "Surname", "Username", "is2FActive", "isUser" },
                values: new object[] { 1, "Masline-Kocine bb", "Moderator", "Admin", "amar.kodro@edu.fit.ba", true, "Amar", "driveparts", "0623331233", "Kodro", "amar.kodro", true, false });

            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "ID", "CountryId", "Name" },
                values: new object[,]
                {
                    { 1, 1, "Banja Luka" },
                    { 2, 1, "Bihać" },
                    { 3, 1, "Bijeljina" },
                    { 4, 1, "Bosnaska Krupa" },
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
                table: "UserAccounts",
                columns: new[] { "Id", "Address", "CityId", "Discriminator", "Email", "GenderId", "IsAdmin", "Name", "Password", "PhoneNumber", "Surname", "Username", "is2FActive", "isUser" },
                values: new object[,]
                {
                    { 2, "useraddress", 18, "User", "testuser@example.com", 1, false, "Test", "testuser123", "0602213312", "User", "TestUser1", false, true },
                    { 3, "useraddress2", 16, "User", "testuser2@example.com", 2, false, "Test2", "testuser2", "0602234312", "User2", "TestUser2", false, true }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                table: "UserAccounts",
                keyColumn: "Id",
                keyValue: 1);

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
