using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data;
using static RS1_2024_25.API.Endpoints.EnginesController;

namespace RS1_2024_25.API.Endpoints
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController(ApplicationDbContext _db) : ControllerBase
    {
        public class CitesResponse
        {
            public int ID { get; set; }
            public string? Name { get; set; }
            public long? PostalCode { get; set; }
            public string? CountryName { get; set; }
        }

        [HttpGet()]
        public ActionResult<CitesResponse[]> GetCities()
        {
            var cities = _db.Cities.Include(x=>x.Country).Select(x => new CitesResponse
            {
                ID = x.ID,
                Name = x.Name,
                PostalCode = x.PostalCode,
                CountryName = x.Country.Name,
            }).ToArray();
            return Ok(cities);
        }

        [HttpGet("{id}")]
        public IActionResult GetCityWithCounty(int id)
        {
            var city = _db.Cities
                .Where(x => x.ID == id)
                .Select(x => new
                {
                    x.ID,
                    x.Name,
                    x.PostalCode,
                    CountryName = x.Country.Name
                }).FirstOrDefault();

            if(city == null) return NotFound();
            

            return Ok(city);
        }
    }
}
