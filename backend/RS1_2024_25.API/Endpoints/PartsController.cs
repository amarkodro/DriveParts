using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Data.Models;

namespace RS1_2024_25.API.Endpoints
{
    [Route("api/controller")]
    [ApiController]
    public class PartController(ApplicationDbContext _db) : ControllerBase
    {
        public class PartRequest
        {
            public string Name { get; set; }
            public double Price { get; set; }
            public string Description { get; set; }
            public int CategoryId { get; set; }
            public int CarId { get; set; }
            public int ManufacturerId { get; set; }

        }

        public class PartResponse
        {
            public string Name { get; set; }
            public double Price { get; set; }
            public string Description { get; set; }
            public string CategoryName { get; set; }
            public string CarName { get; set; }
            public string ManufacturerName { get; set; }
        }

        [HttpGet]
        public ActionResult<PartResponse[]> GetParts()
        {
            var parts = _db.Parts
                          .Include(p => p.Category)
                          .Include(p => p.Manufacturer)
                          .Include(p => p.Car).Select(x => new PartResponse
                          {
                              Name = x.Name,
                              Price = x.Price,
                              Description = x.Description,
                              CategoryName = x.Category != null ? x.Category.Name : "Unknown",
                              CarName = x.Car != null ? x.Car.Brand : "Unknown",
                              ManufacturerName = x.Manufacturer != null ? x.Manufacturer.Name : "Unknown",
                          }).ToArray();

            return parts;
        }

        [HttpGet("{id}")]
        public ActionResult<PartResponse> GetPart(int id)
        {
            var part = _db.Parts
                          .Include(c => c.Car).Include(c => c.Manufacturer).Include(c => c.Category)
                          .Where(c => c.PartId == id)
                          .Select(c => new PartResponse
                          {
                              Name = c.Name,
                              Price = c.Price,
                              Description = c.Description,
                              CategoryName = c.Category != null ? c.Category.Name : "Unknown",
                              CarName = c.Car != null ? c.Car.Brand : "Unknown",
                              ManufacturerName = c.Manufacturer != null ? c.Manufacturer.Name : "Unknown",
                          }).First();

            return part;
        }

        [HttpPost]
        public ActionResult<PartResponse> PostPart(PartRequest request)
        {
            var part = new Part
            {
                Name = request.Name,
                Price = request.Price,
                Description = request.Description,
                CategoryId = request.CategoryId,
                CarId = request.CarId,
                ManufacturerId = request.ManufacturerId,
            };

            _db.Parts.Add(part);
            _db.SaveChanges();

            var response = new PartResponse
            {
                Name = part.Name,
                Price = part.Price,
                Description = part.Description,
                CategoryName = _db.Categories.Find(part.CategoryId)?.Name ?? "Unknown",
                CarName = _db.Cars.Find(part.CarId)?.Brand ?? "Unknown",
                ManufacturerName = _db.Manufacturers.Find(part.ManufacturerId)?.Name ?? "Unknown",
            };

            return Ok(response);
        }

        [HttpPut("{id}")]
        public ActionResult<string> PutPart(string id,PartRequest request)
        {
             var part = _db.Parts.Find(id) ?? throw new KeyNotFoundException("Part not found");

            part.Name = request.Name;
            part.Price = request.Price;
            part.Description = request.Description;
            part.CategoryId = request.CategoryId;
            part.CarId = request.CarId;
            part.ManufacturerId = request.ManufacturerId;

            _db.SaveChanges();

            return Ok("User updated successfully");
        }

        [HttpDelete("{id}")]
        public ActionResult<string> DeletePart(string id)
        {
            var part = _db.Parts.Find(id) ?? throw new KeyNotFoundException("Part not found");

            _db.Parts.Remove(part);
            _db.SaveChanges();

            return Ok("Part deleted successfully");
        }
    }
}