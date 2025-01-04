using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data.Models;
using System.Security.Cryptography;
using BCrypt.Net;
using System.Reflection.Emit;

namespace RS1_2024_25.API.Data
{
    partial class DataSeeder
    {
        public void DataSeed(ModelBuilder modelBuilder)
        {


            modelBuilder.Entity<City>().HasData(
                new City { ID = 1, Name = "Banja Luka", CountryId = 1 },
                new City { ID = 2, Name = "Bihać", CountryId = 1 },
                new City { ID = 3, Name = "Bijeljina", CountryId = 1 },
                new City { ID = 4, Name = "Bosanska Krupa", CountryId = 1 },
                new City { ID = 5, Name = "Cazin", CountryId = 1 },
                new City { ID = 6, Name = "Čapljina", CountryId = 1 },
                new City { ID = 7, Name = "Drventa", CountryId = 1 },
                new City { ID = 8, Name = "Doboj", CountryId = 1 },
                new City { ID = 9, Name = "Goražde", CountryId = 1 },
                new City { ID = 10, Name = "Gračanica", CountryId = 1 },
                new City { ID = 11, Name = "Cityačac", CountryId = 1 },
                new City { ID = 12, Name = "Cityiška", CountryId = 1 },
                new City { ID = 13, Name = "Konjic", CountryId = 1 },
                new City { ID = 14, Name = "Laktaši", CountryId = 1 },
                new City { ID = 15, Name = "Livno", CountryId = 1 },
                new City { ID = 16, Name = "Lukavac", CountryId = 1 },
                new City { ID = 17, Name = "Ljubuški", CountryId = 1 },
                new City { ID = 18, Name = "Mostar", CountryId = 1 },
                new City { ID = 19, Name = "Orašje", CountryId = 1 },
                new City { ID = 20, Name = "Prijedor", CountryId = 1 },
                new City { ID = 21, Name = "Prnjavor", CountryId = 1 },
                new City { ID = 22, Name = "Sarajevo", CountryId = 1 },
                new City { ID = 23, Name = "Srebrenik", CountryId = 1 },
                new City { ID = 24, Name = "Stolac", CountryId = 1 },
                new City { ID = 25, Name = "Široki Brijeg", CountryId = 1 },
                new City { ID = 26, Name = "Travnik", CountryId = 1 },
                new City { ID = 27, Name = "Tuzla", CountryId = 1 },
                new City { ID = 28, Name = "Visoko", CountryId = 1 },
                new City { ID = 29, Name = "Zavidovići", CountryId = 1 },
                new City { ID = 30, Name = "Zenica", CountryId = 1 },
                new City { ID = 31, Name = "Zvornik", CountryId = 1 },
                new City { ID = 32, Name = "Živinice", CountryId = 1 },
                new City { ID = 33, Name = "Donji Vakuf", CountryId = 1 },
                new City { ID = 34, Name = "Zavidovići", CountryId = 1 }
            );

            // Countries
            modelBuilder.Entity<Country>().HasData(
                new Country { ID = 1, Name = "Bosna i Hercegovina" }
            );

            // Genders
            modelBuilder.Entity<Gender>().HasData(
                new Gender { GenderId = 1, GenderName = "Male" },
                new Gender { GenderId = 2, GenderName = "Female" }
            );

            // Admins
            modelBuilder.Entity<Admin>().HasData(
                new Admin { Id = 1, Username = "amar.kodro", Password = "driveparts2003", IsAdmin = true, isUser = false, is2FActive = true, Address = "Masline-Kocine bb", Email = "amar.kodro@edu.fit.ba", Name = "Amar", Surname = "Kodro", PhoneNumber = "0623331233", AdminLevel = "Moderator" },
                new Admin { Id = 4, Username = "ammar.puce", Password = "driveparts2003", IsAdmin = true, isUser = false, is2FActive = true, Address = "Masline-Kocine bb", Email = "ammar.puce@edu.fit.ba", Name = "Ammar", Surname = "Puce", PhoneNumber = "0623331233", AdminLevel = "Moderator" }
            );

            // Users
            modelBuilder.Entity<User>().HasData(
                new User { Id = 2, Username = "TestUser1", Password = "userpassword123", IsAdmin = false, isUser = true, is2FActive = false, Address = "useraddress", Email = "testuser@example.com", Name = "Test", Surname = "User", CityId = 18, GenderId = 1, PhoneNumber = "0602213312" },
                new User { Id = 3, Username = "TestUser2", Password = "userpassword123", IsAdmin = false, isUser = true, is2FActive = false, Address = "useraddress2", Email = "testuser2@example.com", Name = "Test2", Surname = "User2", CityId = 16, GenderId = 2, PhoneNumber = "0602234312" }
            );

            // Cars
            modelBuilder.Entity<Car>().HasData(
                 new Car { CarId = 1, Brand = "Volkswagen" },
                 new Car { CarId = 2, Brand = "Audi" },
                 new Car { CarId = 3, Brand = "Renault" },
                 new Car { CarId = 4, Brand = "Peugeot" },
                 new Car { CarId = 5, Brand = "Fiat" },
                 new Car { CarId = 6, Brand = "BMW" },
                 new Car { CarId = 7, Brand = "Mercedes" },
                 new Car { CarId = 8, Brand = "Tesla" },
                 new Car { CarId = 9, Brand = "Ford" },
                 new Car { CarId = 10, Brand = "Toyota" },
                 new Car { CarId = 11, Brand = "Honda" },
                 new Car { CarId = 12, Brand = "Nissan" },
                 new Car { CarId = 13, Brand = "Mazda" },
                 new Car { CarId = 14, Brand = "Kia" },
                 new Car { CarId = 15, Brand = "Hyundai" },
                 new Car { CarId = 16, Brand = "Chevrolet" },
                 new Car { CarId = 17, Brand = "Subaru" },
                 new Car { CarId = 18, Brand = "Jeep" },
                 new Car { CarId = 19, Brand = "Volvo" }
             );

            // Engines
            modelBuilder.Entity<Engine>().HasData(
                new Engine { EngineId = 1, Name = "TDI 2.0", Power = 150, Displacement = 2000, FuelType = "Diesel" },
                new Engine { EngineId = 2, Name = "TSI 1.5", Power = 130, Displacement = 1500, FuelType = "Petrol" },
                new Engine { EngineId = 3, Name = "Electric R100", Power = 100, Displacement = 0, FuelType = "Electric" },
                new Engine { EngineId = 4, Name = "HY 1.6", Power = 180, Displacement = 1600, FuelType = "Hybrid" },
                new Engine { EngineId = 5, Name = "FIAT Petrol 1.2", Power = 90, Displacement = 1200, FuelType = "Petrol" },
                new Engine { EngineId = 6, Name = "TDI 1.6", Power = 120, Displacement = 1600, FuelType = "Diesel" },
                new Engine { EngineId = 7, Name = "TSI 2.0", Power = 200, Displacement = 2000, FuelType = "Petrol" },
                new Engine { EngineId = 8, Name = "Hybrid E-Drive", Power = 250, Displacement = 2000, FuelType = "Hybrid" },
                new Engine { EngineId = 9, Name = "Electric P150", Power = 150, Displacement = 0, FuelType = "Electric" },
                new Engine { EngineId = 10, Name = "EcoBoost 1.0", Power = 125, Displacement = 1000, FuelType = "Petrol" },
                new Engine { EngineId = 11, Name = "VTEC 1.5", Power = 180, Displacement = 1500, FuelType = "Petrol" },
                new Engine { EngineId = 12, Name = "Skyactiv 2.5", Power = 187, Displacement = 2500, FuelType = "Petrol" },
                new Engine { EngineId = 13, Name = "GDi 1.6", Power = 177, Displacement = 1600, FuelType = "Petrol" },
                new Engine { EngineId = 14, Name = "Hemi 5.7", Power = 395, Displacement = 5700, FuelType = "Petrol" },
                new Engine { EngineId = 15, Name = "T6 2.0", Power = 316, Displacement = 2000, FuelType = "Petrol" }
            );

            //Types
            modelBuilder.Entity<Types>().HasData(
                new Types { TypeId = 1, Name = "Sedan" },
                new Types { TypeId = 2, Name = "Compact" },
                new Types { TypeId = 3, Name = "SUV" },
                new Types { TypeId = 4, Name = "Hatchback" }
            );

            //Models
            modelBuilder.Entity<Model>().HasData(
                new Model { ModelId = 1, Name = "508", CarId = 4, Year = 2020, EngineId = 1 },
                new Model { ModelId = 2, Name = "Panda", CarId = 5, Year = 2019, EngineId = 2 },
                new Model { ModelId = 3, Name = "Tiguan", CarId = 1, Year = 2022, EngineId = 3 },
                new Model { ModelId = 4, Name = "Passat", CarId = 1, Year = 2021, EngineId = 4 },
                new Model { ModelId = 5, Name = "Golf 8", CarId = 1, Year = 2023, EngineId = 5 },
                new Model { ModelId = 6, Name = "Golf 7", CarId = 1, Year = 2020, EngineId = 6 },
                new Model { ModelId = 7, Name = "X5", CarId = 6, Year = 2022, EngineId = 7 },
                new Model { ModelId = 8, Name = "3 Series", CarId = 6, Year = 2021, EngineId = 8 },
                new Model { ModelId = 9, Name = "C-Class", CarId = 7, Year = 2023, EngineId = 9 },
                new Model { ModelId = 10, Name = "GLE", CarId = 7, Year = 2022, EngineId = 10 },
                new Model { ModelId = 11, Name = "Model S", CarId = 8, Year = 2023, EngineId = 11 },
                new Model { ModelId = 12, Name = "Focus", CarId = 9, Year = 2021, EngineId = 12 },
                new Model { ModelId = 13, Name = "Corolla", CarId = 10, Year = 2020, EngineId = 13 },
                new Model { ModelId = 14, Name = "Civic", CarId = 11, Year = 2022, EngineId = 14 },
                new Model { ModelId = 15, Name = "Altima", CarId = 12, Year = 2021, EngineId = 15 },
                new Model { ModelId = 16, Name = "CX-5", CarId = 13, Year = 2023, EngineId = 10 }
             );

            //ModelParts
            modelBuilder.Entity<ModelPart>().HasData(
                new ModelPart { ModelPartId = 1, ModelId = 1, PartId = 4 },  // 508 -> Exhaust Pipe
                new ModelPart { ModelPartId = 2, ModelId = 1, PartId = 17 }, // 508 -> Catalytic Converter
                new ModelPart { ModelPartId = 3, ModelId = 2, PartId = 3 },  // Panda -> Engine Oil Filter
                new ModelPart { ModelPartId = 4, ModelId = 2, PartId = 6 },  // Panda -> Water Pump
                new ModelPart { ModelPartId = 5, ModelId = 3, PartId = 23 }, // Tiguan -> Turbo Hose
                new ModelPart { ModelPartId = 6, ModelId = 4, PartId = 5 },  // Passat -> Timing Belt
                new ModelPart { ModelPartId = 8, ModelId = 6, PartId = 19 }, // Golf 7 -> Brake Discs
                new ModelPart { ModelPartId = 9, ModelId = 7, PartId = 15 }, // X5 -> Wheel Bearings
                new ModelPart { ModelPartId = 10, ModelId = 9, PartId = 12 },// C-Class -> Spark Plugs
                new ModelPart { ModelPartId = 11, ModelId = 4, PartId = 25 },// Passat -> Stop Light
                new ModelPart { ModelPartId = 12, ModelId = 3, PartId = 2 }, // Tiguan -> Suspension Springs
                new ModelPart { ModelPartId = 13, ModelId = 14, PartId = 16 }, // Civic -> Clutch Kit
                new ModelPart { ModelPartId = 14, ModelId = 13, PartId = 22 }, // Corolla -> Fuel Injector
                new ModelPart { ModelPartId = 15, ModelId = 15, PartId = 21 }  // Altima -> Exhaust Manifold
                );

            // Part Engines
            modelBuilder.Entity<PartEngine>().HasData(
                new PartEngine { PartEngineId = 1, PartId = 3, EngineId = 4 }, // Engine Oil Filter -> HY 1.6
                new PartEngine { PartEngineId = 2, PartId = 4, EngineId = 12 }, // Exhaust Pipe -> Skyactiv 2.5
                new PartEngine { PartEngineId = 3, PartId = 5, EngineId = 6 }, // Timing Belt -> TDI 1.6
                new PartEngine { PartEngineId = 4, PartId = 6, EngineId = 5 }, // Water Pump -> FIAT Petrol 1.2
                new PartEngine { PartEngineId = 5, PartId = 7, EngineId = 13 }, // Camshaft -> GDI 1.6
                new PartEngine { PartEngineId = 6, PartId = 8, EngineId = 9 }, // Starter Motor -> Electric P150
                new PartEngine { PartEngineId = 7, PartId = 9, EngineId = 5 }, // Coolant Hose -> FIAT Petrol 1.2
                new PartEngine { PartEngineId = 8, PartId = 10, EngineId = 1 }, // Air Filter -> TDI 2.0
                new PartEngine { PartEngineId = 9, PartId = 22, EngineId = 6 }, // Fuel Injector -> TDI 1.6
                new PartEngine { PartEngineId = 10, PartId = 23, EngineId = 7 }, // Turbo Hose -> TSI 2.0
                new PartEngine { PartEngineId = 11, PartId = 24, EngineId = 13 } // Air Conditioning Compressor -> GDI 1.6
            );

            // Manufacturers
            modelBuilder.Entity<Manufacturer>().HasData(
                new Manufacturer { ManufacturerId = 1, Name = "Bosch", Contact = "+387 33 770 100", Address = "Džemala Bijedića 185, Sarajevo, Bosna i Hercegovina" },
                new Manufacturer { ManufacturerId = 2, Name = "Valeo", Contact = "+387 51 210 990", Address = "Karađorđeva 120, Banja Luka, Bosna i Hercegovina" },
                new Manufacturer { ManufacturerId = 3, Name = "Delphi Technologies", Contact = "+387 35 320 870", Address = "Rudarska 33, Tuzla, Bosna i Hercegovina" },
                new Manufacturer { ManufacturerId = 4, Name = "Continental", Contact = "+387 36 576 600", Address = "Bišće polje bb, Mostar, Bosna i Hercegovina" },
                new Manufacturer { ManufacturerId = 5, Name = "Magneti Marelli", Contact = "+387 32 450 110", Address = "Industrijska zona bb, Zenica, Bosna i Hercegovina" },
                new Manufacturer { ManufacturerId = 6, Name = "Brembo", Contact = "+387 33 210 320", Address = "Pofalići bb, Sarajevo, Bosna i Hercegovina" },
                new Manufacturer { ManufacturerId = 7, Name = "TRW Automotive", Contact = "+387 51 321 480", Address = "Aleja Svetog Save 15, Banja Luka, Bosna i Hercegovina" },
                new Manufacturer { ManufacturerId = 8, Name = "ATE", Contact = "+387 33 234 567", Address = "Zmaja od Bosne bb, Sarajevo, Bosna i Hercegovina" }
            );

            // Suppliers
            modelBuilder.Entity<Supplier>().HasData(
                new Supplier { SupplierId = 1, Name = "A2B Delivery", Contact = "+387 33 123 456", Address = "Maršala Tita 45, Sarajevo, BiH" },
                new Supplier { SupplierId = 2, Name = "EuroExpress", Contact = "+387 51 789 123", Address = "Kralja Petra I Karađorđevića 102, Banja Luka, BiH" },
                new Supplier { SupplierId = 3, Name = "BH Brza Pošta", Contact = "+387 61 987 654", Address = "Hamdije Kreševljakovića 50, Mostar, BiH" },
                new Supplier { SupplierId = 4, Name = "Sky Express", Contact = "+387 36 456 789", Address = "Zagrebačka 10, Tuzla, BiH" },
                new Supplier { SupplierId = 5, Name = "Express One", Contact = "+387 32 555 888", Address = "Bosanska 25, Zenica, BiH" },
                new Supplier { SupplierId = 6, Name = "FastTrack Logistics", Contact = "+387 35 222 333", Address = "Srebrenička 7, Brčko, BiH" },
                new Supplier { SupplierId = 7, Name = "DHL Bosnia", Contact = "+387 33 445 666", Address = "Zmaja od Bosne 12, Sarajevo, BiH" },
                new Supplier { SupplierId = 8, Name = "GLS BiH", Contact = "+387 37 777 888", Address = "Goranska 15, Bihać, BiH" },
                new Supplier { SupplierId = 9, Name = "UPS Delivery", Contact = "+387 66 123 321", Address = "Savska 4, Bijeljina, BiH" },
                new Supplier { SupplierId = 10, Name = "PostExpress BiH", Contact = "+387 65 999 000", Address = "Prijedorska 22, Prijedor, BiH" }
            );

            // Categories
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, Name = "Brakes" },
                new Category { CategoryId = 2, Name = "Suspension" },
                new Category { CategoryId = 3, Name = "Engine Parts" },
                new Category { CategoryId = 4, Name = "Exhaust Systems" },
                new Category { CategoryId = 5, Name = "Oil and Fluids" },
                new Category { CategoryId = 6, Name = "Filters" },
                new Category { CategoryId = 7, Name = "Electrical Components" },
                new Category { CategoryId = 8, Name = "Cooling Systems" },
                new Category { CategoryId = 9, Name = "Interior Accessories" },
                new Category { CategoryId = 10, Name = "Tires and Wheels" },
                new Category { CategoryId = 11, Name = "Body Parts and Exterior" },
                new Category { CategoryId = 12, Name = "Lighting Systems" },
                new Category { CategoryId = 13, Name = "Transmission Parts" },
                new Category { CategoryId = 14, Name = "Fuel System Components" }
            );

