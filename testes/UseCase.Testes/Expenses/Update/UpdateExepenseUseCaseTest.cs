using CashFlow.Application.UseCases.Expenses.Update;
using CashFlow.Domain.Entities;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Request;

namespace UseCase.Tests.Expenses.Update
{
    public class UpdateExepenseUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            var loggedUser = UserBuilder.Build();
            var request = RequestRegisterExpenseJsonBuilder.Build();
            var expense = ExpenseBuilder.Build(loggedUser);
            var useCase = CreateUseCase(loggedUser, expense);

        }
        [Fact]
        public async Task Error_Title_Empty()
        {
            var loggedUser = UserBuilder.Build();
            var request = RequestRegisterExpenseJsonBuilder.Build();
            var expense = ExpenseBuilder.Build(loggedUser);
            request.Title = string.Empty;
            var useCase = CreateUseCase(loggedUser, expense);

            // Act
            var exception = await Assert.ThrowsAsync<ErrorOnValidationException>(() =>
                             useCase.Execute(expense.Id, request));

            Assert.Contains(ResourceErrorMessages.TITLE_REQUIRED, exception.GetErros());
        }

        [Fact]
        public async Task Error_Expense_Not_Fount()
        {
            var loggedUser = UserBuilder.Build();
            var request = RequestRegisterExpenseJsonBuilder.Build();
            var expense = ExpenseBuilder.Build(loggedUser);
            var useCase = CreateUseCase(loggedUser);

            // Act
            var exception = await Assert.ThrowsAsync<NotFoundException>(() =>
                             useCase.Execute(1,request));

            Assert.Contains(ResourceErrorMessages.EXPENSE_NOT_FOUND, exception.GetErros());
        }

        private UpdateExpenseUseCase CreateUseCase(CashFlow.Domain.Entities.User user,Expense? expense = null)
        {
            var repository = new ExpensesUpdateOnlyRepositoryBuilder().GetById(user, expense).Build();

            var mapper = MapperBuilder.Build();

            var unitOfWork = UnitOfWorkBuilder.Build();

            var loggedUser = LoggedUserBuilder.Build(user);

            return new UpdateExpenseUseCase(mapper, repository, loggedUser, unitOfWork);
        }
    }
}
