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
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.VisualBasic;

namespace RS1_2024_25.API.Endpoints
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(ApplicationDbContext _db, IPasswordHasher<UserAccount> _passwordHasher, IConfiguration _configuration) : ControllerBase
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

        public class ChangePasswordRequest
        {
            public int UserId { get; set; }
            public string CurrentPassword { get; set; }
            public string NewPassword { get; set; }
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

            if (user.isDeleted == true)
                return Unauthorized("Your account is deactivated. Click 'Activate my profile' to restore access.");

            var token = CreateJwt(user);
            var role = user.IsAdmin ? "Admin" : "User";

            var refreshToken = GenerateRefreshToken();

            _db.RefreshTokens.Add(new RefreshToken
            {
                Token = refreshToken,
                Expires = DateTime.UtcNow.AddMinutes(15),
                UserAccountId = user.Id,
            });

            _db.SaveChanges();

            return Ok(new { Token = token, Role = role , RefreshToken = refreshToken, isAdmin=user.IsAdmin});
        }

        private string CreateJwt(UserAccount user)
        {
            var role = user.IsAdmin ? "Admin" : "User";
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!);
            var identity = new ClaimsIdentity(new Claim[] {

                   new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                   new Claim("id", user.Id.ToString()),

                   new Claim("username", user.Username ?? ""),
                   new Claim("name", user.Name ?? ""),
                   new Claim("surname", user.Surname ?? ""),
                   new Claim("email", user.Email ?? ""),
                   new Claim("phone", user.PhoneNumber ?? ""),
                   new Claim("cityId", user.CityId?.ToString() ?? "0"),
                   new Claim("address", user.Address ?? ""),
                   new Claim("role", user.IsAdmin ? "Admin" : "User")



            });

            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = credentials
            };
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);

        }

        private string GenerateRefreshToken()
        {
            var randomBytes = new byte [64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
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
        public IActionResult CheckPhone(string phoneNumber, int? userId = null)
        {
            bool exists;

            if(userId.HasValue)
            {
                exists = _db.UserAccounts.Any(x => x.PhoneNumber == phoneNumber && x.Id != userId.Value);
            }
            else
            {
                exists = _db.UserAccounts.Any(x=>x.PhoneNumber == phoneNumber);
            }

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

                    string? localImageUrl = null;

                    if (!string.IsNullOrEmpty(picture))
                    {
                        using var httpClient = new HttpClient();
                        var imageBytes = await httpClient.GetByteArrayAsync(picture);

                        var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UserImages");
                        if (!Directory.Exists(folder))
                            Directory.CreateDirectory(folder);

                        var fileName = Guid.NewGuid().ToString() + ".jpg";
                        var filePath = Path.Combine(folder, fileName);
                        await System.IO.File.WriteAllBytesAsync(filePath, imageBytes);

                        localImageUrl = $"UserImages/{fileName}";
                    }

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
                        Password = _passwordHasher.HashPassword(null, Guid.NewGuid().ToString()),
                        ImageUrl = localImageUrl
                    };

                  
                    _db.Users.Add(user);
                    await _db.SaveChangesAsync();
                }

                if (user.isDeleted == true)
                    return Unauthorized("Your account is deactivated.");

                var token = CreateJwt(user);

                var refreshToken = GenerateRefreshToken();

                _db.RefreshTokens.Add(new RefreshToken
                {
                    Token = refreshToken,
                    Expires = DateTime.UtcNow.AddMinutes(15),
                    UserAccountId = user.Id,
                });

                await _db.SaveChangesAsync();


                return Ok(new { Token = token, RefreshToken = refreshToken, Message = "Google login successful" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An internal server error occurred.");
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

        

        public class UsernameRequest
        {
            public string Username { get; set; }
        }

        [Authorize]
        [HttpPost("deactivate")]
        public async Task<ActionResult> Deactivate([FromBody] UsernameRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Username))
                return BadRequest("Username is required.");

            var userIdClaim = User.FindFirst("id")?.Value;
            var usernameClaim = User.FindFirst("username")?.Value;

            if (!int.TryParse(userIdClaim, out var userId) || string.IsNullOrEmpty(usernameClaim))
                return Unauthorized("Invalid token claims.");

            if (!string.Equals(usernameClaim, request.Username, StringComparison.OrdinalIgnoreCase))
                return Forbid("You are not allowed to deactivate other users.");

            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId && u.Username == request.Username);
            if (user == null)
                return NotFound("User not found.");

            if (user.isDeleted == true)
                return BadRequest("Your profile is already deactivated.");

            user.isDeleted = true;
            await _db.SaveChangesAsync();

            return Ok(new { message = "Your account has been deactivated successfully." });
        }


        public class ReactivateRequest
        {
            public string Email { get; set; }
        }


        [HttpPost("reactivate")]
        public async Task<IActionResult> ReactivateByEmail([FromBody] ReactivateRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email))
                return BadRequest("Email is required.");

            var user = await _db.UserAccounts
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null)
                return NotFound("User not found.");

            if (user.isDeleted == false)
                return BadRequest("Account is already active.");

            user.isDeleted = false;
            await _db.SaveChangesAsync();

            return Ok(new { message = "Account reactivated." });
        }







        public class TwoFactorRequest
        {
            public string PhoneNumber { get; set; }
        }


        [Authorize]
        [HttpPost("enable-2fa")]
        public async Task<IActionResult> EnableTwoFActive([FromBody] TwoFactorRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.PhoneNumber))
                return BadRequest("Phone number is required.");

            var userId = int.Parse(User.FindFirst("id")?.Value ?? "0");

            
            var phoneExists = _db.UserAccounts
                .Any(x => x.PhoneNumber == request.PhoneNumber && x.Id != userId);

            if (phoneExists)
                return BadRequest("Phone number already in use.");

            var user = await _db.UserAccounts.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) return NotFound("User not found in UserAccounts.");

            user.PhoneNumber = request.PhoneNumber;
            user.is2FActive = true;

            await _db.SaveChangesAsync();

            return Ok(new { message = "Two-factor authentication enabled and phone number updated." });
        }

        public class RefreshTokenRequest
        {
            public string Token { get; set; }
        }

        [HttpPost("refresh-token")]
        public IActionResult Refresh([FromBody] RefreshTokenRequest request) {

            if (string.IsNullOrWhiteSpace(request.Token))
                return BadRequest("Refresh token is required.");

            var storedToken = _db.RefreshTokens
                .Include(r => r.UserAccount)
                .FirstOrDefault(r => r.Token == request.Token && r.isRevoked == false);

            if (storedToken == null || storedToken.Expires < DateTime.UtcNow)
                return Unauthorized("Invalid or expired refresh token");

            
            storedToken.isRevoked = true;

            
            var newJwt = CreateJwt(storedToken.UserAccount);
            var newRefreshToken = GenerateRefreshToken();

            _db.RefreshTokens.Add(new RefreshToken
            {
                Token = newRefreshToken,
                Expires = DateTime.UtcNow.AddMinutes(15),
                UserAccountId = storedToken.UserAccountId
            });

            _db.SaveChanges();

            return Ok(new
            {
                Token = newJwt,
                RefreshToken = newRefreshToken
            });

        }
    }
}