            //Parts
            modelBuilder.Entity<Part>().HasData(
               new Part { PartId = 1, Name = "Brake Pads", Description = "High-quality brake pads for Golf", CategoryId = 1, ManufacturerId = 1, Price = 85, IsFeatured = true, IsNewArrival = false, IsOnSale = false, PartImage = "/images/BOSCH_brake_pads.jpg" },
                new Part { PartId = 2, Name = "Suspension Springs", Description = "Durable suspension springs for Q5,Tiguan", CategoryId = 2, ManufacturerId = 2, Price = 220, IsFeatured = true, IsNewArrival = false, IsOnSale = false, PartImage = "/images/Suspension_Springs_q5.jpg" },
                new Part { PartId = 3, Name = "Engine Oil Filter", Description = "Oil filter suitable for Clio engines", CategoryId = 6, ManufacturerId = 3, Price = 25, IsFeatured = true, IsNewArrival = false, IsOnSale = false, PartImage = "/images/oil_filter_renault_clio.jpg" },
                new Part { PartId = 4, Name = "Exhaust Pipe", Description = "Rust-resistant exhaust pipe for 508", CategoryId = 4, ManufacturerId = 4, Price = 300, IsFeatured = true, IsNewArrival = false, IsOnSale = false, PartImage = "/images/Exhaust_Pipe_peugeot_508.jpg", TypeId = 1 },
                new Part { PartId = 5, Name = "Timing Belt", Description = "Durable timing belt for Golf", CategoryId = 3, ManufacturerId = 5, Price = 120, IsFeatured = true, IsNewArrival = false, IsOnSale = false, PartImage = "/images/Timing_Belt_gol_VI.jpg" },
                new Part { PartId = 6, Name = "Water Pump", Description = "High-efficiency water pump for Panda", CategoryId = 8, ManufacturerId = 6, Price = 200, IsFeatured = true, IsNewArrival = false, IsOnSale = false, PartImage = "/images/Water_Pump_panda.jpg" },
                new Part { PartId = 7, Name = "Camshaft", Description = "Precision camshaft for Q5", CategoryId = 3, ManufacturerId = 7, Price = 500, IsFeatured = true, IsNewArrival = false, IsOnSale = false, PartImage = "/images/Camshaft_q5.jpg" },
                new Part { PartId = 8, Name = "Starter Motor", Description = "Reliable starter motor for Clio", CategoryId = 7, ManufacturerId = 8, Price = 250, IsFeatured = true, IsNewArrival = false, IsOnSale = false, PartImage = "/images/Starter_Motor_clio.jpg" },
                new Part { PartId = 9, Name = "Coolant Hose", Description = "High-performance coolant hose for Panda", CategoryId = 8, ManufacturerId = 5, Price = 40, IsFeatured = false, IsNewArrival = true, IsOnSale = false, PartImage = "/images/Coolant_Hose_panda.jpg" },
                new Part { PartId = 10, Name = "Air Filter", Description = "Improves air intake efficiency for Golf", CategoryId = 6, ManufacturerId = 6, Price = 35, IsFeatured = false, IsNewArrival = true, IsOnSale = false, PartImage = "/images/Air_Filter_golf_6.jpg" },
                new Part { PartId = 11, Name = "Shock Absorber", Description = "Enhanced suspension system for Q5", CategoryId = 2, ManufacturerId = 7, Price = 250, IsFeatured = false, IsNewArrival = true, IsOnSale = false, PartImage = "/images/Shock_Absorber_q5.jpg" },
                new Part { PartId = 12, Name = "Spark Plugs", Description = "High-performance spark plugs for Clio", CategoryId = 7, ManufacturerId = 8, Price = 50, IsFeatured = false, IsNewArrival = true, IsOnSale = false, PartImage = "/images/spark_plugs_clio.jpg" },
                new Part { PartId = 13, Name = "Brake Hoses", Description = "Flexible brake hoses for Panda", CategoryId = 1, ManufacturerId = 5, Price = 60, IsFeatured = false, IsNewArrival = true, IsOnSale = false, PartImage = "/images/brake_hoses_panda.jpg" },
                new Part { PartId = 14, Name = "Radiator Cap", Description = "Pressure-regulating radiator cap for Golf", CategoryId = 8, ManufacturerId = 6, Price = 20, IsFeatured = false, IsNewArrival = true, IsOnSale = false, PartImage = "/images/radiator_cap_golVI.jpg" },
                new Part { PartId = 15, Name = "Wheel Bearings", Description = "Precision wheel bearings for Q5", CategoryId = 2, ManufacturerId = 7, Price = 100, IsFeatured = false, IsNewArrival = true, IsOnSale = false, PartImage = "/images/wheel_bearings_q5.jpg" },
                new Part { PartId = 16, Name = "Clutch Kit", Description = "Complete clutch kit for Clio", CategoryId = 5, ManufacturerId = 8, Price = 300, IsFeatured = false, IsNewArrival = true, IsOnSale = false, PartImage = "/images/clutch_kit_clio.jpg" },
                new Part { PartId = 17, Name = "Catalytic Converter", Description = "Eco-friendly exhaust component for 508", CategoryId = 4, ManufacturerId = 1, Price = 450, IsFeatured = false, IsNewArrival = false, IsOnSale = true, PartImage = "/images/catalytic_converter_508.jpg", TypeId = 2 },
                new Part { PartId = 18, Name = "Engine Oil", Description = "Premium oil for Panda engines", CategoryId = 5, ManufacturerId = 2, Price = 90, IsFeatured = false, IsNewArrival = false, IsOnSale = true, PartImage = "/images/engine_oil_panda.jpg" },
                new Part { PartId = 19, Name = "Brake Discs", Description = "Front brake discs for Golf", CategoryId = 1, ManufacturerId = 3, Price = 150, IsFeatured = false, IsNewArrival = false, IsOnSale = true, PartImage = "/images/brake_discs_golfVI.jpg" },
                new Part { PartId = 20, Name = "Tie Rod Ends", Description = "Steering system component for Q5", CategoryId = 2, ManufacturerId = 4, Price = 180, IsFeatured = false, IsNewArrival = false, IsOnSale = true, PartImage = "/images/tie_rod_ends_q5.jpg" },
                new Part { PartId = 21, Name = "Exhaust Manifold", Description = "Durable exhaust manifold for Panda", CategoryId = 4, ManufacturerId = 5, Price = 400, IsFeatured = false, IsNewArrival = false, IsOnSale = true, PartImage = "/images/exhaust_manifold_panda.jpg" },
                new Part { PartId = 22, Name = "Fuel Injector", Description = "High-performance fuel injector for Golf", CategoryId = 3, ManufacturerId = 6, Price = 250, IsFeatured = false, IsNewArrival = false, IsOnSale = true, PartImage = "/images/fuel_injector_golfVI.jpg" },
                new Part { PartId = 23, Name = "Turbo Hose", Description = "Heat-resistant turbo hose for Q5,Tiguan", CategoryId = 8, ManufacturerId = 7, Price = 80, IsFeatured = false, IsNewArrival = false, IsOnSale = true, PartImage = "/images/turbo_hose_q5.jpg" },
                new Part { PartId = 24, Name = "Air Conditioning Compressor", Description = "Efficient AC compressor for Clio", CategoryId = 9, ManufacturerId = 8, Price = 600, IsFeatured = false, IsNewArrival = false, IsOnSale = true, PartImage = "/images/air_conditioning_compressor_clio.jpg" },
                new Part { PartId = 25, Name = "Stop light", Description = "Stop light for Passat", CategoryId = 11, ManufacturerId = 1, Price = 150, IsFeatured = false, IsNewArrival = false, IsOnSale = false, PartImage = "/images/stoplight_passat.jpg", TypeId = 3 }
            );

