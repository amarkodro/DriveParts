using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Data.Models;
using System.ComponentModel.DataAnnotations;
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
            public bool IsFeatured { get; set; }
            public bool IsOnSale { get; set; }
            public bool IsNewArrival { get; set; } 
            public int TypeId { get; set; }

        }

        public class PartResponse
        {
            public int PartId { get; set; }
            public string Name { get; set; }
            public double Price { get; set; }
            public string Description { get; set; }
            public int CategoryId { get; set; }
            public int ManufacturerId { get; set; }
            public string CategoryName { get; set; }
            public string ManufacturerName { get; set; }
            public string PartImage { get; set; }
            public bool IsFeatured { get; set; }
            public bool IsOnSale { get; set; }
            public bool IsNewArrival { get; set; }
            public int? TypeId { get; set; }
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
                              PartImage = x.PartImage,
                              IsNewArrival = x.IsNewArrival,
                              IsOnSale = x.IsOnSale,
                              IsFeatured = x.IsFeatured
                          }).ToArray();

            return parts;
        }

        [HttpGet("{id}")]
        public ActionResult<PartResponse> GetPart(int id)
        {
            var part = _db.Parts
       .Include(c => c.Manufacturer)
       .Include(c => c.Category)
       .First(c => c.PartId == id);
            return new PartResponse
            {
                PartId = part.PartId,
                Name = part.Name,
                Price = part.Price,
                Description = part.Description,
                CategoryId = part.CategoryId,
                ManufacturerId = part.ManufacturerId,
                CategoryName = part.Category != null ? part.Category.Name : "Unknown",
                ManufacturerName = part.Manufacturer != null ? part.Manufacturer.Name : "Unknown",
                PartImage =part.PartImage, // Add full URL
                IsNewArrival = part.IsNewArrival,
                IsOnSale = part.IsOnSale,
                IsFeatured = part.IsFeatured
            };

          
           
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
                    PartImage = imageUrl,
                    IsOnSale = request.IsOnSale,
                    IsFeatured = request.IsFeatured,
                    IsNewArrival = request.IsNewArrival,
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
                PartImage = part.PartImage,
                IsOnSale=part.IsOnSale,
                IsNewArrival=part.IsNewArrival,
                IsFeatured=part.IsFeatured
            };

            return Ok(response);
        }

        [HttpPut("{id}")]
        public ActionResult<string> PutPart(int id, [FromForm] PartUpdateRequest request)
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
            else if (!string.IsNullOrEmpty(request.ExistingImagePath))
            {
                part.PartImage = request.ExistingImagePath;
            }
            part.Name = request.Name;
            part.Price = request.Price;
            part.Description = request.Description;
            part.CategoryId = request.CategoryId;
            part.ManufacturerId = request.ManufacturerId;
            part.IsFeatured = request.IsFeatured;
            part.IsOnSale = request.IsOnSale;
            part.IsNewArrival = request.IsNewArrival;

            _db.SaveChanges();

            return Ok(new { message = "Part updated successfully" });
        }
        public class PartUpdateRequest
        {
            public string Name { get; set; }
            public double Price { get; set; }
            public string Description { get; set; }
            public int CategoryId { get; set; }
            public int ManufacturerId { get; set; }
            public IFormFile? PartImage { get; set; } // Make nullable
            public bool IsFeatured { get; set; }
            public bool IsOnSale { get; set; }
            public bool IsNewArrival { get; set; }

            [RequiredIfNoNewImage]
            public string? ExistingImagePath { get; set; }
        }
        public class RequiredIfNoNewImageAttribute : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext context)
            {
                var model = (PartUpdateRequest)context.ObjectInstance;
                if (model.PartImage == null && string.IsNullOrEmpty(value?.ToString()))
                {
                    return new ValidationResult("Existing image path is required when no new image is uploaded");
                }
                return ValidationResult.Success;
            }
        }
        [HttpDelete("{id}")]
        public ActionResult<string> DeletePart(int id)
        {
            var part = _db.Parts.Find(id) ?? throw new KeyNotFoundException("Part not found");
            _db.Parts.Remove(part);
            _db.SaveChanges();

            return Ok(new { message = "Part deleted successfully" });
        }

        [HttpGet("featured")]
        public ActionResult<PartResponse[]> GetFeaturedParts()
        {
            var parts = _db.Parts
                         .Include(c => c.Manufacturer).Include(c => c.Category)
                         .Where(c => c.IsFeatured == true)
                         .Select(c => new PartResponse
                         {
                             PartId=c.PartId,
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
                             PartId = c.PartId,
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
                             PartId = c.PartId,
                             Name = c.Name,
                             Price = c.Price,
                             Description = c.Description,
                             CategoryName = c.Category != null ? c.Category.Name : "Unknown",
                             ManufacturerName = c.Manufacturer != null ? c.Manufacturer.Name : "Unknown",
                             PartImage = c.PartImage
                         }).ToArray();

            return parts;
        }

        public class AddToCartRequest
        {
            public int ProductId { get; set; }
            public int Quantity { get; set; }
        }

       


    }
}