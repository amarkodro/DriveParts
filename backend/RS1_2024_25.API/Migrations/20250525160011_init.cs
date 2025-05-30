using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RS1_2024_25.API.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
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
                    Brand = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
                name: "Dashboards",
                columns: table => new
                {
                    DashboardId = table.Column<int>(type: "int", nullable: false),
                    TotalOrders = table.Column<int>(type: "int", nullable: false),
                    PendingOrders = table.Column<int>(type: "int", nullable: false),
                    CompletedOrders = table.Column<int>(type: "int", nullable: false),
                    FailedOrders = table.Column<int>(type: "int", nullable: false),
                    ApprovedOrders = table.Column<int>(type: "int", nullable: false),
                    RejectedOrders = table.Column<int>(type: "int", nullable: false),
                    InProgressOrders = table.Column<int>(type: "int", nullable: false),
                    CancelledOrders = table.Column<int>(type: "int", nullable: false),
                    OnHoldOrders = table.Column<int>(type: "int", nullable: false),
                    DraftOrders = table.Column<int>(type: "int", nullable: false),
                    SubmittedOrders = table.Column<int>(type: "int", nullable: false),
                    TotalCustomers = table.Column<int>(type: "int", nullable: false),
                    TotalSales = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
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
                name: "PromoCodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Discount = table.Column<float>(type: "real", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromoCodes", x => x.Id);
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
                name: "Types",
                columns: table => new
                {
                    TypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Types", x => x.TypeId);
                });

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CountryId = table.Column<int>(type: "int", nullable: false),
                    PostalCode = table.Column<long>(type: "bigint", nullable: true)
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
                name: "Models",
                columns: table => new
                {
                    ModelId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    CarId = table.Column<int>(type: "int", nullable: false),
                    EngineId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Models", x => x.ModelId);
                    table.ForeignKey(
                        name: "FK_Models_Cars_CarId",
                        column: x => x.CarId,
                        principalTable: "Cars",
                        principalColumn: "CarId");
                    table.ForeignKey(
                        name: "FK_Models_Engines_EngineId",
                        column: x => x.EngineId,
                        principalTable: "Engines",
                        principalColumn: "EngineId");
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
                    PartImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsFeatured = table.Column<bool>(type: "bit", nullable: false),
                    IsOnSale = table.Column<bool>(type: "bit", nullable: false),
                    IsNewArrival = table.Column<bool>(type: "bit", nullable: false),
                    TypeId = table.Column<int>(type: "int", nullable: true)
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
                    table.ForeignKey(
                        name: "FK_Parts_Types_TypeId",
                        column: x => x.TypeId,
                        principalTable: "Types",
                        principalColumn: "TypeId");
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
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: false),
                    isUser = table.Column<bool>(type: "bit", nullable: false),
                    is2FActive = table.Column<bool>(type: "bit", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isDeleted = table.Column<bool>(type: "bit", nullable: true),
                    CityId = table.Column<int>(type: "int", nullable: true),
                    Discriminator = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    AdminLevel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GenderId = table.Column<int>(type: "int", nullable: true)
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
                name: "ModelParts",
                columns: table => new
                {
                    ModelPartId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModelId = table.Column<int>(type: "int", nullable: false),
                    PartId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModelParts", x => x.ModelPartId);
                    table.ForeignKey(
                        name: "FK_ModelParts_Models_ModelId",
                        column: x => x.ModelId,
                        principalTable: "Models",
                        principalColumn: "ModelId");
                    table.ForeignKey(
                        name: "FK_ModelParts_Parts_PartId",
                        column: x => x.PartId,
                        principalTable: "Parts",
                        principalColumn: "PartId");
                });

            migrationBuilder.CreateTable(
                name: "PartEngines",
                columns: table => new
                {
                    PartEngineId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PartId = table.Column<int>(type: "int", nullable: false),
                    EngineId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartEngines", x => x.PartEngineId);
                    table.ForeignKey(
                        name: "FK_PartEngines_Engines_EngineId",
                        column: x => x.EngineId,
                        principalTable: "Engines",
                        principalColumn: "EngineId");
                    table.ForeignKey(
                        name: "FK_PartEngines_Parts_PartId",
                        column: x => x.PartId,
                        principalTable: "Parts",
                        principalColumn: "PartId");
                });

            migrationBuilder.CreateTable(
                name: "CartItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    PartId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartItems_Parts_PartId",
                        column: x => x.PartId,
                        principalTable: "Parts",
                        principalColumn: "PartId");
                    table.ForeignKey(
                        name: "FK_CartItems_UserAccounts_UserId",
                        column: x => x.UserId,
                        principalTable: "UserAccounts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FAQs",
                columns: table => new
                {
                    FAQId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Question = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Answer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true)
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
                    PaymentId = table.Column<int>(type: "int", nullable: false),
                    PromoCodeId = table.Column<int>(type: "int", nullable: true),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
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
                        name: "FK_Orders_PromoCodes_PromoCodeId",
                        column: x => x.PromoCodeId,
                        principalTable: "PromoCodes",
                        principalColumn: "Id");
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
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Expires = table.Column<DateTime>(type: "datetime2", nullable: false),
                    isRevoked = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserAccountId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_UserAccounts_UserAccountId",
                        column: x => x.UserAccountId,
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
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
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
                columns: new[] { "CarId", "Brand" },
                values: new object[,]
                {
                    { 1, "Volkswagen" },
                    { 2, "Audi" },
                    { 3, "Renault" },
                    { 4, "Peugeot" },
                    { 5, "Fiat" },
                    { 6, "BMW" },
                    { 7, "Mercedes" },
                    { 8, "Tesla" },
                    { 9, "Ford" },
                    { 10, "Toyota" },
                    { 11, "Honda" },
                    { 12, "Nissan" },
                    { 13, "Mazda" },
                    { 14, "Kia" },
                    { 15, "Hyundai" },
                    { 16, "Chevrolet" },
                    { 17, "Subaru" },
                    { 18, "Jeep" },
                    { 19, "Volvo" }
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
                    { 10, "Tires and Wheels" },
                    { 11, "Body Parts and Exterior" },
                    { 12, "Lighting Systems" },
                    { 13, "Transmission Parts" },
                    { 14, "Fuel System Components" }
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
                    { 5, 1200.0, "Petrol", "FIAT Petrol 1.2", 90 },
                    { 6, 1600.0, "Diesel", "TDI 1.6", 120 },
                    { 7, 2000.0, "Petrol", "TSI 2.0", 200 },
                    { 8, 2000.0, "Hybrid", "Hybrid E-Drive", 250 },
                    { 9, 0.0, "Electric", "Electric P150", 150 },
                    { 10, 1000.0, "Petrol", "EcoBoost 1.0", 125 },
                    { 11, 1500.0, "Petrol", "VTEC 1.5", 180 },
                    { 12, 2500.0, "Petrol", "Skyactiv 2.5", 187 },
                    { 13, 1600.0, "Petrol", "GDi 1.6", 177 },
                    { 14, 5700.0, "Petrol", "Hemi 5.7", 395 },
                    { 15, 2000.0, "Petrol", "T6 2.0", 316 },
                    { 16, 3000.0, "Diesel", "3.0 TDI", 245 },
                    { 17, 3000.0, "Diesel", "N57D30O0", 241 },
                    { 18, 5500.0, "Petrol", "M157 V8", 577 }
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
                table: "Types",
                columns: new[] { "TypeId", "Name" },
                values: new object[,]
                {
                    { 1, "Sedan" },
                    { 2, "Compact" },
                    { 3, "SUV" },
                    { 4, "Hatchback" }
                });

            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "ID", "CountryId", "Name", "PostalCode" },
                values: new object[,]
                {
                    { 1, 1, "Banja Luka", 78000L },
                    { 2, 1, "Bihać", 77000L },
                    { 3, 1, "Bijeljina", 76300L },
                    { 4, 1, "Bosanska Krupa", 77240L },
                    { 5, 1, "Cazin", 77220L },
                    { 6, 1, "Čapljina", 88300L },
                    { 7, 1, "Derventa", 74400L },
                    { 8, 1, "Doboj", 74000L },
                    { 9, 1, "Goražde", 73000L },
                    { 10, 1, "Gračanica", 75320L },
                    { 11, 1, "Konjic", 88400L },
                    { 12, 1, "Laktaši", 78250L },
                    { 13, 1, "Livno", 80101L },
                    { 14, 1, "Lukavac", 75300L },
                    { 15, 1, "Ljubuški", 88320L },
                    { 16, 1, "Mostar", 88000L },
                    { 17, 1, "Orašje", 76270L },
                    { 18, 1, "Prijedor", 79101L },
                    { 19, 1, "Prnjavor", 78430L },
                    { 20, 1, "Sarajevo", 71000L },
                    { 21, 1, "Srebrenik", 75350L },
                    { 22, 1, "Stolac", 88360L },
                    { 23, 1, "Široki Brijeg", 88220L },
                    { 24, 1, "Travnik", 72270L },
                    { 25, 1, "Tuzla", 75000L },
                    { 26, 1, "Visoko", 71300L },
                    { 27, 1, "Zavidovići", 72220L },
                    { 28, 1, "Zenica", 72000L },
                    { 29, 1, "Zvornik", 75400L },
                    { 30, 1, "Živinice", 75270L },
                    { 31, 1, "Donji Vakuf", 70220L }
                });

            migrationBuilder.InsertData(
                table: "Models",
                columns: new[] { "ModelId", "CarId", "EngineId", "Name", "Year" },
                values: new object[,]
                {
                    { 1, 4, 1, "508", 2020 },
                    { 2, 5, 2, "Panda", 2019 },
                    { 3, 1, 3, "Tiguan", 2022 },
                    { 4, 1, 4, "Passat", 2021 },
                    { 5, 1, 5, "Golf 8", 2023 },
                    { 6, 1, 6, "Golf 7", 2020 },
                    { 7, 6, 7, "X5", 2022 },
                    { 8, 6, 8, "3 Series", 2021 },
                    { 9, 7, 9, "C-Class", 2023 },
                    { 10, 7, 10, "GLE", 2022 },
                    { 11, 8, 11, "Model S", 2023 },
                    { 12, 9, 12, "Focus", 2021 },
                    { 13, 10, 13, "Corolla", 2020 },
                    { 14, 11, 14, "Civic", 2022 },
                    { 15, 12, 15, "Altima", 2021 },
                    { 16, 13, 10, "CX-5", 2023 },
                    { 17, 6, 17, "5 series", 2014 },
                    { 18, 4, 13, "308", 2008 },
                    { 19, 7, 18, "S63", 2020 },
                    { 20, 2, 16, "A6", 2013 },
                    { 21, 6, 17, "7 series", 2013 }
                });

            migrationBuilder.InsertData(
                table: "Parts",
                columns: new[] { "PartId", "CategoryId", "Description", "IsFeatured", "IsNewArrival", "IsOnSale", "ManufacturerId", "Name", "PartImage", "Price", "TypeId" },
                values: new object[,]
                {
                    { 1, 1, "High-quality brake pads for Golf", true, false, false, 1, "Brake Pads", "/images/BOSCH_brake_pads.jpg", 85.0, null },
                    { 2, 2, "Durable suspension springs for Q5,Tiguan", true, false, false, 2, "Suspension Springs", "/images/Suspension_Springs_q5.jpg", 220.0, null },
                    { 3, 6, "Oil filter suitable for Clio engines", true, false, false, 3, "Engine Oil Filter", "/images/oil_filter_renault_clio.jpg", 25.0, null },
                    { 4, 4, "Rust-resistant exhaust pipe for 508", true, false, false, 4, "Exhaust Pipe", "/images/Exhaust_Pipe peugeot_508.jpg", 300.0, 1 },
                    { 5, 3, "Durable timing belt for Golf", true, false, false, 5, "Timing Belt", "/images/Timing_Belt_gol_VI.jpg", 120.0, null },
                    { 6, 8, "High-efficiency water pump for Panda", true, false, false, 6, "Water Pump", "/images/Water_Pump_panda.jpg", 200.0, null },
                    { 7, 3, "Precision camshaft for Q5", true, false, false, 7, "Camshaft", "/images/Camshaft_q5.jpg", 500.0, null },
                    { 8, 7, "Reliable starter motor for Clio", true, false, false, 8, "Starter Motor", "/images/Starter_Motor_clio.jpg", 250.0, null },
                    { 9, 8, "High-performance coolant hose for Panda", false, true, false, 5, "Coolant Hose", "/images/Coolant_Hose_panda.jpg", 40.0, null },
                    { 10, 6, "Improves air intake efficiency for Golf", false, true, false, 6, "Air Filter", "/images/Air_Filter_golf_6.jpg", 35.0, null },
                    { 11, 2, "Enhanced suspension system for Q5", false, true, false, 7, "Shock Absorber", "/images/Shock_Absorber_q5.jpg", 250.0, null },
                    { 12, 7, "High-performance spark plugs for Clio", false, true, false, 8, "Spark Plugs", "/images/spark_plugs_clio.jpg", 50.0, null },
                    { 13, 1, "Flexible brake hoses for Panda", false, true, false, 5, "Brake Hoses", "/images/brake_hoses_panda.jpg", 60.0, null },
                    { 14, 8, "Pressure-regulating radiator cap for Golf", false, true, false, 6, "Radiator Cap", "/images/radiator_cap_golVI.jpg", 20.0, null },
                    { 15, 2, "Precision wheel bearings for Q5", false, true, false, 7, "Wheel Bearings", "/images/wheel_bearings_q5.jpg", 100.0, null },
                    { 16, 5, "Complete clutch kit for Clio", false, true, false, 8, "Clutch Kit", "/images/clutch_kit_clio.jpg", 300.0, null },
                    { 17, 4, "Eco-friendly exhaust component for 508", false, false, true, 1, "Catalytic Converter", "/images/catalytic_converter_508.jpg", 450.0, 2 },
                    { 18, 5, "Premium oil for Panda engines", false, false, true, 2, "Engine Oil", "/images/engine_oil_panda.jpg", 90.0, null },
                    { 19, 1, "Front brake discs for Golf", false, false, true, 3, "Brake Discs", "/images/brake_discs_golfVI.jpg", 150.0, null },
                    { 20, 2, "Steering system component for Q5", false, false, true, 4, "Tie Rod Ends", "/images/tie_rod_ends_q5.jpg", 180.0, null },
                    { 21, 4, "Durable exhaust manifold for Panda", false, false, true, 5, "Exhaust Manifold", "/images/exhaust_manifold_panda.jpg", 400.0, null },
                    { 22, 3, "High-performance fuel injector for Golf", false, false, true, 6, "Fuel Injector", "/images/fuel_injector_golfVI.jpg", 250.0, null },
                    { 23, 8, "Heat-resistant turbo hose for Q5,Tiguan", false, false, true, 7, "Turbo Hose", "/images/turbo_hose_q5.jpg", 80.0, null },
                    { 24, 9, "Efficient AC compressor for Clio", false, false, true, 8, "Air Conditioning Compressor", "/images/air_conditioning_compressor_clio.jpg", 600.0, null },
                    { 25, 11, "Stop light for Passat", false, false, false, 1, "Stop light", "/images/stoplight_passat.jpg", 150.0, 1 },
                    { 26, 7, "German made battery charging part F10", false, false, false, 1, "Alternator", "/images/Alternator_BMW.jpg", 350.0, null },
                    { 27, 8, "High-quality cooling part for 308", false, false, false, 2, "Radiator", "/images/Radiator_308.jpg", 200.0, null },
                    { 28, 12, "Clear and blinding headlights for S63", false, false, false, 1, "Headlights", "/images/Headlight_S63.jpg", 320.0, null },
                    { 29, 12, "LED tailights,highly visible for A6", false, false, false, 2, "Taillights", "/images/Taillights-A6.jpg", 100.0, 1 },
                    { 30, 7, "Long lasting battery with start-stop system for BMW 7-series", false, false, false, 1, "Car Battery", "/images/Battery_BMW.jpg", 280.0, null },
                    { 31, 11, "Smooth and high quality wipers for Tiguan", false, false, false, 4, "Windshield Wipers", "/images/Wipers_VW.jpg", 54.0, null },
                    { 32, 5, "Durable pump for Passat", false, false, false, 2, "Oil Pump", "/images/OilPump_Passat.jpg", 140.0, null },
                    { 33, 14, "Pump with filter for F10", false, false, false, 2, "Fuel Pump", "/images/FuelPump_BMW.jpg", 170.0, null },
                    { 34, 9, "Dashboard with sun protection for GLE", false, false, false, 3, "Dashboard", "/images/DashBoard_Mercedes.jpg", 480.0, null },
                    { 35, 9, "M-Performance Steering wheel for 3 series", false, false, false, 8, "Steering Wheel", "/images/SteeringWheel_3S.jpg", 220.0, null }
                });

            migrationBuilder.InsertData(
                table: "ModelParts",
                columns: new[] { "ModelPartId", "ModelId", "PartId" },
                values: new object[,]
                {
                    { 1, 1, 4 },
                    { 2, 1, 17 },
                    { 3, 2, 3 },
                    { 4, 2, 6 },
                    { 5, 3, 23 },
                    { 6, 4, 5 },
                    { 8, 6, 19 },
                    { 9, 7, 15 },
                    { 10, 9, 12 },
                    { 11, 4, 25 },
                    { 12, 3, 2 },
                    { 13, 14, 16 },
                    { 14, 13, 22 },
                    { 15, 15, 21 },
                    { 16, 17, 26 },
                    { 17, 17, 33 },
                    { 18, 18, 27 },
                    { 19, 19, 28 },
                    { 20, 20, 29 },
                    { 21, 21, 30 },
                    { 22, 3, 31 },
                    { 23, 4, 32 },
                    { 24, 10, 34 },
                    { 25, 8, 35 }
                });

            migrationBuilder.InsertData(
                table: "PartEngines",
                columns: new[] { "PartEngineId", "EngineId", "PartId" },
                values: new object[,]
                {
                    { 1, 4, 3 },
                    { 2, 12, 4 },
                    { 3, 6, 5 },
                    { 4, 5, 6 },
                    { 5, 13, 7 },
                    { 6, 9, 8 },
                    { 7, 5, 9 },
                    { 8, 1, 10 },
                    { 9, 6, 22 },
                    { 10, 7, 23 },
                    { 11, 13, 24 },
                    { 12, 17, 26 },
                    { 13, 13, 27 },
                    { 14, 1, 32 },
                    { 15, 17, 33 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_PartId",
                table: "CartItems",
                column: "PartId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_UserId",
                table: "CartItems",
                column: "UserId");

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
                name: "IX_ModelParts_ModelId",
                table: "ModelParts",
                column: "ModelId");

            migrationBuilder.CreateIndex(
                name: "IX_ModelParts_PartId",
                table: "ModelParts",
                column: "PartId");

            migrationBuilder.CreateIndex(
                name: "IX_Models_CarId",
                table: "Models",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_Models_EngineId",
                table: "Models",
                column: "EngineId");

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
                name: "IX_Orders_PromoCodeId",
                table: "Orders",
                column: "PromoCodeId");

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
                name: "IX_PartEngines_EngineId",
                table: "PartEngines",
                column: "EngineId");

            migrationBuilder.CreateIndex(
                name: "IX_PartEngines_PartId",
                table: "PartEngines",
                column: "PartId");

            migrationBuilder.CreateIndex(
                name: "IX_Parts_CategoryId",
                table: "Parts",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Parts_ManufacturerId",
                table: "Parts",
                column: "ManufacturerId");

            migrationBuilder.CreateIndex(
                name: "IX_Parts_TypeId",
                table: "Parts",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserAccountId",
                table: "RefreshTokens",
                column: "UserAccountId");

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
                name: "CartItems");

            migrationBuilder.DropTable(
                name: "Dashboards");

            migrationBuilder.DropTable(
                name: "FAQs");

            migrationBuilder.DropTable(
                name: "Journeys");

            migrationBuilder.DropTable(
                name: "ModelParts");

            migrationBuilder.DropTable(
                name: "MyAuthenticationTokens");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "PartEngines");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "Models");

            migrationBuilder.DropTable(
                name: "MyAppUsers");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Parts");

            migrationBuilder.DropTable(
                name: "Cars");

            migrationBuilder.DropTable(
                name: "Engines");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "PromoCodes");

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
                name: "Types");

            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropTable(
                name: "Genders");

            migrationBuilder.DropTable(
                name: "Countries");
        }
    }
}
