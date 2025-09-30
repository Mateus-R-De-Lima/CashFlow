using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Expenses;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CashFlow.Infrastructure.DataAccess.Repositories
{
    internal class ExpensesRepository(
        CashFlowDbContext dbContext
        ) : IExpensesWriteOnlyRepository, IExpensesReadOnlyRepository
    {
        public async Task Add(Expense expense)
        {
            await dbContext.Expenses.AddAsync(expense);

        }

        public async Task<List<Expense>> GetAll()
        {
            return await dbContext.Expenses.AsNoTracking().OrderBy(e => e.Title).ToListAsync();
        }

        public async Task<Expense?> GetById(long id)
        {
            return await dbContext.Expenses.AsNoTracking()
                                           .FirstOrDefaultAsync(e => e.Id == id);
        }
    }
}
