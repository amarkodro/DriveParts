using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Data.Models;
using System.Security.Claims;
using static RS1_2024_25.API.Endpoints.PartsController;

namespace RS1_2024_25.API.Endpoints
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FavoritesController(ApplicationDbContext _db) : ControllerBase
    {
        [HttpPost("toggle/{partId}")]
        public async Task<IActionResult> ToggleFavorite(int partId)
        {
            var userId = int.Parse(User.FindFirst("id")?.Value ?? "0");
            if (userId == 0) return Unauthorized();

            var existing = await _db.MyParts
                .FirstOrDefaultAsync(mp => mp.UserId == userId && mp.PartId == partId);

            if (existing != null)
            {
                _db.MyParts.Remove(existing);
                await _db.SaveChangesAsync();
                return Ok(new { isFavorite = false });
            }
            else
            {
                var myPart = new MyPart
                {
                    UserId = userId,
                    PartId = partId,
                    DateAdded = DateTime.UtcNow
                };
                _db.MyParts.Add(myPart);
                await _db.SaveChangesAsync();
                return Ok(new { isFavorite = true });
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<PartResponse>>> GetFavorites()
        {
            var userId = int.Parse(User.FindFirst("id")?.Value ?? "0");
            if (userId == 0) return Unauthorized();

            var parts = await _db.MyParts
                .Where(mp => mp.UserId == userId)
                .Include(mp => mp.Part)
                .ThenInclude(p => p.Category)
                .Include(mp => mp.Part)
                .ThenInclude(p => p.Manufacturer)
                .Select(mp => mp.Part)
                .Select(x => new PartResponse
                {
                    PartId = x.PartId,
                    Name = x.Name,
                    Price = x.Price,
                    Description = x.Description,
                    CategoryId = x.CategoryId,
                    ManufacturerId = x.ManufacturerId,
                    CategoryName = x.Category != null ? x.Category.Name : "Unknown",
                    ManufacturerName = x.Manufacturer != null ? x.Manufacturer.Name : "Unknown",
                    PartImage = x.PartImage,
                    IsNewArrival = x.IsNewArrival,
                    IsOnSale = x.IsOnSale,
                    IsFeatured = x.IsFeatured
                })
                .ToListAsync();

            return Ok(parts);
        }

        [HttpGet("ids")]
        public async Task<ActionResult<List<int>>> GetFavoriteIds()
        {
            var userId = int.Parse(User.FindFirst("id")?.Value ?? "0");
            if (userId == 0) return Unauthorized();

            var ids = await _db.MyParts
                .Where(mp => mp.UserId == userId)
                .Select(mp => mp.PartId)
                .ToListAsync();

            return Ok(ids);
        }
    }
}
