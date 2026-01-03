using CashFlow.Application.UseCases.Expenses.GetById;
using CashFlow.Communication.Enums;
using CashFlow.Domain.Entities;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;

namespace UseCase.Tests.Expenses.GetById
{
    public class GetByIdExepenseUseCaseTest
    {

        [Fact]
        public async Task Success()
        {
            var loggedUser = UserBuilder.Build();
            var expense = ExpenseBuilder.Build(loggedUser);
            var useCase = CreateUseCase(loggedUser, expense);

            var result = await useCase.Execute(expense.Id);
            Assert.NotNull(result);
                       

            // Validando campos essenciais
            Assert.False(string.IsNullOrWhiteSpace(result.Title));
            Assert.False(string.IsNullOrWhiteSpace(result.Description));
            
            Assert.True(result.Amount >= 0);           

            Assert.NotNull(result.Tags);
           

            // Data válida
            Assert.True(result.Date != default);

            // PaymentType válido
            Assert.True(Enum.IsDefined(typeof(PaymentTypes), result.PaymentType));

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

        private GetByIdExpenseUseCase CreateUseCase(CashFlow.Domain.Entities.User user, Expense? expense = null)
        {
            var repository = new ExpensesReadOnlyRepositoryBuilder().GetById(user,expense).Build();
            var mapper = MapperBuilder.Build();
            var loggedUser = LoggedUserBuilder.Build(user);

            return new GetByIdExpenseUseCase(repository, loggedUser, mapper);
        }
    }
}
