using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Expenses;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CashFlow.Infrastructure.DataAccess.Repositories
{
    internal class ExpensesRepository(CashFlowDbContext dbContext) : IExpensesWriteOnlyRepository, IExpensesReadOnlyRepository, IExpenseUpdateOnlyRepository
    {
        public async Task Add(Expense expense)
        {
            await dbContext.Expenses.AddAsync(expense);

        }

        public async Task Delete(long id)
        {
            var result = await dbContext.Expenses.FirstOrDefaultAsync(e => e.Id.Equals(id));

            dbContext.Expenses.Remove(result!);

        }

        public async Task<List<Expense>> GetAll(User user)
        {
            return await dbContext.Expenses.AsNoTracking().OrderBy(e => e.Title).Where(expense => expense.UserId == user.Id).ToListAsync();
        }

        async Task<Expense?> IExpensesReadOnlyRepository.GetById(User user, long id)
        {
            return await dbContext.Expenses.AsNoTracking()
                                           .FirstOrDefaultAsync(e => e.Id == id && e.UserId.Equals(user.Id));
        }
        async Task<Expense?> IExpenseUpdateOnlyRepository.GetById(User user, long id)
        {
            return await dbContext.Expenses.FirstOrDefaultAsync(e => e.Id == id && e.UserId.Equals(user.Id));
        }
        public void Update(Expense expense)
        {
            dbContext.Expenses.Update(expense);
        }

        public async Task<List<Expense>> FilterByMonth(User user,DateOnly date)
        {
            var startDate = new DateTime(year: date.Year, month: date.Month, day: 1).Date;
            var daysInMonth = DateTime.DaysInMonth(year: date.Year, month: date.Month);
            var endDate = new DateTime(year: date.Year, month: date.Month, day: daysInMonth, hour: 23, minute: 59, second: 59);


            return await dbContext.Expenses
                .AsNoTracking()
                .Where(expense => expense.Date >= startDate && expense.Date <= endDate)
                .OrderBy(expense => expense.Date)
                .ThenBy(expense => expense.Title)
                .ToListAsync();

        }
    }
}
