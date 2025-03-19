using Microsoft.AspNetCore.Mvc;
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
            public string Name { get; set; }
        }

        [HttpGet()]
        public ActionResult<CitesResponse[]> GetCities()
        {
            var cities = _db.Cities.Select(x => new CitesResponse
            {
                ID = x.ID,
                Name = x.Name,
            }).ToArray();
            return Ok(cities);
        }


    }
}
