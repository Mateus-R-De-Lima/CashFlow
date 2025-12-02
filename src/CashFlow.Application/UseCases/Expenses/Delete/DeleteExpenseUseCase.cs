using CashFlow.Application.Services.LoggerUser;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.Expenses.Delete
{
    public class DeleteExpenseUseCase(IExpensesWriteOnlyRepository repository,
                                      IExpensesReadOnlyRepository expensesReadOnlyRepository,
                                      ILoggerUser loggerUser,
                                      IUnitOfWork unitOfWork) : IDeleteExpenseUseCase
    {

        public async Task Execute(long id)
        {
            var user = await loggerUser.Get();

            var expense = await expensesReadOnlyRepository.GetById(user,id);

            if (expense is null)
                throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);

            await repository.Delete(id);

            await unitOfWork.Commit();

        }


    }
}