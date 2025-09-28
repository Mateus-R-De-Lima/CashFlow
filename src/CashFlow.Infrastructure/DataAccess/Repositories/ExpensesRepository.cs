using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Expenses;
using System.Threading.Tasks;

namespace CashFlow.Infrastructure.DataAccess.Repositories
{
    internal class ExpensesRepository(
        CashFlowDbContext dbContext
        ) : IExpensesRepository
    {
        public async Task Add(Expense expense)
        {
           await dbContext.Expenses.AddAsync(expense);
           
        }
    }
}