            //Payments 
            modelBuilder.Entity<Payment>().HasData(

                new Payment { PaymentId = 1, PaymentMethod = "Card" },
                new Payment { PaymentId = 2, PaymentMethod = "Cash" }

            );

            //Status
            modelBuilder.Entity<Status>().HasData(

                new Status { StatusId = 1, Name = "Pending" },
                new Status { StatusId = 2, Name = "Approved" },
                new Status { StatusId = 3, Name = "Rejected" },
                new Status { StatusId = 4, Name = "In Progress" },
                new Status { StatusId = 5, Name = "Completed" },
                new Status { StatusId = 6, Name = "Cancelled" },
                new Status { StatusId = 7, Name = "On Hold" },
                new Status { StatusId = 8, Name = "Failed" },
                new Status { StatusId = 9, Name = "Draft" },
                new Status { StatusId = 10, Name = "Submitted" }

            );

            //Orders
            modelBuilder.Entity<Order>().HasData(

                new Order { OrderId = 1, UserId = 2, PaymentId = 1, StatusId = 1, SupplierId = 1, Date = new DateTime(2024, 12, 1) },
                new Order { OrderId = 2, UserId = 3, PaymentId = 2, StatusId = 2, SupplierId = 2, Date = new DateTime(2024, 11, 30) },
                new Order { OrderId = 3, UserId = 2, PaymentId = 2, StatusId = 3, SupplierId = 4, Date = new DateTime(2024, 12, 2) },
                new Order { OrderId = 4, UserId = 3, PaymentId = 1, StatusId = 4, SupplierId = 5, Date = new DateTime(2024, 12, 3) },
                new Order { OrderId = 5, UserId = 2, PaymentId = 1, StatusId = 5, SupplierId = 6, Date = new DateTime(2024, 12, 4) },
                new Order { OrderId = 6, UserId = 3, PaymentId = 2, StatusId = 6, SupplierId = 7, Date = new DateTime(2024, 12, 5) },
                new Order { OrderId = 7, UserId = 2, PaymentId = 2, StatusId = 7, SupplierId = 8, Date = new DateTime(2024, 12, 6) },
                new Order { OrderId = 8, UserId = 3, PaymentId = 1, StatusId = 8, SupplierId = 9, Date = new DateTime(2024, 12, 7) },
                new Order { OrderId = 9, UserId = 2, PaymentId = 1, StatusId = 9, SupplierId = 10, Date = new DateTime(2024, 12, 8) },
                new Order { OrderId = 10, UserId = 3, PaymentId = 2, StatusId = 10, SupplierId = 3, Date = new DateTime(2024, 12, 9) }

            );

