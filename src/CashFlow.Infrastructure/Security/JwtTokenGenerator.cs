using CashFlow.Domain.Entities;
using CashFlow.Domain.Security.Token;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CashFlow.Infrastructure.Security
{
    public class JwtTokenGenerator(
        uint expirationTimeMinutes,
        string signingKey
        ) : IAccessTokenGenarator
    {
        public string Generate(User user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim("UserIdentifier",user.UserIdentifier.ToString()),
                new Claim(ClaimTypes.Role, user.Role)

            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow.AddMinutes(expirationTimeMinutes),
                SigningCredentials = new SigningCredentials(SecurityKey(),SecurityAlgorithms.HmacSha256Signature),
                Subject = new ClaimsIdentity(claims)

            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var securityToken = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(securityToken);
        }


        private SymmetricSecurityKey SecurityKey()
        {
            var key = Encoding.UTF8.GetBytes(signingKey);

            return new SymmetricSecurityKey(key);
        }
    }
}
