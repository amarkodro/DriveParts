using RS1_2024_25.API.Data;
using RS1_2024_25.API.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace RS1_2024_25.API.Endpoints
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(ApplicationDbContext _db, IPasswordHasher<UserAccount> _passwordHasher) : ControllerBase
    {

        public class RegisterRequest
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public string Email { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
            public int GenderId { get; set; }
            public int CityId { get; set; }
            public string PhoneNumber { get; set; }
            public string Address { get; set; }
            public IFormFile? ProfileImage { get; set; }
        }

        [HttpPost("register")]
        public IActionResult Register([FromForm] RegisterRequest request)
        {
            if (_db.UserAccounts.Any(u => u.Username == request.Username))
                return BadRequest("Username already exists");
            if (_db.UserAccounts.Any(u => u.Email == request.Email))
                return BadRequest("Email already exists");
            if (_db.UserAccounts.Any(u => u.PhoneNumber == request.PhoneNumber))
                return BadRequest("Phone number already exists");

            var gender = _db.Genders.Find(request.GenderId);
            if (gender == null)
                return BadRequest("Invalid GenderId");

            var city = _db.Cities.Find(request.CityId);
            if (city == null)
                return BadRequest("Invalid CityId");

            string? imagePath = null;

            if (request.ProfileImage != null)
            {
                var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UserImages");
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(request.ProfileImage.FileName);
                var fullPath = Path.Combine(folder, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    request.ProfileImage.CopyTo(stream);
                }

                imagePath = $"UserImages/{fileName}";
            }

            var user = new User
            {
                Name = request.Name,
                Surname = request.Surname,
                Email = request.Email,
                Username = request.Username,
                Address = request.Address,
                PhoneNumber = request.PhoneNumber,
                GenderId = request.GenderId,
                CityId = request.CityId,
                isUser =true,
                IsAdmin = false,
                ImageUrl = imagePath

            };

            user.Password = _passwordHasher.HashPassword(user, request.Password);

            _db.Users.Add(user);
            _db.SaveChanges();

            var token = CreateJwt(user);

            return Ok(new { Message = "User registered successfully", Token = token });
        }

        // DTO for login
        public class LoginRequest
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        [HttpPost("login")]
        public IActionResult Login(LoginRequest request)
        {
            var user = _db.UserAccounts.FirstOrDefault(x => x.Username == request.Username);

            if (user == null)
                return Unauthorized("Invalid username or password");

            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, request.Password);

            if (result == PasswordVerificationResult.Failed)
                return Unauthorized("Invalid username or password");

            var token = CreateJwt(user);
            var role = user.IsAdmin ? "Admin" : "User";

            return Ok(new { Token = token, Role = role });
        }

        private string CreateJwt(UserAccount user)
        {
            var role = user.IsAdmin ? "Admin" : "User";
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes("f8d2eV3r5/8nW1qR4xPqL6zM9xD5u2F8xM0a1pZ3wNk=");
            var identity = new ClaimsIdentity(new Claim[] {

                    new Claim("id", user.Id.ToString()),
                    new Claim(ClaimTypes.Role, $"{role}"),
                    new Claim("username", user.Username),
                    new Claim("name", user.Name),
                    new Claim("surname", user.Surname),

            });

            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = credentials
            };
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);

        }

        [HttpGet("check-username")]
        public IActionResult CheckUsername(string username)
        {
            bool exists = _db.UserAccounts.Any(u => u.Username == username);
            return Ok(new { exists });
        }

        [HttpGet("check-email")]
        public IActionResult CheckEmail(string email)
        {
            bool exists = _db.UserAccounts.Any(u => u.Email == email);
            return Ok(new { exists });
        }

        [HttpGet("check-phone")]
        public IActionResult CheckPhone(string phoneNumber)
        {
            bool exists = _db.UserAccounts.Any(u => u.PhoneNumber == phoneNumber);
            return Ok(new { exists });
        }



        public class GoogleLoginRequest
        {
            public string IdToken { get; set; }
        }

        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.IdToken))
                    return BadRequest("IdToken is required");

                var client = new HttpClient();
                var response = await client.GetAsync($"https://oauth2.googleapis.com/tokeninfo?id_token={request.IdToken}");

                if (!response.IsSuccessStatusCode)
                    return BadRequest("Invalid Google token");

                var content = await response.Content.ReadAsStringAsync();
                var payload = System.Text.Json.JsonDocument.Parse(content).RootElement;

                if (!payload.TryGetProperty("email", out var emailProp))
                    return BadRequest("Email not found in token");

                var email = emailProp.GetString();
                var name = payload.TryGetProperty("given_name", out var n) ? n.GetString() : "";
                var surname = payload.TryGetProperty("family_name", out var s) ? s.GetString() : "";
                var picture = payload.TryGetProperty("picture", out var pic) ? pic.GetString() : null;

                var user = _db.Users.FirstOrDefault(u => u.Email == email);

                if (user == null)
                {
                    var baseUsername = email.Split('@')[0];
                    var uniqueUsername = GenerateUniqueUsername(baseUsername);

                    user = new User
                    {
                        Email = email ?? "no-email@google.com",
                        Username = uniqueUsername,
                        Name = name ?? "",
                        Surname = surname ?? "",
                        isUser = true,
                        IsAdmin = false,
                        is2FActive = false,
                        PhoneNumber = null,
                        CityId = null,
                        GenderId = null,
                        Address = null,
                        Password = Guid.NewGuid().ToString(),
                        ImageUrl = picture
                    };

                    _db.Users.Add(user);
                    await _db.SaveChangesAsync();
                }

                var token = CreateJwt(user);

                return Ok(new { Token = token, Message = "Google login successful" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        private string GenerateUniqueUsername(string baseUsername)
        {
            var username = baseUsername;
            int counter = 1;

            while (_db.Users.Any(u => u.Username == username))
            {
                username = $"{baseUsername}-{counter}";
                counter++;
            }

            return username;
        }
    }
}
