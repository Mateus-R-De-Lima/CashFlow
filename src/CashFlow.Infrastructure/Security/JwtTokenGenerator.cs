using CashFlow.Domain.Entities;
using CashFlow.Domain.Security.Token;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CashFlow.Infrastructure.Security
{
    public class JwtTokenGenerator(
        uint expirationTimeMinutes,
        string signingKey,
        IConnectionMultiplexer redis
        ) : IAccessTokenGenarator
    {
        public async Task<string> Generate(User user)
        {
            var redisDb = redis.GetDatabase();

            var userCache = redisDb.StringGet($"userId:{user.UserIdentifier.ToString()}");

            if (userCache.HasValue)
                return userCache;

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim("UserIdentifier",user.UserIdentifier.ToString()),
                new Claim(ClaimTypes.Sid,user.UserIdentifier.ToString()),
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

            var token = tokenHandler.WriteToken(securityToken);

            await redisDb.StringSetAsync(
                user.UserIdentifier.ToString(),
                token, 
                TimeSpan.FromHours(2));   

            return token;
        }


        private SymmetricSecurityKey SecurityKey()
        {
            var key = Encoding.UTF8.GetBytes(signingKey);

            return new SymmetricSecurityKey(key);
        }
    }
}
