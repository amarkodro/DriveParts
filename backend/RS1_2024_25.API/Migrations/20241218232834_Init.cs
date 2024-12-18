using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RS1_2024_25.API.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cars",
                columns: table => new
                {
                    CarId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Brand = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cars", x => x.CarId);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Engines",
                columns: table => new
                {
                    EngineId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Power = table.Column<int>(type: "int", nullable: false),
                    Displacement = table.Column<double>(type: "float", nullable: false),
                    FuelType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Engines", x => x.EngineId);
                });

            migrationBuilder.CreateTable(
                name: "Genders",
                columns: table => new
                {
                    GenderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GenderName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genders", x => x.GenderId);
                });

            migrationBuilder.CreateTable(
                name: "Manufacturers",
                columns: table => new
                {
                    ManufacturerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Contact = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manufacturers", x => x.ManufacturerId);
                });

            migrationBuilder.CreateTable(
                name: "MyAppUsers",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: false),
                    IsManager = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MyAppUsers", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    PaymentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.PaymentId);
                });

            migrationBuilder.CreateTable(
                name: "Statuses",
                columns: table => new
                {
                    StatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statuses", x => x.StatusId);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    SupplierId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Contact = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.SupplierId);
                });

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CountryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Cities_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "CarEngines",
                columns: table => new
                {
                    CarEngineId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CarId = table.Column<int>(type: "int", nullable: false),
                    EngineId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarEngines", x => x.CarEngineId);
                    table.ForeignKey(
                        name: "FK_CarEngines_Cars_CarId",
                        column: x => x.CarId,
                        principalTable: "Cars",
                        principalColumn: "CarId");
                    table.ForeignKey(
                        name: "FK_CarEngines_Engines_EngineId",
                        column: x => x.EngineId,
                        principalTable: "Engines",
                        principalColumn: "EngineId");
                });

            migrationBuilder.CreateTable(
                name: "Parts",
                columns: table => new
                {
                    PartId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    ManufacturerId = table.Column<int>(type: "int", nullable: false),
                    PartImage = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parts", x => x.PartId);
                    table.ForeignKey(
                        name: "FK_Parts_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId");
                    table.ForeignKey(
                        name: "FK_Parts_Manufacturers_ManufacturerId",
                        column: x => x.ManufacturerId,
                        principalTable: "Manufacturers",
                        principalColumn: "ManufacturerId");
                });

            migrationBuilder.CreateTable(
                name: "MyAuthenticationTokens",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RecordedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MyAppUserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MyAuthenticationTokens", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MyAuthenticationTokens_MyAppUsers_MyAppUserId",
                        column: x => x.MyAppUserId,
                        principalTable: "MyAppUsers",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Journeys",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartCityId = table.Column<int>(type: "int", nullable: false),
                    EndCityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Journeys", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Journeys_Cities_EndCityId",
                        column: x => x.EndCityId,
                        principalTable: "Cities",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Journeys_Cities_StartCityId",
                        column: x => x.StartCityId,
                        principalTable: "Cities",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "UserAccounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: false),
                    isUser = table.Column<bool>(type: "bit", nullable: false),
                    is2FActive = table.Column<bool>(type: "bit", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    AdminLevel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GenderId = table.Column<int>(type: "int", nullable: true),
                    CityId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAccounts_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_UserAccounts_Genders_GenderId",
                        column: x => x.GenderId,
                        principalTable: "Genders",
                        principalColumn: "GenderId");
                });

            migrationBuilder.CreateTable(
                name: "CarParts",
                columns: table => new
                {
                    CarPartId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CarId = table.Column<int>(type: "int", nullable: false),
                    PartId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarParts", x => x.CarPartId);
                    table.ForeignKey(
                        name: "FK_CarParts_Cars_CarId",
                        column: x => x.CarId,
                        principalTable: "Cars",
                        principalColumn: "CarId");
                    table.ForeignKey(
                        name: "FK_CarParts_Parts_PartId",
                        column: x => x.PartId,
                        principalTable: "Parts",
                        principalColumn: "PartId");
                });

            migrationBuilder.CreateTable(
                name: "FAQs",
                columns: table => new
                {
                    FAQId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Question = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Answer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FAQs", x => x.FAQId);
                    table.ForeignKey(
                        name: "FK_FAQs_UserAccounts_UserId",
                        column: x => x.UserId,
                        principalTable: "UserAccounts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    PaymentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Orders_Payments_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "Payments",
                        principalColumn: "PaymentId");
                    table.ForeignKey(
                        name: "FK_Orders_Statuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Statuses",
                        principalColumn: "StatusId");
                    table.ForeignKey(
                        name: "FK_Orders_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "SupplierId");
                    table.ForeignKey(
                        name: "FK_Orders_UserAccounts_UserId",
                        column: x => x.UserId,
                        principalTable: "UserAccounts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    ReviewId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    PartId = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Picture = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.ReviewId);
                    table.ForeignKey(
                        name: "FK_Reviews_Parts_PartId",
                        column: x => x.PartId,
                        principalTable: "Parts",
                        principalColumn: "PartId");
                    table.ForeignKey(
                        name: "FK_Reviews_UserAccounts_UserId",
                        column: x => x.UserId,
                        principalTable: "UserAccounts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    OrderItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    PartId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.OrderItemId);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId");
                    table.ForeignKey(
                        name: "FK_OrderItems_Parts_PartId",
                        column: x => x.PartId,
                        principalTable: "Parts",
                        principalColumn: "PartId");
                });

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
                columns: new[] { "PaymentId", "PaymentMethod" },
                values: new object[,]
                {
                    { 1, "Card" },
                    { 2, "Cash" }
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
                values: new object[,]
                {
                    { 1, "Masline-Kocine bb", "Moderator", "Admin", "amar.kodro@edu.fit.ba", true, "Amar", "$2a$11$T3GVQBvgyTtZ6PFA5...", "0623331233", "Kodro", "amar.kodro", true, false },
                    { 4, "Masline-Kocine bb", "Moderator", "Admin", "ammar.puce@edu.fit.ba", true, "Ammar", "$2a$11$T3GVQBvgyTtZ6PFA5...", "0623331233", "Puce", "ammar.puce", true, false }
                });

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
                columns: new[] { "PartId", "CategoryId", "Description", "ManufacturerId", "Name", "PartImage", "Price" },
                values: new object[,]
                {
                    { 1, 1, "High-quality brake pads for Golf", 1, "Brake Pads", null, 85.0 },
                    { 2, 2, "Durable suspension springs for Q5", 2, "Suspension Springs", null, 220.0 },
                    { 3, 6, "Oil filter suitable for Clio engines", 3, "Engine Oil Filter", null, 25.0 },
                    { 4, 4, "Rust-resistant exhaust pipe for 508", 4, "Exhaust Pipe", null, 300.0 },
                    { 5, 8, "High-performance coolant hose for Panda", 5, "Coolant Hose", null, 40.0 },
                    { 6, 6, "Improves air intake efficiency for Golf", 6, "Air Filter", null, 35.0 },
                    { 7, 2, "Enhanced suspension system for Q5", 7, "Shock Absorber", null, 250.0 },
                    { 8, 7, "High-performance spark plugs for Clio", 8, "Spark Plugs", null, 50.0 },
                    { 9, 4, "Eco-friendly exhaust component for 508", 1, "Catalytic Converter", null, 450.0 },
                    { 10, 5, "Premium oil for Panda engines", 2, "Engine Oil", null, 90.0 },
                    { 11, 1, "Front brake discs for Golf", 3, "Brake Discs", null, 150.0 },
                    { 12, 2, "Steering system component for Q5", 4, "Tie Rod Ends", null, 180.0 },
                    { 13, 3, "Efficient fuel pump for Clio", 5, "Fuel Pump", null, 220.0 },
                    { 14, 3, "Boost engine power for 508", 6, "Turbocharger", null, 1000.0 },
                    { 15, 9, "All-weather wipers for Panda", 7, "Windshield Wipers", null, 25.0 },
                    { 16, 8, "High-efficiency radiator for Golf", 8, "Radiator", null, 300.0 },
                    { 17, 2, "Stable suspension arms for Q5", 1, "Control Arms", null, 350.0 },
                    { 18, 7, "Reliable ignition coil for Clio", 2, "Ignition Coil", null, 75.0 },
                    { 19, 9, "Bright and durable headlights for 508", 3, "Headlight Assembly", null, 400.0 },
                    { 20, 7, "Long-lasting battery for Panda", 4, "Battery", null, 180.0 }
                });

            migrationBuilder.InsertData(
                table: "CarParts",
                columns: new[] { "CarPartId", "CarId", "PartId" },
                values: new object[,]
                {
                    { 1, 1, 1 },
                    { 2, 2, 2 },
                    { 3, 3, 3 },
                    { 4, 4, 4 },
                    { 5, 5, 5 }
                });

            migrationBuilder.InsertData(
                table: "UserAccounts",
                columns: new[] { "Id", "Address", "CityId", "Discriminator", "Email", "GenderId", "IsAdmin", "Name", "Password", "PhoneNumber", "Surname", "Username", "is2FActive", "isUser" },
                values: new object[,]
                {
                    { 2, "useraddress", 18, "User", "testuser@example.com", 1, false, "Test", "$2a$11$Aq1jWbAgjeHkWkiAk...", "0602213312", "User", "TestUser1", false, true },
                    { 3, "useraddress2", 16, "User", "testuser2@example.com", 2, false, "Test2", "$2a$11$Aq1jWbAgjeHkWkiAk...", "0602234312", "User2", "TestUser2", false, true }
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

            migrationBuilder.CreateIndex(
                name: "IX_CarEngines_CarId",
                table: "CarEngines",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_CarEngines_EngineId",
                table: "CarEngines",
                column: "EngineId");

            migrationBuilder.CreateIndex(
                name: "IX_CarParts_CarId",
                table: "CarParts",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_CarParts_PartId",
                table: "CarParts",
                column: "PartId");

            migrationBuilder.CreateIndex(
                name: "IX_Cities_CountryId",
                table: "Cities",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_FAQs_UserId",
                table: "FAQs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Journeys_EndCityId",
                table: "Journeys",
                column: "EndCityId");

            migrationBuilder.CreateIndex(
                name: "IX_Journeys_StartCityId",
                table: "Journeys",
                column: "StartCityId");

            migrationBuilder.CreateIndex(
                name: "IX_MyAuthenticationTokens_MyAppUserId",
                table: "MyAuthenticationTokens",
                column: "MyAppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_PartId",
                table: "OrderItems",
                column: "PartId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_PaymentId",
                table: "Orders",
                column: "PaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_StatusId",
                table: "Orders",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_SupplierId",
                table: "Orders",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Parts_CategoryId",
                table: "Parts",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Parts_ManufacturerId",
                table: "Parts",
                column: "ManufacturerId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_PartId",
                table: "Reviews",
                column: "PartId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_UserId",
                table: "Reviews",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAccounts_CityId",
                table: "UserAccounts",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAccounts_GenderId",
                table: "UserAccounts",
                column: "GenderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CarEngines");

            migrationBuilder.DropTable(
                name: "CarParts");

            migrationBuilder.DropTable(
                name: "FAQs");

            migrationBuilder.DropTable(
                name: "Journeys");

            migrationBuilder.DropTable(
                name: "MyAuthenticationTokens");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "Engines");

            migrationBuilder.DropTable(
                name: "Cars");

            migrationBuilder.DropTable(
                name: "MyAppUsers");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Parts");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "Statuses");

            migrationBuilder.DropTable(
                name: "Suppliers");

            migrationBuilder.DropTable(
                name: "UserAccounts");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Manufacturers");

            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropTable(
                name: "Genders");

            migrationBuilder.DropTable(
                name: "Countries");
        }
    }
}
