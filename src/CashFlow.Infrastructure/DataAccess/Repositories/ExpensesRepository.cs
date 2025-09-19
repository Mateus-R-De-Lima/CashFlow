using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Expenses;

namespace CashFlow.Infrastructure.DataAccess.Repositories
{
    internal class ExpensesRepository(
        CashFlowDbContext dbContext
        ) : IExpensesRepository
    {
        public void Add(Expense expense)
        {
            dbContext.Expenses.Add(expense);
           
        }
    }
}
