using RS1_2024_25.API.Data;
using RS1_2024_25.API.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace RS1_2024_25.API.Endpoints
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAccountController(ApplicationDbContext _db) : ControllerBase
    {
        public class UserAccountRequest
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
            public int GenderId { get; set; }
            public int CityId { get; set; }
            public string AdminLevel { get; set; } // Admin-specific
        }

        public class UserAccountResponse
        {
            public int Id { get; set; }
            public string Type { get; set; } // "User" or "Admin"
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
            public string GenderName { get; set; }
            public string CityName { get; set; }
            public string AdminLevel { get; set; } // Admin-specific
        }

        // GET: api/UserAccount
        [HttpGet]
        public ActionResult<UserAccountResponse[]> GetUserAccounts()
        {
            var users = _db.UserAccounts.OfType<User>()
                .Include(u => u.Gender).Include(u => u.City)
                .Select(u => new UserAccountResponse
                {
                    Id = u.Id,
                    Type = "User",
                    Name = u.Name,
                    Surname = u.Surname,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    Address = u.Address,
                    Username = u.Username,
                    Password = u.Password,
                    IsAdmin = u.IsAdmin,
                    isUser = u.isUser,
                    is2FActive = u.is2FActive,
                    GenderName = u.Gender.GenderName,
                    CityName = u.City.Name 
                }).ToList();

            var admins = _db.UserAccounts.OfType<Admin>()
                .Select(a => new UserAccountResponse
                {
                    Id = a.Id,
                    Type = "Admin",
                    Name = a.Name,
                    Surname = a.Surname,
                    Email = a.Email,
                    PhoneNumber = a.PhoneNumber,
                    Address = a.Address,
                    Username = a.Username,
                    Password = a.Password,
                    IsAdmin = a.IsAdmin,
                    isUser = a.isUser,
                    is2FActive = a.is2FActive,
                    AdminLevel = a.AdminLevel
                }).ToList();

            return users.Concat(admins).ToArray();
        }

        [HttpGet("{id}")]
        public ActionResult<UserAccountResponse> GetUserAccount(int id)
        {
            var userAccount = _db.UserAccounts
                .Include(ua => (ua as User).Gender)
                .Include(ua => (ua as User).City)
                .FirstOrDefault(ua => ua.Id == id);

            if (userAccount == null)
                return NotFound("User account not found");

            var response = new UserAccountResponse
            {
                Id = userAccount.Id,
                Type = userAccount is User ? "User" : "Admin",
                Name = userAccount.Name,
                Surname = userAccount.Surname,
                Email = userAccount.Email,
                PhoneNumber = userAccount.PhoneNumber,
                Address = userAccount.Address,
                Username = userAccount.Username,
                Password = userAccount.Password,
                IsAdmin = userAccount.IsAdmin,
                isUser = userAccount.isUser,
                is2FActive = userAccount.is2FActive,
                GenderName = userAccount is User u && u.Gender != null ? u.Gender.GenderName : "N/A",
                CityName = userAccount is User ua && ua.City != null ? ua.City.Name : "N/A",
                AdminLevel = userAccount is Admin admin ? admin.AdminLevel : null
            };

            return Ok(response);
        }



        [HttpPost]
        public ActionResult<UserAccountResponse> PostUserAccount(UserAccountRequest request)
        {
            UserAccount newUserAccount;

            // Determine the type based on IsAdmin flag
            if (request.IsAdmin)
            {
                if (string.IsNullOrEmpty(request.AdminLevel))
                {
                    return BadRequest("AdminLevel is required for admin accounts.");
                }

                newUserAccount = new Admin
                {
                    Name = request.Name,
                    Surname = request.Surname,
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber,
                    Address = request.Address,
                    Username = request.Username,
                    Password = request.Password,
                    IsAdmin = true,
                    isUser = false,
                    is2FActive = request.is2FActive,
                    AdminLevel = request.AdminLevel
                };
            }
            else
            {
                newUserAccount = new User
                {
                    Name = request.Name,
                    Surname = request.Surname,
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber,
                    Address = request.Address,
                    Username = request.Username,
                    Password = request.Password,
                    IsAdmin = false,
                    isUser = true,
                    is2FActive = request.is2FActive,
                    GenderId = request.GenderId,
                    CityId = request.CityId
                };
            }

            _db.UserAccounts.Add(newUserAccount);
            _db.SaveChanges();

            return GetUserAccount(newUserAccount.Id);
        }


      

        // DELETE: api/UserAccount/5
        [HttpDelete("{id}")]
        public ActionResult<string> DeleteUserAccount(int id)
        {
            var userAccount = _db.UserAccounts.Find(id) ?? throw new KeyNotFoundException("User account not found");

            _db.UserAccounts.Remove(userAccount);
            _db.SaveChanges();

            return Ok("User account deleted successfully");
        }
    }
}
