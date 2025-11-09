using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Security.Token
{
    public interface IAccessTokenGenarator
    {
        string Generate(User user);
    }
}
