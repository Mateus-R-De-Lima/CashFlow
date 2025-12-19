using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Repositories.User
{
    public interface IUserUpdateOnlyRepository
    {
        void UpdateProfile(Entities.User user);

        Task<Entities.User> GetById(long id); 

    }
}
