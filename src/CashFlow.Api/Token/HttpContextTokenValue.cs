using CashFlow.Domain.Security.Token;

namespace CashFlow.Api.Token
{
    public class HttpContextTokenValue(IHttpContextAccessor httpContextAccessor) : ITokenProvider
    {
        public string TokenOnRequest()
        {
            var authorization = httpContextAccessor.HttpContext!.Request.Headers.Authorization.ToString();

            return authorization["Bearer ".Length..].Trim();
        }
    }
}
