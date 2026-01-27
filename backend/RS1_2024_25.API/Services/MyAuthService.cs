using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Data.Models;
using RS1_2024_25.API.Data.Models.Auth;
using RS1_2024_25.API.Helper;

namespace RS1_2024_25.API.Services
{
    public class MyAuthService(ApplicationDbContext applicationDbContext, IHttpContextAccessor httpContextAccessor, MyTokenGenerator myTokenGenerator)
    {

        // Generisanje novog tokena za korisnika
        public async Task<MyAuthenticationToken> GenerateAuthToken(MyAppUser user, CancellationToken cancellationToken = default)
        {
            string randomToken = myTokenGenerator.Generate(10);

            var authToken = new MyAuthenticationToken
            {
                IpAddress = httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? string.Empty,
                Value = randomToken,
                MyAppUser = user,
                RecordedAt = DateTime.Now
            };

            applicationDbContext.MyAuthenticationTokens.Add(authToken);
            await applicationDbContext.SaveChangesAsync(cancellationToken);

            return authToken;
        }

        // Uklanjanje tokena iz baze podataka
        public async Task<bool> RevokeAuthToken(string tokenValue, CancellationToken cancellationToken = default)
        {
            var authToken = await applicationDbContext.MyAuthenticationTokens
                .FirstOrDefaultAsync(t => t.Value == tokenValue, cancellationToken);

            if (authToken == null)
                return false;

            applicationDbContext.MyAuthenticationTokens.Remove(authToken);
            await applicationDbContext.SaveChangesAsync(cancellationToken);

            return true;
        }

        // Dohvatanje informacija o autentifikaciji korisnika
        public MyAuthInfo GetAuthInfo()
        {
            string? authToken = httpContextAccessor.HttpContext?.Request.Headers["my-auth-token"];

            if (!string.IsNullOrEmpty(authToken))
            {
                var myAuthToken = applicationDbContext.MyAuthenticationTokens
                    .Include(x => x.MyAppUser)
                    .SingleOrDefault(x => x.Value == authToken);

                return GetAuthInfo(myAuthToken);
            }

            // Fallback: Check for JWT (Bearer) Authentication via HttpContext.User
            var user = httpContextAccessor.HttpContext?.User;
            if (user != null && user.Identity != null && user.Identity.IsAuthenticated)
            {
                return new MyAuthInfo
                {
                    UserId = int.Parse(user.FindFirst("id")?.Value ?? "0"),
                    Username = user.FindFirst("username")?.Value ?? "",
                    FirstName = user.FindFirst("name")?.Value ?? "",
                    LastName = user.FindFirst("surname")?.Value ?? "",
                    IsAdmin = user.FindFirst("role")?.Value == "Admin" || user.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value == "Admin",
                    IsManager = user.FindFirst("role")?.Value == "Manager" || user.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value == "Manager", // Assuming Manager role logic if applicable
                    IsLoggedIn = true
                };
            }

            return GetAuthInfo(null);
        }

        public MyAuthInfo GetAuthInfo(MyAuthenticationToken? myAuthToken)
        {
            if (myAuthToken == null)
            {
                return new MyAuthInfo
                {
                    IsAdmin = false,
                    IsManager = false,
                    IsLoggedIn = false,
                };
            }

            return new MyAuthInfo
            {
                UserId = myAuthToken.MyAppUserId,
                Username = myAuthToken.MyAppUser!.Username,
                FirstName = myAuthToken.MyAppUser.FirstName,
                LastName = myAuthToken.MyAppUser.LastName,
                IsAdmin = myAuthToken.MyAppUser.IsAdmin,
                IsManager = myAuthToken.MyAppUser.IsManager,
                IsLoggedIn = true
            };
        }

        internal async Task<MyAuthenticationToken?> GenerateAuthToken(UserAccount loggedInUser, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

    // DTO to hold authentication information
    public class MyAuthInfo
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsManager { get; set; }
        public bool IsLoggedIn { get; set; }
        public string SlikaPath { get; set; }
    }
}
