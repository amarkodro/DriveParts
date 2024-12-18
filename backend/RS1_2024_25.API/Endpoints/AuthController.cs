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

namespace RS1_2024_25.API.Endpoints
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(ApplicationDbContext _db) : ControllerBase
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
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterRequest request)
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

            var user = new UserAccount
            {
                Name = request.Name,
                Surname = request.Surname,
                Email = request.Email,
                Username = request.Username,
                Password = request.Password,
                Address = request.Address,
                PhoneNumber = request.PhoneNumber,
                IsAdmin = false,
                is2FActive = true,
                isUser = true


            };

            _db.UserAccounts.Add(user);
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
            {
                return Unauthorized("Invalid username or password");
            }

            if (request.Password==user.Password)
            {
                return Unauthorized("Invalid username or password");
            }

            
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

                    new Claim(ClaimTypes.Role, role),
                    new Claim(ClaimTypes.Name, $"{user.Name} {user.Surname}"),

            });

            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = DateTime.Now.AddHours(1),
                SigningCredentials = credentials
            };
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);

        }
    }
}
