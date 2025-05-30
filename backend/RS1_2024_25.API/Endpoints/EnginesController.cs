using Microsoft.AspNetCore.Mvc;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Data.Models;
using Microsoft.Identity.Client;
using static RS1_2024_25.API.Endpoints.CarsController;

namespace RS1_2024_25.API.Endpoints
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnginesController(ApplicationDbContext _db):ControllerBase
    {
        public class EnginesRequest
        {
            public string Name { get; set; }
            public int Power { get; set; }
            public double Displacement { get; set; }
            public string FuelType { get; set; }
        }
        public class EnginesResponse
        {
            public int EngineId { get; set; }
            public string Name { get; set; }
            public int Power { get; set; }
            public double Displacement { get; set; }
            public string FuelType { get; set; }
        }

        [HttpGet]
        public ActionResult<EnginesResponse[]> GetEngines()
        {
            var engines = _db.Engines.Select(x => new EnginesResponse
            {
                EngineId = x.EngineId,
                Name = x.Name,
                Power = x.Power,
                Displacement = x.Displacement,
                FuelType = x.FuelType,
            }).ToArray();
            return engines;
        }

        [HttpGet("{id}")]
        public ActionResult<EnginesResponse> GetEngine(int id)
        {
            var engine = _db.Engines.Where(x => x.EngineId == id).Select(x => new EnginesResponse
            {
                EngineId = x.EngineId,
                Name = x.Name,
                Power = x.Power,
                Displacement = x.Displacement,
                FuelType = x.FuelType,

            }).First();
            return engine;
        }

        [HttpPost]
        public ActionResult<EnginesResponse> PostEngine(EnginesRequest request)
        {
            var engine = new Engine
            {
                Name = request.Name,
                Power = request.Power,
                Displacement = request.Displacement,
                FuelType = request.FuelType,
            };
            _db.Engines.Add(engine);
            _db.SaveChanges();

            var response = new EnginesResponse
            {
                Name = engine.Name,
                Power = engine.Power,
                Displacement = engine.Displacement,
                FuelType = engine.FuelType,
            };
            return Ok(response);
        }

        [HttpPut("{id}")]
        public ActionResult<string> PutEngine(int id, EnginesRequest request)
        {
            var engine = _db.Engines.Find(id) ?? throw new KeyNotFoundException("Engine not found");

            engine.Name = request.Name;
            engine.Power = request.Power;
            engine.Displacement = request.Displacement;
            engine.FuelType = request.FuelType;

            _db.SaveChanges();
            return Ok("Engine updated successfully");

        }

        [HttpDelete("{id}")]

        public ActionResult<string> DeleteEngine(int id)
        {
            var engine= _db.Engines.Find(id)?? throw new KeyNotFoundException("Engine not found");

            _db.Engines.Remove(engine);
            _db.SaveChanges();
            return Ok("Engine successfully deleted");
        }

    }
}