            //OrderItems
            modelBuilder.Entity<OrderItem>().HasData(

                new OrderItem { OrderItemId = 1, OrderId = 1, PartId = 1, Quantity = 1, Price = 45 },
                new OrderItem { OrderItemId = 2, OrderId = 1, PartId = 2, Quantity = 2, Price = 90 },
                new OrderItem { OrderItemId = 3, OrderId = 2, PartId = 3, Quantity = 1, Price = 15 },
                new OrderItem { OrderItemId = 4, OrderId = 2, PartId = 4, Quantity = 1, Price = 70 },
                new OrderItem { OrderItemId = 5, OrderId = 3, PartId = 5, Quantity = 1, Price = 25 },
                new OrderItem { OrderItemId = 6, OrderId = 3, PartId = 6, Quantity = 1, Price = 20 },
                new OrderItem { OrderItemId = 7, OrderId = 4, PartId = 7, Quantity = 2, Price = 50 },
                new OrderItem { OrderItemId = 8, OrderId = 4, PartId = 8, Quantity = 4, Price = 40 },
                new OrderItem { OrderItemId = 9, OrderId = 5, PartId = 9, Quantity = 1, Price = 150 },
                new OrderItem { OrderItemId = 10, OrderId = 5, PartId = 10, Quantity = 1, Price = 30 },
                new OrderItem { OrderItemId = 11, OrderId = 6, PartId = 11, Quantity = 2, Price = 100 },
                new OrderItem { OrderItemId = 12, OrderId = 6, PartId = 12, Quantity = 1, Price = 35 },
                new OrderItem { OrderItemId = 13, OrderId = 7, PartId = 13, Quantity = 1, Price = 200 },
                new OrderItem { OrderItemId = 14, OrderId = 8, PartId = 14, Quantity = 1, Price = 250 },
                new OrderItem { OrderItemId = 15, OrderId = 9, PartId = 15, Quantity = 1, Price = 20 }

            );

