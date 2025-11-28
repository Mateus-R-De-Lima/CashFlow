using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.User;
using CashFlow.Domain.Security.Token;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CashFlow.Application.Services.LoggerUser
{
    public class LoggerUser(IUserReadOnlyRepository userReadOnlyRepository,
                            ITokenProvider tokenProvider) : ILoggerUser
    {
        public async Task<User> Get()
        {
            var token = tokenProvider.TokenOnRequest();

            var tokenHandler = new JwtSecurityTokenHandler();

            var jwtSecurityToken = tokenHandler.ReadJwtToken(token);

            var identifier = jwtSecurityToken.Claims.First(claim => claim.Type == ClaimTypes.Sid).Value;

            return await userReadOnlyRepository.GetUserByIdentifier(Guid.Parse(identifier));
        }
    }
}
