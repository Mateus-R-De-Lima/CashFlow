using CashFlow.Application.UseCases.Expenses.Delete;
using CashFlow.Application.UseCases.Expenses.GetById;
using CashFlow.Domain.Entities;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;

namespace UseCase.Tests.Expenses.Delete
{
    public class DeleteExepenseUseCaseTest
    {

        [Fact]
        public async Task Success()
        {
            var loggedUser = UserBuilder.Build();
            var expense = ExpenseBuilder.Build(loggedUser);
            var useCase = CreateUseCase(loggedUser, expense);                     

            // Act
            var act = async () => await useCase.Execute(expense.Id);

            // Assert — NÃO lança exceção
            await act();


        }

        [Fact]
        public async Task Error_Expense_Not_Found()
        {
            var loggedUser = UserBuilder.Build();
            var useCase = CreateUseCase(loggedUser);

            var exception = await Assert.ThrowsAsync<NotFoundException>(() =>
                            useCase.Execute(id: 1000));

            Assert.Contains(ResourceErrorMessages.EXPENSE_NOT_FOUND, exception.GetErros());
        }

        private DeleteExpenseUseCase CreateUseCase(CashFlow.Domain.Entities.User user, Expense? expense = null)
        {
            var repositoryWriteOnly = ExpensesWriteOnlyRepositoryBuilder.Build();
            var repository = new ExpensesReadOnlyRepositoryBuilder().GetById(user, expense).Build();
            var unitOfWork = UnitOfWorkBuilder.Build();
            var loggedUser = LoggedUserBuilder.Build(user);

            return new DeleteExpenseUseCase(repositoryWriteOnly,repository, loggedUser, unitOfWork);
        }
    }
}
