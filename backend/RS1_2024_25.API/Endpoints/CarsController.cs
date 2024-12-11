using Microsoft.AspNetCore.Mvc;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace RS1_2024_25.API.Endpoints
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController(ApplicationDbContext _db) : ControllerBase
    {
        public class CarRequest
        {
            public string Model { get; set; }
            public string Brand { get; set; }
            public string Type { get; set; }
            public int Year { get; set; }
        }

        public class CarResponse
        {
            public int CarId { get; set; }
            public string Model { get; set; }
            public string Brand { get; set; }
            public string Type { get; set; }
            public int Year { get; set; }

        }

        //GET: api/Car
        [HttpGet]
        public ActionResult<CarResponse[]> GetCar()
        {
            var cars = _db.Cars
                        .Select(x => new CarResponse
                        {
                            CarId = x.CarId,
                            Model = x.Model,
                            Brand = x.Brand,
                            Type = x.Type,
                            Year = x.Year,
                        }).ToArray();

            return cars;
        }

        //GET: api/Car/5

        [HttpGet("{id}")]
        public ActionResult<CarResponse> GetCar(int id)
        {
            var car = _db.Cars
                       .Where(x => x.CarId == id)
                       .Select(x => new CarResponse
                       {
                           CarId = x.CarId,
                           Model = x.Model,
                           Brand = x.Brand,
                           Type = x.Type,
                           Year = x.Year
                       }).First();

            return car;
        }

        //POST: api/Car
        [HttpPost]
        public ActionResult<CarResponse> PostCar(CarRequest request)
        {
            var car = new Car
            {
                Model = request.Model,
                Brand = request.Brand,
                Type = request.Type,
                Year = request.Year,
            };

            _db.Cars.Add(car);
            _db.SaveChanges();

            var response = new CarResponse
            {
                Model = car.Model,
                Brand = car.Brand,
                Type = car.Type,
                Year = car.Year
            };

            return Ok(response);
        }

        //PUT: api/Car/5
        [HttpPut("{id}")]
        public ActionResult<string> PatchCar(int id, CarRequest request)
        {
            var car = _db.Cars.Find(id) ?? throw new KeyNotFoundException("Car not found");

            car.Model = request.Model;
            car.Brand = request.Brand;
            car.Type = request.Type;
            car.Year = request.Year;
            
            _db.SaveChanges();

            return Ok("Car updated successfully");
        }


        [HttpDelete("{id}")]

        public ActionResult<string> DeleteCar(int id)
        {
            var car = _db.Cars.Find(id) ?? throw new KeyNotFoundException("Car not found");

            _db.Cars.Remove(car);
            _db.SaveChanges();

            return Ok("Car deleted successfully");
        }
    }
}
