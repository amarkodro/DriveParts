using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Data.Models;
using static RS1_2024_25.API.Endpoints.CarsController;
using static RS1_2024_25.API.Endpoints.EnginesController;
using static RS1_2024_25.API.Endpoints.PartsController;

namespace RS1_2024_25.API.Endpoints
{
    [Route("api/contoller")]
    [ApiController]
    public class CarsEnginesController(ApplicationDbContext _db) : ControllerBase
    {


        public class CarsEnginesRequest
        {
            public int CarId { get; set; }
     
            public int EngineId { get; set; }

        }

        public class CarsEnginesResponse
        {
            public string CarName { get; set; }
            public string CarModel { get; set; }
            public string EngineName { get; set; }
        }

        [HttpGet]
        public ActionResult<CarsEnginesResponse[]> GetCarsEngines()
        {
            var carsengines = _db.CarEngines
                          .Include(p => p.Car)
                          .Include(p => p.Engine)
                          .Select(p => new CarsEnginesResponse
                          {
                              CarName = p.Car != null ? p.Car.Brand : "Unknown",
                              CarModel = p.Car != null ? p.Car.Model : "Unknown",
                              EngineName = p.Engine != null ? p.Engine.Name : "Unknown"

                          }).ToArray();

            return carsengines;
        }
        [HttpPost]
        public ActionResult<CarsEnginesResponse> PostCarEngine(CarsEnginesRequest request)
        {
            var car = _db.Cars.Find(request.CarId);
            var engine = _db.Engines.Find(request.EngineId);
            var carEngine = new CarEngine
            {
                CarId = request.CarId,
                EngineId = request.EngineId
            };

            _db.CarEngines.Add(carEngine);
            _db.SaveChanges();

            var response = new CarsEnginesResponse
            {
            
                CarName = car.Brand,
                CarModel = car.Model,
               
                EngineName = engine.Name
            };

            return Ok(response);
        }
        [HttpPut("{id}")]
        public ActionResult<string> PutCarEngine(int id, CarsEnginesRequest request)
        {
            var carEngine = _db.CarEngines.Find(id);        
            var car = _db.Cars.Find(request.CarId);          
            var engine = _db.Engines.Find(request.EngineId);
         
            carEngine.CarId = request.CarId;
            carEngine.EngineId = request.EngineId;
            _db.SaveChanges();
            return Ok("Car-Engine association updated successfully.");
        }

        // DELETE: api/CarsEngines/5
        [HttpDelete("{id}")]
        public ActionResult<string> DeleteCarEngine(int id)
        {
            var carEngine = _db.CarEngines.Find(id);
            if (carEngine == null)
                return NotFound("Car-Engine association not found.");

            _db.CarEngines.Remove(carEngine);
            _db.SaveChanges();

            return Ok("Car-Engine association deleted successfully.");
        }

    }
}
