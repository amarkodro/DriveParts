using Microsoft.AspNetCore.Mvc;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using static RS1_2024_25.API.Endpoints.CarsController;
namespace RS1_2024_25.API.Endpoints
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManufacturersController(ApplicationDbContext _db) : ControllerBase
    {
        public class ManufacturerRequest
        {
            public string Name { get; set; }
            public string Contact { get; set; }
            public string Address { get; set; }
        }
        public class ManufacturerResponse
        {
            public int ManufacturerId { get; set; }
            public string Name { get; set; }
            public string Contact { get; set; }
            public string Address { get; set; }
        }
        [HttpGet]
        public ActionResult<ManufacturerResponse[]> GetManufacturer()
        {
            var manufacturer = _db.Manufacturers
                        .Select(x => new ManufacturerResponse 
                        {
                            ManufacturerId = x.ManufacturerId,
                            Name = x.Name,
                            Contact = x.Contact,
                            Address = x.Address
                        }).ToArray();

            return manufacturer;
        }

        [HttpGet("{id}")]
        public ActionResult<ManufacturerResponse> GetManufacturer(int id)
        {
            var manufacturer = _db.Manufacturers
                       .Where(x => x.ManufacturerId == id)
                       .Select(x => new ManufacturerResponse
                       {
                           ManufacturerId = x.ManufacturerId,
                           Name = x.Name,
                           Contact = x.Contact,
                           Address = x.Address

                       }).First();

            return manufacturer;
        }
        [HttpPost]
        public ActionResult<ManufacturerResponse> PostManufacturer(ManufacturerRequest request)
        {
            var manufacturer = new Manufacturer
            {
                Name = request.Name,
                Contact = request.Contact,
                Address = request.Address
            };

            _db.Manufacturers.Add(manufacturer);
            _db.SaveChanges();

            var response = new ManufacturerResponse
            {
                Name=manufacturer.Name,
                Contact=manufacturer.Contact,
                Address=manufacturer.Address

            };

            return Ok(response);
        }

        [HttpPut("{id}")]
        public ActionResult<string> PatchManufacturer(int id, ManufacturerRequest request)
        {
            var manufacturer = _db.Manufacturers.Find(id) ?? throw new KeyNotFoundException("Manufacturer not found");

            manufacturer.Name = request.Name;
            manufacturer.Contact = request.Contact;
            manufacturer.Address = request.Address;


            _db.SaveChanges();

            return Ok("Manufacturer updated successfully");
        }
        [HttpDelete("{id}")]

        public ActionResult<string> DeleteManufacturer(int id)
        {
            var manufacturer = _db.Manufacturers.Find(id) ?? throw new KeyNotFoundException("Manufacturer not found");

            _db.Manufacturers.Remove(manufacturer);
            _db.SaveChanges();

            return Ok("Manufacturer deleted successfully");
        }
    }
}
