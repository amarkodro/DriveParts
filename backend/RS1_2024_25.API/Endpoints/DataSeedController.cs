using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Data.Models;

namespace RS1_2024_25.API.Endpoints
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
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
                PhoneNumber = "+387 62 111 111",
                Address = "Masline-Kocine bb",
                IsAdmin = true,
                isUser = false,
                is2FActive = true,
                AdminLevel = "Moderator",
                ImageUrl = "UserImages/EFOJ7431.png",
                CityId = 18,
                
            };

            admin1.Password = _passwordHasher.HashPassword(admin1, "admin123");


            var admin2 = new Admin
            {               
                Username = "ammar.puce",
                Email = "ammar.puce@edu.fit.ba",
                Name = "Ammar",
                Surname = "Puce",
                PhoneNumber = "+387 62 111 112",
                Address = "Masline-Kocine bb",
                IsAdmin = true,
                isUser = false,
                is2FActive = true,
                AdminLevel = "Moderator",
                ImageUrl = "UserImages/istockphoto-871752462-612x612.jpg",
                CityId = 18,
            };

            admin2.Password = _passwordHasher.HashPassword(admin2, "admin123");

            var user1 = new User
            {             
                Username = "TestUser1",
                Email = "testuser@example.com",
                Name = "Test",
                Surname = "User",
                PhoneNumber = "+387 62 111 113",
                Address = "useraddress",
                IsAdmin = false,
                isUser = true,
                is2FActive = false,
                CityId = 18,
                GenderId = 1,
                ImageUrl = "UserImages/istockphoto-871752462-612x612.jpg",
                
            };

            user1.Password = _passwordHasher.HashPassword(user1, "driveparts123");


            var user2 = new User
            {
                Username = "TestUser2",
                Email = "testuser2@example.com",
                Name = "Test2",
                Surname = "User2",
                PhoneNumber = "+387 62 111 114",
                Address = "useraddress2",
                IsAdmin = false,
                isUser = true,
                is2FActive = false,
                CityId = 16,
                GenderId = 2,
                ImageUrl = "UserImages/istockphoto-871752462-612x612.jpg",
            };

            user2.Password = _passwordHasher.HashPassword(user2, "driveparts123");

            await _db.AddAsync(admin1);
            await _db.AddAsync(admin2);
            await _db.AddAsync(user1);
            await _db.AddAsync(user2);

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
                new FAQ {
                    Question = "How can I find the right part for my vehicle?",
                    Answer = "You can easily find the right part by using our advanced search feature where you can filter parts by your vehicle's make, model, year, and engine type. If you're unsure, feel free to contact our support team and we’ll help you find exactly what you need."
                },
                new FAQ {
                    Question = "How can I track my order?",
                    Answer = "Once your order has been shipped, you will receive an email with a tracking number and a link to the courier's website where you can check the status and location of your package in real time."
                },
                new FAQ {
                    Question = "Do you offer refunds for defective parts?",
                    Answer = "Yes, we have a hassle-free return policy. If you receive a defective or damaged part, you can return it within 30 days of purchase. Simply contact our customer service team and provide your order details and a photo of the item."
                },
                new FAQ {
                    Question = "What payment methods are available?",
                    Answer = "We offer multiple secure payment methods including credit and debit cards, PayPal, bank transfer, and cash on delivery. All transactions are encrypted and your payment information is kept safe."
                },
                new FAQ {
                    Question = "How long does delivery take?",
                    Answer = "Delivery times vary depending on your location. Typically, orders are delivered within 3 to 5 business days. In more remote areas, delivery might take a little longer, but we’ll always provide you with tracking information."
                },
                new FAQ {
                    Question = "Can I return a part if it doesn’t fit my vehicle?",
                    Answer = "Yes, if the part you ordered doesn't fit and it is still unused and in its original packaging, you can return it within 15 days. Make sure to keep the invoice and notify us in advance so we can process your return smoothly."
                },
                new FAQ {
                    Question = "Do you offer discounts for bulk orders?",
                    Answer = "Yes, we offer special pricing and discounts for bulk orders, typically starting from purchases over 500 BAM. If you're a garage or reseller, feel free to reach out to us to discuss personalized offers and long-term cooperation."
                },
                new FAQ {
                    Question = "What should I do if I receive the wrong part?",
                    Answer = "If you receive the wrong item, please contact us immediately with your order number and a photo of the received item. We will arrange for a quick replacement or a full refund, depending on your preference."
                },
                new FAQ {
                    Question = "Is in-store pickup available?",
                    Answer = "Currently, we operate exclusively online to keep our prices competitive. All items are delivered directly to your address via our trusted courier partners, ensuring convenience and speed."
                },
                new FAQ {
                    Question = "Do you ship internationally?",
                    Answer = "At the moment, we only ship within Bosnia and Herzegovina. We are working on expanding our logistics network to include regional and international shipping in the near future."
                }
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

            var promoCodes = new List<PromoCode>
            {
                new PromoCode {Code = "DRIVEPARTS5" , Discount = 5},
                new PromoCode {Code = "DRIVEPARTS10" , Discount = 10},
                new PromoCode {Code = "DRIVEPARTS15" , Discount = 15}
            };

            await _db.PromoCodes.AddRangeAsync(promoCodes);
            await _db.SaveChangesAsync();

            return Ok("Admins, users, orders, order items, FAQ-s, review, promoCodes have been successfully added.");

            

        }
    }
}