            //FAQs
            modelBuilder.Entity<FAQ>().HasData(

                new FAQ { FAQId = 1, Question = "Kako da pronađem pravi dio za moje vozilo?", Answer = "Koristite našu pretragu po modelu vozila ili kontaktirajte podršku za pomoć.", UserId = 2 },
                new FAQ { FAQId = 2, Question = "How can I track my order?", Answer = "You can track your order using the tracking number sent to your email.", UserId = 3 },
                new FAQ { FAQId = 3, Question = "Da li nudite povrat novca za neispravne dijelove?", Answer = "Da, povrat novca je moguć unutar 30 dana uz dostavljen dokaz o kupovini.", UserId = 2 },
                new FAQ { FAQId = 4, Question = "What payment methods are available?", Answer = "We accept card payments, cash on delivery, and bank transfers.", UserId = 3 },
                new FAQ { FAQId = 5, Question = "Koliko traje dostava?", Answer = "Dostava obično traje 3-5 radnih dana, u zavisnosti od lokacije.", UserId = 2 },
                new FAQ { FAQId = 6, Question = "Can I return a part if it doesn't fit my vehicle?", Answer = "Yes, you can return unused parts within 15 days of delivery.", UserId = 3 },
                new FAQ { FAQId = 7, Question = "Da li nudite popuste za veće narudžbe?", Answer = "Da, popusti su dostupni za narudžbe veće od 500 BAM. Kontaktirajte nas za detalje.", UserId = 2 },
                new FAQ { FAQId = 8, Question = "What should I do if I receive the wrong part?", Answer = "Please contact our support team immediately, and we will arrange for a replacement.", UserId = 3 },
                new FAQ { FAQId = 9, Question = "Da li je moguće preuzimanje dijelova u prodavnici?", Answer = "Nažalost, trenutno nudimo samo online naručivanje i dostavu.", UserId = 2 },
                new FAQ { FAQId = 10, Question = "Do you ship internationally?", Answer = "Currently, we only ship within Bosnia and Herzegovina.", UserId = 3 }

            );

