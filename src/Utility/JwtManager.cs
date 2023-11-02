using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using api.src.Domain.Entities.Shared;
using Microsoft.IdentityModel.Tokens;

namespace api.src.Utility
{
    public class JwtManager
    {
        private static readonly string _key = "testy12%GDSA^7%#4323sfdgDAcz@#43";
        private static readonly SymmetricSecurityKey _securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
        private static readonly SigningCredentials _signingCredentials = new SigningCredentials(_securityKey, SecurityAlgorithms.HmacSha256);

        public static string GenerateAccessToken(Guid id, string roleName)
        {
            var claims = new List<Claim>
            {
                new Claim("UserId", id.ToString()),
                new Claim(ClaimTypes.Role, roleName)
            };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddDays(2),
                signingCredentials: _signingCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static string GenerateRefreshToken() => Guid.NewGuid().ToString();

        public static TokenPair GenerateTokenPair(Guid id, string roleName) =>
            new TokenPair(
                    GenerateAccessToken(id, roleName),
                    GenerateRefreshToken()
                );

        public static List<Claim> GetClaims(string token) =>
            new JwtSecurityTokenHandler()
                .ReadJwtToken(token.Replace("Bearer ", ""))
                .Claims
                .ToList();

        public static Guid GetUserId(string token) =>
            Guid.Parse(GetClaims(token)
                .FirstOrDefault(claim => claim.Type == "UserId")?.Value ?? Guid.Empty.ToString());
    }

}