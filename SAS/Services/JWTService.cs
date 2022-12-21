using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using SAS.DTO;
using SAS.Interfaces;
using SAS.Queries;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SAS.Services
{
    public class JWTService : IJWTService
    {
        private readonly ApplicationContext db;
        private readonly IUserQuery userQuery;
        private readonly UserManager<UserDTO> userManager;
        public JWTService(ApplicationContext context,
                            IUserQuery userQuery,
                            UserManager<UserDTO> userManager)
        {
            db = context;
            this.userQuery = userQuery;
            this.userManager = userManager;
        }

        public async Task<ClaimsIdentity> GetUserClaimsIdentity(UserDTO user)
        {
            if (user != null)
            {
                var Role = await userManager.GetRolesAsync(user);
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Role, Role[0]),
                    new Claim(ClaimTypes.Surname, user.LastName),
                    new Claim(ClaimTypes.Name, user.FirstName)
                };

                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token");
                return claimsIdentity;
            }

            return null;
            
        }

        public async Task<string> GenerateToken(UserDTO user, string TokenType)
        {
            int LIFETIME = 10;
            if (TokenType == "access")
            {
                LIFETIME = AuthOptions.ACCESS_LIFETIME;
            }

            else if (TokenType == "refresh")
            {
                LIFETIME = AuthOptions.REFRESH_LIFETIME;
            }

            else
            {
                return null;
            }

            var identity = await GetUserClaimsIdentity(user);
            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            string Token = new JwtSecurityTokenHandler().WriteToken(jwt);

            return Token;
        }
    }
}
