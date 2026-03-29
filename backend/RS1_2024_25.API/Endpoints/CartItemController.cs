using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Data.Models;
namespace RS1_2024_25.API.Endpoints
{
    [ApiController]
    [Route("api/cart")]
    [Authorize]
    public class CartItemController(ApplicationDbContext _db) : ControllerBase
    {
        private int? GetCurrentUserId()
        {
            var claims = User.FindFirst("id")?.Value;
            if (string.IsNullOrEmpty(claims) || !int.TryParse(claims, out var userId))
                return null;
            return userId;
        }

        [HttpPost("add")]

        public IActionResult AddToCart([FromBody] CartItemRequests requests)
        {
            var userId = GetCurrentUserId();
            if (userId == null) return Unauthorized("Invalid token");

            var cartItem = new CartItem
            {
                UserId = userId.Value,
                PartId = requests.PartId,
                Quantity = requests.Quantity,
            };

            _db.CartItems.Add(cartItem);
            _db.SaveChanges();

            return Ok(new { message = "Item added to cart. " });
        }



        [HttpGet("getAll")]

        public IActionResult GetCart()
        {
            var userId = GetCurrentUserId();
            if (userId == null) return Unauthorized("Invalid token.");

            var items = _db.CartItems
                .Include(c => c.Part)
                .Where(c => c.UserId == userId)
                .ToList();

            var result = items
                .Where(c => c.Part != null)
                .Select(c => new CartItemResponse
                {
                    Id = c.Id,
                    PartId = c.PartId,
                    PartName = c.Part.Name,
                    Image = c.Part.PartImage,
                    Price = c.Part.Price,
                    Quantity = c.Quantity,
                    IsSavedForLater = c.IsSavedForLater
                });

            return Ok(result);
        }

        [HttpDelete("remove/{partId}")]

        public IActionResult RemoveFromCart(int partId)
        {
            var userId = GetCurrentUserId();
            if (userId == null) return Unauthorized("Invalid token.");

            var item = _db.CartItems.FirstOrDefault(c => c.UserId == userId && c.PartId == partId);
            if (item == null) return NotFound(new { message = "Item not fount in cart." });

            _db.CartItems.Remove(item);
            _db.SaveChanges();

            return Ok(new { message = "Item removed from cart." });
        }


        [HttpDelete("clear")]
        public IActionResult ClearCart()
        {
            var userId = GetCurrentUserId();
            if (userId == null) return Unauthorized("Invalid token.");

            var items = _db.CartItems.Where(c => c.UserId == userId).ToList();

            if (!items.Any()) return BadRequest(new { message = "Cart is already empty" });

            _db.CartItems.RemoveRange(items);
            _db.SaveChanges();

            return Ok(new { message = "Cart cleared successfully. " });

        }

        [HttpPut("update")]
        public IActionResult UpdateQuantity([FromBody] CartItemRequests requests)
        {

            var userId = GetCurrentUserId();
            if (userId == null) return Unauthorized("Invalid token.");

            var item = _db.CartItems.FirstOrDefault(c => c.UserId == userId && c.PartId == requests.PartId);
            if (item == null) return NotFound(new { message = "Item not found in cart." });

            item.Quantity = requests.Quantity;
            _db.SaveChanges();

            return Ok(new { message = "Quantity update successfully. " });
        }

        [HttpPut("{id}/move-to-cart")]
        public async Task<IActionResult> MoveToCart(int id)
        {
            var userId = GetCurrentUserId();
            if (userId == null) return Unauthorized("Invalid token.");

            var item = await _db.CartItems.FindAsync(id);
            if (item == null)
            {
                return NotFound(new { message = "Item not found in cart." });
            }

            if (item.UserId != userId)
                return Forbid();

            item.IsSavedForLater = false;
            await _db.SaveChangesAsync();

            return Ok(new { message = "Item moved to cart successfully." });
        }

        [HttpPut("{id}/save-to-later")]
        public async Task<IActionResult> SaveToLater(int id)
        {
            var userId = GetCurrentUserId();
            if (userId == null) return Unauthorized("Invalid token.");

            var item = await _db.CartItems.FindAsync(id);
            if (item == null)
            {
                return NotFound(new { message = "Item not found in cart." });
            }

            if (item.UserId != userId)
                return Forbid();

            item.IsSavedForLater = true;
            await _db.SaveChangesAsync();

            return Ok(new { message = "Item moved to cart successfully." });
        }


    }

    public class CartItemResponse
    {
        public int Id { get; set; }
        public int PartId { get; set; }
        public string PartName { get; set; }
        public string Image { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public bool IsSavedForLater { get; set; }
    }


    public class CartItemRequests
    {
        public int PartId { get; set; }
        public int Quantity { get; set; }
    }
}
