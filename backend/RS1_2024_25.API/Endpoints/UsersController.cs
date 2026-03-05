using RS1_2024_25.API.Data;
using RS1_2024_25.API.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Net;
using Microsoft.AspNetCore.Identity;

namespace RS1_2024_25.API.Endpoints
{

    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(ApplicationDbContext _db, IPasswordHasher<UserAccount> _passwordHasher) : ControllerBase
    {
        public class UserRequest
        {
            public string? Name { get; set; }
            public string? Surname { get; set; }
            public string? Email { get; set; }
            public string? PhoneNumber { get; set; }
            public string? Address { get; set; }
            public string? Username { get; set; }
            public string? Password { get; set; }
            public bool IsAdmin { get; set; }
            public bool isUser { get; set; }
            public IFormFile? Image { get; set; }
            public bool is2FActive { get; set; }
            public int? GenderId { get; set; }
            public int? CityId { get; set; }
        }

        public class UserResponse
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }
            public string Address { get; set; }
            public string Username { get; set; }
            public bool IsAdmin { get; set; }
            public bool isUser { get; set; }
            public bool is2FActive { get; set; }
            public int GenderId { get; set; }
            public string GenderName { get; set; }
            public int CityId { get; set; }
            public string CityName { get; set; }

        }


        [HttpGet]
        public ActionResult<UserResponse[]> GetUsers(
            [FromQuery] string? search,
            [FromQuery] string? role,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            IQueryable<UserAccount> query = _db.UserAccounts;

            // Apply search filter
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(u =>
                    u.Username.Contains(search) ||
                    u.Email.Contains(search) ||
                    u.Name.Contains(search) ||
                    u.Surname.Contains(search));
            }

            // Apply role filter
            if (!string.IsNullOrEmpty(role) && role != "all")
            {
                if (role == "admin")
                {
                    query = query.Where(u => u.IsAdmin == true);
                }
                else if (role == "user")
                {
                    query = query.Where(u => u.IsAdmin == false);
                }
            }

            // Pagination
            var totalCount = query.Count();
            var users = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new
                {
                    u.Id,
                    u.Username,
                    u.Email,
                    u.IsAdmin,
                    u.Name,
                    u.Surname,
                    u.PhoneNumber,
                    u.Address,
                    u.ImageUrl
                })
                .ToArray();

            // Return with pagination metadata
            return Ok(new
            {
                TotalCount = totalCount,
                Items = users
            });
        }
        // GET: api/User/5
        [HttpGet("{id}")]
        public ActionResult<UserResponse> GetUser(int id)
        {
            var user = _db.Users
                 .Include(c => c.Gender).Include(c => c.City)
                 .Where(c => id == c.Id)
                 .Select(c => new UserResponse
                 {
                     Id = c.Id,
                     Name = c.Name,
                     Surname = c.Surname,
                     Email = c.Email,
                     PhoneNumber = c.PhoneNumber,
                     Address = c.Address,
                     Username = c.Username,
                     IsAdmin = c.IsAdmin,
                     isUser = c.isUser,
                     is2FActive = c.is2FActive ?? false,
                     GenderName = c.Gender != null ? c.Gender.GenderName : "Unknown",
                     CityName = c.City != null ? c.City.Name : "Unknown"

                 }).FirstOrDefault();

            if (user == null) return NotFound("User not found");

            return user;
        }

        //POST: api/User
        [HttpPost]
        public ActionResult<UserResponse> PostUser(UserRequest request)
        {
            var user = new User
            {
                Name = request.Name,
                Surname = request.Surname,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Address = request.Address,
                Username = request.Username,
                IsAdmin = request.IsAdmin,
                isUser = request.isUser,
                is2FActive = request.is2FActive,
                GenderId = request.GenderId,
                CityId = request.CityId,
            };

            user.Password = _passwordHasher.HashPassword(user, request.Password);

            _db.Users.Add(user);
            _db.SaveChanges();

            var response = new UserResponse
            {
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                Username = user.Username,
                IsAdmin = user.IsAdmin,
                isUser = user.isUser,
                is2FActive = user.is2FActive ?? false,
                GenderName = _db.Genders.Find(user.GenderId)?.GenderName ?? "Unknown",
                CityName = _db.Cities.Find(user.CityId)?.Name ?? "Unknown"
            };

            return Ok(response);
        }

        //PUT: api/User/5
        [HttpPut("{id}")]
        public ActionResult<string> PutUser(int id, [FromBody] UserRequest request)
        {
            var user = _db.Users.Find(id) ?? throw new KeyNotFoundException("User not found");

            if (request.CityId.HasValue)
            {
                bool cityExists = _db.Cities.Any(c => c.ID == request.CityId.Value);
                if (!cityExists) return BadRequest("Invalid City ID.");

                user.CityId = request.CityId.Value;
            }

            if (request.GenderId.HasValue)
            {
                bool genderExists = _db.Genders.Any(c => c.GenderId == request.GenderId.Value);
                if (!genderExists) return BadRequest("Invalid Gender ID.");

                user.GenderId = request.GenderId.Value;
            }


            user.Name = request.Name;
            user.Surname = request.Surname;
            user.Email = request.Email;
            user.PhoneNumber = request.PhoneNumber;
            user.Address = request.Address;
            user.Username = request.Username;
            user.is2FActive = request.is2FActive;

            _db.SaveChanges();

            return Ok(new { message = "User updated successfully" });
        }


        [HttpPut("update-profile/{id}")]
        public async Task<IActionResult> UpdateProfile(int id, [FromForm] UserRequest request)
        {
            var user = await _db.Users.FindAsync(id);
            if (user == null)
                return NotFound("User not found.");

            user.Username = request.Username;
            user.Name = request.Name;
            user.Surname = request.Surname;
            user.Email = request.Email;
            user.PhoneNumber = request.PhoneNumber;
            user.Address = request.Address;
            user.CityId = request.CityId;
            user.GenderId = request.GenderId;

            if (request.Image != null && request.Image.Length > 0)
            {
                var imageName = Guid.NewGuid().ToString() + Path.GetExtension(request.Image.FileName);
                var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserImages", imageName);

                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    await request.Image.CopyToAsync(stream);
                }

                user.ImageUrl = $"UserImages/{imageName}";
            }

            await _db.SaveChangesAsync();
            return Ok(new { message = "Profile updated successfully" });
        }



        [HttpDelete("{id}")]
        public ActionResult<string> DeleteUser(int id)
        {
            var user = _db.Users.Find(id) ?? throw new KeyNotFoundException("User not found");

            _db.Users.Remove(user);
            _db.SaveChanges();

            return Ok("User deleted successfully");
        }
    }
}
