using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Data.Models;
using RS1_2024_25.API.Endpoints;
using Xunit;

namespace RS1_2024_25.Tests
{
    public class PromoCodeControllerTests
    {
        private ApplicationDbContext CreateInMemoryDb()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new ApplicationDbContext(options);

            db.PromoCodes.Add(new PromoCode
            {
                Id = 1,
                Code = "SAVE10",
                Discount = 10
            });

            db.SaveChanges();
            return db;
        }

        [Fact]
        public void CheckPromoCode_WithValidCode_ReturnsOk()
        {
            // Arrange
            var db = CreateInMemoryDb();
            var controller = new PromoCodeController(db);

            // Act
            var result = controller.CheckPromoCode("SAVE10");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        }

        [Fact]
        public void CheckPromoCode_WithEmptyCode_ReturnsBadRequest()
        {
            // Arrange
            var db = CreateInMemoryDb();
            var controller = new PromoCodeController(db);

            // Act
            var result = controller.CheckPromoCode("");

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Promo code is required.", badRequestResult.Value);
        }
    }
}