using CashFlow.Application.UseCases.Reports.PDF;
using CashFlow.Domain.Entities;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;

namespace UseCase.Tests.Expenses.Reports.PDF
{
    public class GeneateExpensesReportPdfUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            var loggedUser = UserBuilder.Build();
            var expenses = ExpenseBuilder.Collection(loggedUser);

            var useCase = CreateUseCase(loggedUser, expenses);

            var result = await useCase.Execute(DateOnly.FromDateTime(DateTime.Now));

            Assert.NotNull(result);

        }

        [Fact]
        public async Task Success_Empty()
        {
            var loggedUser = UserBuilder.Build();         

            var useCase = CreateUseCase(loggedUser,new List<Expense>());

            var result = await useCase.Execute(DateOnly.FromDateTime(DateTime.Now));

            Assert.Empty(result);
        }


        private GenerateExpensesReportPDFUseCase CreateUseCase(CashFlow.Domain.Entities.User user, List<Expense> expenses)
        {
            var repository = new ExpensesReadOnlyRepositoryBuilder().FilterByMonth(user, expenses).Build();

            var loggedUser = LoggedUserBuilder.Build(user);

            return new GenerateExpensesReportPDFUseCase(repository, loggedUser);

        }
    }
}
