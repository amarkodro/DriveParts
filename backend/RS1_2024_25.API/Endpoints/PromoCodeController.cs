using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RS1_2024_25.API.Data;

namespace RS1_2024_25.API.Endpoints
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PromoCodeController(ApplicationDbContext _db) : ControllerBase
    {

        private int? GetCurrentUserId()
        {
            var claims = User.FindFirst("id")?.Value;
            if (string.IsNullOrEmpty(claims) || !int.TryParse(claims, out var userId))
                return null;
            return userId;
        }

        [HttpGet("check/{code}")]
        public IActionResult CheckPromoCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code)) return BadRequest("Promo code is required.");

            var userId = GetCurrentUserId();
            if (userId == null) return Unauthorized("Invalid token.");
            var promo = _db.PromoCodes.FirstOrDefault(x => x.Code.ToUpper() == code.ToUpper());


            if (promo == null) return BadRequest("Promo code not found");
            if (promo.Discount == null || promo.Discount <= 0) return BadRequest("Invalid discount value");

            var alreadyUsed = _db.PromoCodeUsages.Any(x => x.PromoCodeId == promo.Id && x.UserId == userId.Value);

            if (alreadyUsed)
                return BadRequest("You have already used this promo code.");

            return Ok(new
            {
                id = promo.Id,
                discount = promo.Discount,
            });

        }


    }
}
