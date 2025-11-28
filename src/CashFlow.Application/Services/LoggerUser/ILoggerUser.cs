using CashFlow.Domain.Entities;

namespace CashFlow.Application.Services.LoggerUser
{
    public interface ILoggerUser
    {
        Task<User> Get();
    }
}