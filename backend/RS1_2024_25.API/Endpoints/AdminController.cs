using RS1_2024_25.API.Data;
using RS1_2024_25.API.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Net;
using static RS1_2024_25.API.Endpoints.UsersController;

namespace RS1_2024_25.API.Endpoints
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController(ApplicationDbContext _db) : ControllerBase
    {
        public class AdminRequest
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }
            public string Address { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
            public bool IsAdmin { get; set; }
            public bool isUser { get; set; }
            public bool is2FActive { get; set; }
            public string AdminLevel { get; set; }

        }
        public class AdminResponse
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }
            public string Address { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
            public bool IsAdmin { get; set; }
            public bool isUser { get; set; }
            public bool is2FActive { get; set; }
            public string AdminLevel { get; set; }

        }
        [HttpGet]
        public ActionResult<AdminResponse[]> GetAdmins()
        {
            var admins = _db.Admins
                   .Select(c => new AdminResponse
                   {
                       Id = c.Id,
                       Name = c.Name,
                       Surname = c.Surname,
                       Email = c.Email,
                       PhoneNumber = c.PhoneNumber,
                       Address = c.Address,
                       Username = c.Username,
                       Password = c.Password,
                       IsAdmin = c.IsAdmin,
                       isUser = c.isUser,
                       is2FActive = c.is2FActive,
                       AdminLevel = c.AdminLevel,
                   }).ToArray();

            return admins;
        }
        [HttpGet("{id}")]
        public ActionResult<AdminResponse> GetAdmin(int id)
        {
            var admin = _db.Admins
                 .Where(c => id == c.Id)
                 .Select(c => new AdminResponse
                 {
                     Id = c.Id,
                     Name = c.Name,
                     Surname = c.Surname,
                     Email = c.Email,
                     PhoneNumber = c.PhoneNumber,
                     Address = c.Address,
                     Username = c.Username,
                     Password = c.Password,
                     IsAdmin = c.IsAdmin,
                     isUser = c.isUser,
                     is2FActive = c.is2FActive,
                     AdminLevel= c.AdminLevel,
                 }).First();

            return admin;
        }

        [HttpPost]
        public ActionResult<AdminResponse> PostAdmin(AdminRequest request)
        {
            var admin = new Admin
            {
                Name = request.Name,
                Surname = request.Surname,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Address = request.Address,
                Username = request.Username,
                Password = request.Password,
                IsAdmin = request.IsAdmin,
                isUser = request.isUser,
                is2FActive = request.is2FActive,
               AdminLevel = request.AdminLevel,
            };

            _db.Admins.Add(admin);
            _db.SaveChanges();

            var response = new AdminResponse
            {
                Name = admin.Name,
                Surname = admin.Surname,
                Email = admin.Email,
                PhoneNumber = admin.PhoneNumber,
                Address = admin.Address,
                Username = admin.Username,
                Password = admin.Password,
                IsAdmin = admin.IsAdmin,
                isUser = admin.isUser,
                is2FActive = admin.is2FActive,
                AdminLevel= admin.AdminLevel,
            };

            return Ok(response);
        }
        [HttpPut("update-password/{id}")]
        public IActionResult UpdateAdminPassword(int id, string newPassword)
        {
            var admin = _db.Admins.Find(id);

            if (admin == null)
            {
                return NotFound("Admin not found");
            }

            // Hashiranje nove lozinke
            admin.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(newPassword);

            _db.SaveChanges();

            return Ok("Password updated successfully");
        }
        [HttpPut("{id}")]
        public ActionResult<string> PutAdmin(int id, AdminRequest request)
        {
            var admin = _db.Admins.Find(id) ?? throw new KeyNotFoundException("Admin not found");

            admin.Name = request.Name;
            admin.Surname = request.Surname;
            admin.Email = request.Email;
            admin.PhoneNumber = request.PhoneNumber;
            admin.Address = request.Address;
            admin.Username = request.Username;
            
            admin.is2FActive = request.is2FActive;

            _db.SaveChanges();

            return Ok("Admin updated successfully");
        }
        [HttpDelete("{id}")]
        public ActionResult<string> DeleteAdmin(int id)
        {
            var admin = _db.Admins.Find(id) ?? throw new KeyNotFoundException("Admin not found");

            _db.Admins.Remove(admin);
            _db.SaveChanges();

            return Ok("Admin deleted successfully");
        }
    }
}
