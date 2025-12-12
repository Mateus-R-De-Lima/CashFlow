using CashFlow.Application.UseCases.Expenses.GetAll;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Entities;
using CashFlow.Exception;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;


namespace UseCase.Tests.Expenses.GetAll
{
    public class GetAllExepenseUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            var loggedUser = UserBuilder.Build();
            var expenses = ExpenseBuilder.Collection(loggedUser);

            var useCase = CreateUseCase(loggedUser, expenses);

            var result = await useCase.Execute();

            Assert.NotNull(result);

            // Assert: objeto e lista não nulos
            Assert.NotNull(result);
            Assert.NotNull(result.Expenses);

            // Assert:  Contagem igual ao mock           
            Assert.Equal(expenses.Count, result.Expenses.Count);

            Assert.All<ResponseShortExpenseJson>(result.Expenses, item =>
            {
                
                Assert.False(string.IsNullOrWhiteSpace(item.Title),ResourceErrorMessages.TITLE_REQUIRED);
                Assert.True(item.Amount >= 0m, ResourceErrorMessages.AMOUNT_MUST_BE_GREATER_THAN_ZERO);
            });

            // Valida que cada Expense mockado foi mapeado corretamente
            foreach (var expense in expenses)
            {
                // assumindo que Expense.Id é long e campos Title/Amount existem
                var mapped = result.Expenses.SingleOrDefault(x => x.Id == expense.Id);
                Assert.NotNull(mapped);

                Assert.Equal(expense.Title, mapped.Title);
                Assert.Equal(expense.Amount, mapped.Amount);
            }

        }

        private GetAllExpenseUseCase CreateUseCase(CashFlow.Domain.Entities.User user, List<Expense> expenses)
        {
            var repository = new ExpensesReadOnlyRepositoryBuilder().GetAll(user, expenses)
                                                                     .Build();

            var mapper = MapperBuilder.Build();
            var loggedUser = LoggedUserBuilder.Build(user);

            return new GetAllExpenseUseCase(repository, loggedUser, mapper);
        }
    }
}
