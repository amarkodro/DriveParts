using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Data.Models;

namespace RS1_2024_25.API.Endpoints
{
    [ApiController]
    [Route("api/[controller]")]
    public class DataSeedController(ApplicationDbContext _db, IPasswordHasher<UserAccount> _passwordHasher) : ControllerBase
    {
        [HttpPost("add")]
        public async Task<IActionResult> SeedUserAccount()
        {
            if(_db.UserAccounts.Any()) return BadRequest("Users already exist.");

            var admin1 = new Admin
            {               
                Username = "amar.kodro",
                Email = "amar.kodro@edu.fit.ba",
                Name = "Amar",
                Surname = "Kodro",
                PhoneNumber = "0623331233",
                Address = "Masline-Kocine bb",
                IsAdmin = true,
                isUser = false,
                is2FActive = true,
                AdminLevel = "Moderator",
                ImageUrl = "UserImages/EFOJ7431.png"
            };

            admin1.Password = _passwordHasher.HashPassword(admin1, "admin123");


            var admin2 = new Admin
            {               
                Username = "ammar.puce",
                Email = "ammar.puce@edu.fit.ba",
                Name = "Ammar",
                Surname = "Puce",
                PhoneNumber = "0623331233",
                Address = "Masline-Kocine bb",
                IsAdmin = true,
                isUser = false,
                is2FActive = true,
                AdminLevel = "Moderator",
                ImageUrl = "UserImages/istockphoto-871752462-612x612.jpg"
            };

            admin2.Password = _passwordHasher.HashPassword(admin2, "admin123");

            var user1 = new User
            {             
                Username = "TestUser1",
                Email = "testuser@example.com",
                Name = "Test",
                Surname = "User",
                PhoneNumber = "0602213312",
                Address = "useraddress",
                IsAdmin = false,
                isUser = true,
                is2FActive = false,
                CityId = 18,
                GenderId = 1,
                ImageUrl = "UserImages/istockphoto-871752462-612x612.jpg"
            };

            user1.Password = _passwordHasher.HashPassword(user1, "driveparts123");


            var user2 = new User
            {
                Username = "TestUser2",
                Email = "testuser2@example.com",
                Name = "Test2",
                Surname = "User2",
                PhoneNumber = "0602234312",
                Address = "useraddress2",
                IsAdmin = false,
                isUser = true,
                is2FActive = false,
                CityId = 16,
                GenderId = 2,
                ImageUrl = "UserImages/istockphoto-871752462-612x612.jpg"
            };

            user2.Password = _passwordHasher.HashPassword(user2, "driveparts123");

            _db.AddAsync(admin1);
            _db.AddAsync(admin2);
            _db.AddAsync(user1);
            _db.AddAsync(user2);

            await _db.SaveChangesAsync();
            


            // --- Orders ---
            var orders = new List<Order>
            {
                new Order { UserId = 3, PaymentId = 1, StatusId = 1, SupplierId = 1, Date = new DateTime(2024, 12, 1) },
                new Order { UserId = 4, PaymentId = 2, StatusId = 2, SupplierId = 2, Date = new DateTime(2024, 11, 30) },
                new Order { UserId = 3, PaymentId = 2, StatusId = 3, SupplierId = 4, Date = new DateTime(2024, 12, 2) },
                new Order { UserId = 4, PaymentId = 1, StatusId = 4, SupplierId = 5, Date = new DateTime(2024, 12, 3) },
                new Order { UserId = 3, PaymentId = 1, StatusId = 5, SupplierId = 6, Date = new DateTime(2024, 12, 4) },
                new Order { UserId = 4, PaymentId = 2, StatusId = 6, SupplierId = 7, Date = new DateTime(2024, 12, 5) },
                new Order { UserId = 3, PaymentId = 2, StatusId = 7, SupplierId = 8, Date = new DateTime(2024, 12, 6) },
                new Order { UserId = 4, PaymentId = 1, StatusId = 8, SupplierId = 9, Date = new DateTime(2024, 12, 7) },
                new Order { UserId = 3, PaymentId = 1, StatusId = 9, SupplierId = 10, Date = new DateTime(2024, 12, 8) },
                new Order { UserId = 4, PaymentId = 2, StatusId = 10, SupplierId = 3, Date = new DateTime(2024, 12, 9) }
            };

            await _db.Orders.AddRangeAsync(orders);
            await _db.SaveChangesAsync();


            // --- OrderItems ---
            var orderItems = new List<OrderItem>
            {
                new OrderItem { OrderId = 1, PartId = 1, Quantity = 1, Price = 45 },
                new OrderItem { OrderId = 1, PartId = 2, Quantity = 2, Price = 90 },
                new OrderItem { OrderId = 2, PartId = 3, Quantity = 1, Price = 15 },
                new OrderItem { OrderId = 2, PartId = 4, Quantity = 1, Price = 70 },
                new OrderItem { OrderId = 3, PartId = 5, Quantity = 1, Price = 25 },
                new OrderItem { OrderId = 3, PartId = 6, Quantity = 1, Price = 20 },
                new OrderItem { OrderId = 4, PartId = 7, Quantity = 2, Price = 50 },
                new OrderItem { OrderId = 4, PartId = 8, Quantity = 4, Price = 40 },
                new OrderItem { OrderId = 5, PartId = 9, Quantity = 1, Price = 150 },
                new OrderItem { OrderId = 5, PartId = 10, Quantity = 1, Price = 30 },
                new OrderItem { OrderId = 6, PartId = 11, Quantity = 2, Price = 100 },
                new OrderItem { OrderId = 6, PartId = 12, Quantity = 1, Price = 35 },
                new OrderItem { OrderId = 7, PartId = 13, Quantity = 1, Price = 200 },
                new OrderItem { OrderId = 8, PartId = 14, Quantity = 1, Price = 250 },
                new OrderItem { OrderId = 9, PartId = 15, Quantity = 1, Price = 20 }
            };

            await _db.OrderItems.AddRangeAsync(orderItems);
            await _db.SaveChangesAsync();


            var faqs = new List<FAQ>
            {
                new FAQ { Question = "Kako da pronađem pravi dio za moje vozilo?", Answer = "Koristite našu pretragu po modelu vozila ili kontaktirajte podršku za pomoć.", UserId = 4 },
                new FAQ { Question = "How can I track my order?", Answer = "You can track your order using the tracking number sent to your email.", UserId = 4 },
                new FAQ { Question = "Da li nudite povrat novca za neispravne dijelove?", Answer = "Da, povrat novca je moguć unutar 30 dana uz dostavljen dokaz o kupovini.", UserId = 4 },
                new FAQ { Question = "What payment methods are available?", Answer = "We accept card payments, cash on delivery, and bank transfers.", UserId = 4 },
                new FAQ { Question = "Koliko traje dostava?", Answer = "Dostava obično traje 3-5 radnih dana, u zavisnosti od lokacije.", UserId = 3 },
                new FAQ { Question = "Can I return a part if it doesn't fit my vehicle?", Answer = "Yes, you can return unused parts within 15 days of delivery.", UserId = 4 },
                new FAQ { Question = "Da li nudite popuste za veće narudžbe?", Answer = "Da, popusti su dostupni za narudžbe veće od 500 BAM. Kontaktirajte nas za detalje.", UserId = 3 },
                new FAQ { Question = "What should I do if I receive the wrong part?", Answer = "Please contact our support team immediately, and we will arrange for a replacement.", UserId = 4 },
                new FAQ { Question = "Da li je moguće preuzimanje dijelova u prodavnici?", Answer = "Nažalost, trenutno nudimo samo online naručivanje i dostavu.", UserId = 3 },
                new FAQ { Question = "Do you ship internationally?", Answer = "Currently, we only ship within Bosnia and Herzegovina.", UserId = 4 }
            };

            await _db.FAQs.AddRangeAsync(faqs);
            await _db.SaveChangesAsync();


            var reviews = new List<Review>
            {
                new Review { UserId = 3, PartId = 1, Text = "Odličan kvalitet kočnica, stigle brzo i lako ih je bilo ugraditi.", Picture = null, Date = new DateTime(2023, 01, 11) },
                new Review { UserId = 4, PartId = 2, Text = "Ovjes je perfektan, poboljšana stabilnost auta. Preporučujem!", Picture = null, Date = new DateTime(2023, 02, 05) },
                new Review { UserId = 3, PartId = 3, Text = "Filter ulja je odličan, jednostavan za instalaciju i povoljan.", Picture = null, Date = new DateTime(2023, 03, 15) },
                new Review { UserId = 4, PartId = 4, Text = "Cijev za auspuh savršeno odgovara. Bez problema je montirana.", Picture = null, Date = new DateTime(2023, 04, 20) },
                new Review { UserId = 3, PartId = 5, Text = "Crijevo rashladnog sistema odlično obavlja posao. Dostava na vrijeme.", Picture = null, Date = new DateTime(2023, 05, 10) },
                new Review { UserId = 4, PartId = 6, Text = "Zračni filter je povećao efikasnost motora. Zadovoljan kupovinom.", Picture = null, Date = new DateTime(2023, 06, 25) },
                new Review { UserId = 3, PartId = 7, Text = "Amortizeri su vrhunski. Auto je sada puno stabilniji.", Picture = null, Date = new DateTime(2023, 07, 12) }
            };

            await _db.Reviews.AddRangeAsync(reviews);
            await _db.SaveChangesAsync();

            return Ok("Admins, users, orders, order items, FAQ-s, review have been successfully added.");

        }
    }
}
