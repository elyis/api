using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace api.src.Utility
{
    public class InvitationLinkGenerator
    {
        private static readonly string _key = "testy12%GDSA^7%#4323sfdgDAcz@#43";
        private static readonly SymmetricSecurityKey _securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
        private static readonly SigningCredentials _signingCredentials = new SigningCredentials(_securityKey, SecurityAlgorithms.HmacSha256);

        public static string Generate(Guid organizationId)
        {
            var claims = new List<Claim>
            {
                new Claim("OrganizationId", organizationId.ToString()),
            };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: _signingCredentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenString;
        }
    }
}