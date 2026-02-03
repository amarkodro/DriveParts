using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Data.Models;
using System.Runtime.InteropServices;

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
            public int? ModelId { get; set; }
            public int? TypeId { get; set; }
        }

        public class FilterResponse
        {
            public int Id { get; set; }
            public int PartId { get; set; }
            public string Name { get; set; }
            public double Price { get; set; }
            public string Description { get; set; }
            public string CategoryName { get; set; }
            public string ManufacturerName { get; set; }
            public string CarName { get; set; }
            public string ModelName { get; set; }
            public string TypeName { get; set; }
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
                Name = $"{x.Brand}",
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

        [HttpGet("models")]
        public ActionResult<FilterDataResponse> GetModelsName([FromQuery] int carId)
        {
            var models = _db.Models.Where(x=>x.CarId==carId).Select(x => new FilterDataResponse
            {
                Id=x.ModelId,
                Name = x.Name,
            });

            return Ok(models);
        }

        [HttpGet("types")]
        public ActionResult<FilterDataResponse> GetTypesName()
        {
            var types = _db.Types.Select(x => new FilterDataResponse
            {
                Id = x.TypeId,
                Name = x.Name,
            });

            return Ok(types);
        }

        [HttpGet("filter")]
        public ActionResult<FilterResponse[]> FilterParts(int? carId, int? categoryId, int? partId, int? modelId, int? typeId)
        {

            var parts = _db.ModelParts
                         .Include(x => x.Part)
                         .Include(x => x.Model)
                         .Include(x => x.Model).ThenInclude(x => x.Car)
                         .Include(x => x.Part).ThenInclude(x => x.Type).Where(x =>
                            (categoryId == null || x.Part.CategoryId == categoryId) &&
                            (partId == null || x.PartId == partId) &&
                            (modelId == null || x.ModelId == modelId) &&
                            (typeId == null || x.Part.TypeId== typeId) &&
                            (carId==null || x.Model.CarId==carId))
                        .Select(x => new FilterResponse
                        {
                            Id = x.PartId,
                            PartId = x.PartId,
                            Name = x.Part.Name,
                            Price = x.Part.Price,
                            Description = x.Part.Description,
                            CategoryName = x.Part.Category.Name,
                            ManufacturerName = x.Part.Manufacturer.Name,
                            CarName = x.Model.Car.Brand,
                            ModelName = x.Model.Name,
                            TypeName = x.Part.Type.Name,
                            PartImage = x.Part.PartImage,

                        }).ToArray();

            if (!parts.Any())
            {
                return NotFound(new { Message = "No parts found for the given filters." });
            }


            return Ok(parts);
        }


    }
}