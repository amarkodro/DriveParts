using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Data.Models;
using static RS1_2024_25.API.Endpoints.PartsController;

namespace RS1_2024_25.API.Endpoints
{
    [Route("api/parts")]
    [ApiController]
    public class PartsController(ApplicationDbContext _db) : ControllerBase
    {
        public class PartRequest
        {
            public string Name { get; set; }
            public double Price { get; set; }
            public string Description { get; set; }
            public int CategoryId { get; set; }
            public int ManufacturerId { get; set; }
            public IFormFile PartImage { get; set; }

        }

        public class PartResponse
        {
            public int PartId { get; set; }
            public string Name { get; set; }
            public double Price { get; set; }
            public string Description { get; set; }
            public string CategoryName { get; set; }
            public string ManufacturerName { get; set; }
            public string PartImage { get; set; }
        }

        [HttpGet]
        public ActionResult<PartResponse[]> GetParts()
        {
            var parts = _db.Parts
                          .Include(p => p.Category)
                          .Include(p => p.Manufacturer)
                          .Select(x => new PartResponse
                          {
                              PartId=x.PartId,
                              Name = x.Name,
                              Price = x.Price,
                              Description = x.Description,
                              CategoryName = x.Category != null ? x.Category.Name : "Unknown",
                              ManufacturerName = x.Manufacturer != null ? x.Manufacturer.Name : "Unknown",
                              PartImage = x.PartImage
                          }).ToArray();

            return parts;
        }

        [HttpGet("{id}")]
        public ActionResult<PartResponse> GetPart(int id)
        {
            var part = _db.Parts
                          .Include(c => c.Manufacturer).Include(c => c.Category)
                          .Where(c => c.PartId == id)
                          .Select(c => new PartResponse
                          {
                              Name = c.Name,
                              Price = c.Price,
                              Description = c.Description,
                              CategoryName = c.Category != null ? c.Category.Name : "Unknown",
                              ManufacturerName = c.Manufacturer != null ? c.Manufacturer.Name : "Unknown",
                              PartImage = c.PartImage
                          }).First();

            return part;
        }

        [HttpPost]
        public ActionResult<PartResponse> PostPart(PartRequest request)
        {
            string imageUrl = null;
            if (request.PartImage != null)
            {
                var fileName = Path.GetFileNameWithoutExtension(request.PartImage.FileName);
                var extension = Path.GetExtension(request.PartImage.FileName);
                var uniqueFileName = $"{fileName}_{Guid.NewGuid()}{extension}";
                var filePath = Path.Combine("wwwroot/images", uniqueFileName);
                using (var stream = new FileStream(filePath, FileMode.Create)) { request.PartImage.CopyToAsync(stream); }
                imageUrl = $"/images/{uniqueFileName}";
            }
            var part = new Part
            {
                Name = request.Name,
                Price = request.Price,
                Description = request.Description,
                CategoryId = request.CategoryId,
                ManufacturerId = request.ManufacturerId,
                PartImage = imageUrl
            };

            _db.Parts.Add(part);
            _db.SaveChanges();

            var response = new PartResponse
            {
                Name = part.Name,
                Price = part.Price,
                Description = part.Description,
                CategoryName = _db.Categories.Find(part.CategoryId)?.Name ?? "Unknown",
                ManufacturerName = _db.Manufacturers.Find(part.ManufacturerId)?.Name ?? "Unknown",
                PartImage = part.PartImage
            };

            return Ok(response);
        }

        [HttpPut("{id}")]
        public ActionResult<string> PutPart(int id, PartRequest request)
        {
            var part = _db.Parts.Find(id) ?? throw new KeyNotFoundException("Part not found");
            if (request.PartImage != null)
            {
                var fileName = Path.GetFileNameWithoutExtension(request.PartImage.FileName);
                var extension = Path.GetExtension(request.PartImage.FileName);
                var uniqueFileName = $"{fileName}_{Guid.NewGuid()}{extension}";
                var filePath = Path.Combine("wwwroot/images", uniqueFileName);
                using (var stream = new FileStream(filePath, FileMode.Create)) { request.PartImage.CopyTo(stream); }
                part.PartImage = $"/images/{uniqueFileName}"; // Spremi URL slike kao string }
            }
            part.Name = request.Name;
            part.Price = request.Price;
            part.Description = request.Description;
            part.CategoryId = request.CategoryId;
            part.ManufacturerId = request.ManufacturerId;


            _db.SaveChanges();

            return Ok("Part updated successfully");
        }

        [HttpDelete("{id}")]
        public ActionResult<string> DeletePart(int id)
        {
            var part = _db.Parts.Find(id) ?? throw new KeyNotFoundException("Part not found");
            _db.Parts.Remove(part);
            _db.SaveChanges();

            return Ok("Part deleted successfully");
        }

        [HttpGet("featured")]
        public ActionResult<PartResponse[]> GetFeaturedParts()
        {
            var parts = _db.Parts
                         .Include(c => c.Manufacturer).Include(c => c.Category)
                         .Where(c => c.IsFeatured == true)
                         .Select(c => new PartResponse
                         {
                             Name = c.Name,
                             Price = c.Price,
                             Description = c.Description,
                             CategoryName = c.Category != null ? c.Category.Name : "Unknown",
                             ManufacturerName = c.Manufacturer != null ? c.Manufacturer.Name : "Unknown",
                             PartImage = c.PartImage
                         }).ToArray();

            return parts;
        }

        [HttpGet("newArrival")]
        public ActionResult<PartResponse[]> GetNewArrivalParts()
        {
            var parts = _db.Parts
                         .Include(c => c.Manufacturer).Include(c => c.Category)
                         .Where(c => c.IsNewArrival == true)
                         .Select(c => new PartResponse
                         {
                             Name = c.Name,
                             Price = c.Price,
                             Description = c.Description,
                             CategoryName = c.Category != null ? c.Category.Name : "Unknown",
                             ManufacturerName = c.Manufacturer != null ? c.Manufacturer.Name : "Unknown",
                             PartImage = c.PartImage
                         }).ToArray();

            return parts;
        }

        [HttpGet("onSale")]
        public ActionResult<PartResponse[]> GetOnSaleParts()
        {
            var parts = _db.Parts
                         .Include(c => c.Manufacturer).Include(c => c.Category)
                         .Where(c => c.IsOnSale == true)
                         .Select(c => new PartResponse
                         {
                             Name = c.Name,
                             Price = c.Price,
                             Description = c.Description,
                             CategoryName = c.Category != null ? c.Category.Name : "Unknown",
                             ManufacturerName = c.Manufacturer != null ? c.Manufacturer.Name : "Unknown",
                             PartImage = c.PartImage
                         }).ToArray();

            return parts;
        }

    }
}