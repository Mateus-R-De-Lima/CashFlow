using CashFlow.Domain.Repositories.User;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.DataAccess.Repositories
{
    internal class UserRepository(CashFlowDbContext dbContext) : IUserReadOnlyRepository
    {
        public async Task<bool> ExistActiveUserWithEmail(string email)
        {
            return await dbContext.Users.AnyAsync(user => user.Email.Equals(email));
        }
    }
}
