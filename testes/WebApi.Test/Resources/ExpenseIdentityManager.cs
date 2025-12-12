using CashFlow.Domain.Entities;

namespace WebApi.Test.Resources
{
    public class ExpenseIdentityManager(CashFlow.Domain.Entities.Expense expense)
    {
        public long GetExpenseId() => expense.Id;
    }
}
