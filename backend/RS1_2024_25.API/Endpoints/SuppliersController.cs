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
    public class SuppliersController(ApplicationDbContext _db):ControllerBase
    {
        public class SupplierRequest
        {
            public string Name { get; set; }
            public string Contact { get; set; }
            public string Address { get; set; }
        }
        public class SupplierResponse
        {
            public int SupplierId { get; set; }
            public string Name { get; set; }
            public string Contact { get; set; }
            public string Address { get; set; }
        }

        [HttpGet("all")]
        public ActionResult<SupplierResponse[]> GetSupplier()
        {
            var supplier = _db.Suppliers
                        .Select(x => new SupplierResponse
                        {
                            SupplierId = x.SupplierId,
                            Name = x.Name,
                            Contact = x.Contact,
                            Address = x.Address

                        }).ToArray();

            return supplier;
        }

        [HttpGet("{id}")]
        public ActionResult<SupplierResponse> GetSupplier(int id)
        {
            var supplier = _db.Suppliers
                       .Where(x => x.SupplierId == id)
                       .Select(x => new SupplierResponse
                       {
                           SupplierId = x.SupplierId,
                           Name = x.Name,
                           Contact = x.Contact,
                           Address = x.Address  
                       }).First();

            return supplier;
        }

        
        [HttpPost]
        public ActionResult<SupplierResponse> PostSupplier(SupplierRequest request)
        {
            var supplier = new Supplier
            {
                Name = request.Name,
                Contact = request.Contact,
                Address = request.Address

            };

            _db.Suppliers.Add(supplier);
            _db.SaveChanges();

            var response = new SupplierResponse
            {
                Name= supplier.Name,
                Contact = supplier.Contact,
                Address = supplier.Address


            };

            return Ok(response);
        }

     
        [HttpPut("{id}")]
        public ActionResult<string> PatchSupplier(int id, SupplierRequest request)
        {
            var supplier = _db.Suppliers.Find(id) ?? throw new KeyNotFoundException("Supplier not found");

            supplier.Name = request.Name;
            supplier.Contact = request.Contact;
            supplier.Address = request.Address;

            _db.SaveChanges();

            return Ok("Supplier updated successfully");
        }


        [HttpDelete("{id}")]

        public ActionResult<string> DeleteSupplier(int id)
        {
            var supplier = _db.Suppliers.Find(id) ?? throw new KeyNotFoundException("Supplier not found");

            _db.Suppliers.Remove(supplier);
            _db.SaveChanges();

            return Ok("Supplier deleted successfully");
        }
    }
    
}
