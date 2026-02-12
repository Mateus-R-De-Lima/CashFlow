using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Security.Token
{
    public interface IAccessTokenGenarator
    {
       Task<string> Generate(User user);
    }
}
