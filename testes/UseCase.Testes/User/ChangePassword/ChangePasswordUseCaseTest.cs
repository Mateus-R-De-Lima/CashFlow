using CashFlow.Application.UseCases.User.ChangePassword;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Request;

namespace UseCase.Tests.User.ChangePassword
{
    public class ChangePasswordUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            var user = UserBuilder.Build();
            var request = RequestChangePasswordJsonBuilder.Build();

            var useCase = CreateUseCase(user, request.Password);
            //Act
            await useCase.Execute(request);
            //Assert
            Assert.True(true);
        }

        [Fact]
        public async Task Error_NewPassword_Empty()
        {
            var user = UserBuilder.Build();

            var request = RequestChangePasswordJsonBuilder.Build();
            request.NewPassword = string.Empty;

            var useCase = CreateUseCase(user, request.Password);

            var exception = await Assert.ThrowsAsync<ErrorOnValidationException>(() =>
                                       useCase.Execute(request));
            Assert.Contains(ResourceErrorMessages.INVALID_PASSWORD, exception.GetErros());

        }

        [Fact]
        public async Task Error_CurrentPassword_Different()
        {
            var user = UserBuilder.Build();
            var request = RequestChangePasswordJsonBuilder.Build();         
            var useCase = CreateUseCase(user);
            var exception = await Assert.ThrowsAsync<ErrorOnValidationException>(() =>
                                       useCase.Execute(request));
            Assert.Contains(ResourceErrorMessages.PASSWORD_DIFFERENT_CURRENT_PASSWORD, exception.GetErros());
        }

        private static ChangePasswordUseCase CreateUseCase(CashFlow.Domain.Entities.User user, string? password = null)
        {
            var unitOfWork = UnitOfWorkBuilder.Build();
            var userUpdateOnlyRepository = UserUpdateOnlyRepositoryBuilder.Build(user);
            var loggedUser = LoggedUserBuilder.Build(user);
            var passwordEncripter = new PasswordEncripterBuilder().Verify(password).Build();

            return new ChangePasswordUseCase(loggedUser,
                                           userUpdateOnlyRepository,
                                           unitOfWork,
                                           passwordEncripter);

        }
    }
}
