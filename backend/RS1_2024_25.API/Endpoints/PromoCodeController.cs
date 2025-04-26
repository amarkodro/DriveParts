using Microsoft.AspNetCore.Mvc;
using RS1_2024_25.API.Data;

namespace RS1_2024_25.API.Endpoints
{
    [ApiController]
    [Route("api/[controller]")]
    public class PromoCodeController(ApplicationDbContext _db) : ControllerBase
    {
        [HttpGet("check/{code}")]
        public IActionResult CheckPromoCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code)) return BadRequest("Promo code is required.");

            var promo = _db.PromoCodes.FirstOrDefault(x => x.Code.ToUpper() == code.ToUpper());


            if (promo == null) return BadRequest("Promo code not found");
            if (promo.Discount == null || promo.Discount <= 0) return BadRequest("Invalid discount value");

            return Ok(new
            {
                id = promo.Id,
                discount = promo.Discount,
            });

        }


    }
}
