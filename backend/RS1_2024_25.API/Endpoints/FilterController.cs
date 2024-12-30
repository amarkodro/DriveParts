using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data;

namespace RS1_2024_25.API.Endpoints
{
    [Route("api/filter")]
    [ApiController]
    public class FilterController(ApplicationDbContext _db) : ControllerBase
    {
        public class FilterRequest
        {
            public int? CarId { get; set; }
            public int? CategoryId { get; set; }
            public int? PartId { get; set; }
            public int? BrandId { get; set; }
        }

        public class FilterResponse
        {
            public int PartId { get; set; }
            public string Name { get; set; }
            public double Price { get; set; }
            public string Description { get; set; }
            public string CategoryName { get; set; }
            public string ManufacturerName { get; set; }
            public string CarName { get; set; }
            public string PartImage { get; set; }
        }

        public class FilterDataResponse()
        {
            public int Id { get; set; }
            public string Name { get; set; }

        }

        [HttpGet("cars")]
        public ActionResult<FilterDataResponse> GetCarNames()
        {
            var cars = _db.Cars.Select(x => new FilterDataResponse
            {
                Id = x.CarId,
                Name = $"{x.Brand} {x.Model}",
            }).ToList();

            return Ok(cars);

        }

        [HttpGet("parts")]
        public ActionResult<FilterDataResponse> GetPartNames()
        {
            var parts = _db.Parts.Select(x => new FilterDataResponse
            {
                Id = x.PartId,
                Name = x.Name,
            }).ToList();

            return Ok(parts);
        }

        [HttpGet("manufacturers")]
        public ActionResult<FilterDataResponse> GetManufacturerNames()
        {
            var manufacturer = _db.Manufacturers.Select(x => new FilterDataResponse
            {
                Id = x.ManufacturerId,
                Name = x.Name,
            }).ToList();

            return Ok(manufacturer);
        }

        [HttpGet("categories")]
        public ActionResult<FilterDataResponse> GetCategoryNames()
        {
            var category = _db.Categories.Select(x => new FilterDataResponse
            {
                Id = x.CategoryId,
                Name = x.Name,
            }).ToList();

            return Ok(category);
        }


        [HttpGet("filter")]
        public ActionResult<FilterResponse[]> FilterParts(int? carId, int? categoryId, int? partId, int? manufacturerId)
        {
            var parts = _db.CarParts
                .Include(x => x.Part)
                .ThenInclude(x => x.Manufacturer)
                .Include(x => x.Part)
                .ThenInclude(x => x.Category)
                .Include(x => x.Car)
                .Where(x =>
                    (carId == null || x.CarId == carId) &&
                    (categoryId == null || x.Part.CategoryId == categoryId) &&
                    (partId == null || x.Part.PartId == partId) &&
                    (manufacturerId == null || x.Part.ManufacturerId == manufacturerId))
                .Select(x => new FilterResponse
                {
                    PartId = x.Part.PartId,
                    Name = x.Part.Name,
                    Price = x.Part.Price,
                    Description = x.Part.Description,
                    CategoryName = x.Part.Category.Name,
                    ManufacturerName = x.Part.Manufacturer.Name,
                    CarName = x.Car.Brand + " " + x.Car.Model,
                    PartImage = x.Part.PartImage
                })
                .ToList();

            return Ok(parts);
        }

    }
}
