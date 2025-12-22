using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.User;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.DataAccess.Repositories
{
    internal class UserRepository(CashFlowDbContext dbContext) : IUserReadOnlyRepository, IUserWriteOnlyRepository, IUserUpdateOnlyRepository
    {
        public async Task Add(User user)
        {
            await dbContext.Users.AddAsync(user);
        }

        public async Task<bool> ExistActiveUserWithEmail(string email)
        {
            return await dbContext.Users.AnyAsync(user => user.Email.Equals(email));
        }


        public async Task<User?> GetUserByEmail(string email)
        {
            return await dbContext.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Email.Equals(email));
        }

        public async Task<User?> GetUserByIdentifier(Guid identifier)
        {
            return await dbContext
                 .Users
                 .AsNoTracking()
                 .FirstAsync(user => user.UserIdentifier.Equals(identifier));
        }

        public void Update(User user)
        {
            dbContext.Users.Update(user);
        }


        public async Task<User> GetById(long id)
        {
            return await dbContext.Users.FirstAsync(user => user.Id == id);
        }
    }
}