            //Reviews
            modelBuilder.Entity<Review>().HasData(

                new Review { ReviewId = 1, UserId = 2, PartId = 1, Text = "Odličan kvalitet kočnica, stigle brzo i lako ih je bilo ugraditi.", Picture = null, Date = new DateTime(2023, 01, 11) },
                new Review { ReviewId = 2, UserId = 3, PartId = 2, Text = "Ovjes je perfektan, poboljšana stabilnost auta. Preporučujem!", Picture = null, Date = new DateTime(2023, 02, 05) },
                new Review { ReviewId = 3, UserId = 2, PartId = 3, Text = "Filter ulja je odličan, jednostavan za instalaciju i povoljan.", Picture = null, Date = new DateTime(2023, 03, 15) },
                new Review { ReviewId = 4, UserId = 3, PartId = 4, Text = "Cijev za auspuh savršeno odgovara. Bez problema je montirana.", Picture = null, Date = new DateTime(2023, 04, 20) },
                new Review { ReviewId = 5, UserId = 2, PartId = 5, Text = "Crijevo rashladnog sistema odlično obavlja posao. Dostava na vrijeme.", Picture = null, Date = new DateTime(2023, 05, 10) },
                new Review { ReviewId = 6, UserId = 3, PartId = 6, Text = "Zračni filter je povećao efikasnost motora. Zadovoljan kupovinom.", Picture = null, Date = new DateTime(2023, 06, 25) },
                new Review { ReviewId = 7, UserId = 2, PartId = 7, Text = "Amortizeri su vrhunski. Auto je sada puno stabilniji.", Picture = null, Date = new DateTime(2023, 07, 12) }

            );
        }
    }
}