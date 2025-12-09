using CashFlow.Application.UseCases.Expenses.Register;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Request;

namespace UseCase.Tests.Expenses.Register
{
    public class RegisterExepenseUseCaseTest
    {

        [Fact]
        public async Task Success()
        {
            var loggedUser = UserBuilder.Build();
            var request = RequestRegisterExpenseJsonBuilder.Build();
            var useCase = CreateUseCase(loggedUser);

            var result = await useCase.Execute(request);

            Assert.NotNull(result);

            Assert.Equal(request.Title, result.Title);

        }

        [Fact]
        public async Task Error_Name_Empty()
        {
            // Arrange
            var loggedUser = UserBuilder.Build();
            var request = RequestRegisterExpenseJsonBuilder.Build();
            request.Title = string.Empty;
            var useCase = CreateUseCase(loggedUser);
            // Act
            var exception = await Assert.ThrowsAsync<ErrorOnValidationException>(() =>
                             useCase.Execute(request));

            Assert.Contains(ResourceErrorMessages.TITLE_REQUIRED, exception.GetErros());

        }
        private RegisterExpenseUseCase CreateUseCase(CashFlow.Domain.Entities.User user)
        {
            var repository = ExpensesWriteOnlyRepositoryBuilder.Build();

            var mapper = MapperBuilder.Build();

            var unitOfWork = UnitOfWorkBuilder.Build();

            var loggedUser = LoggedUserBuilder.Build(user);

            return new RegisterExpenseUseCase(repository, unitOfWork, mapper, loggedUser);

        }
    }
}

