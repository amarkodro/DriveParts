using Microsoft.AspNetCore.Mvc;
using RS1_2024_25.API.Data;

namespace RS1_2024_25.API.Endpoints
{
    [Route("api/[controller]")]
    public class GenderController(ApplicationDbContext _db) : ControllerBase
    {


        public class GenderResponse
        {
            public int Id { get; set; }
            public string GenderName { get; set; }
        }


        [HttpGet]
        public IActionResult GetGenders()
        {
            var genders = _db.Genders.Select(x => new GenderResponse {
            
                Id=x.GenderId,
                GenderName=x.GenderName,
            }).ToList();

            return Ok(genders);
        }

    }
}
