using CashFlow.Application.UseCases.DoLogin;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Request;
using CommonTestUtilities.Token;

namespace UseCase.Tests.User.DoLogin
{
    public class DoLoginUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            var user = UserBuilder.Build();

            var request = RequestLoginJsonBuilder.Build();
            request.Email = user.Email;

            var useCase = CreateUseCase(user, request.Password);

            // Act
            var result = await useCase.Execute(request);

            Assert.NotNull(result);

        }

        [Fact]
        public async Task Error_User_Not_Found()
        {
            var user = UserBuilder.Build();

            var request = RequestLoginJsonBuilder.Build();

            var useCase = CreateUseCase(user, request.Password);

            var exception = await Assert.ThrowsAsync<InvalidLoginException>(() =>
                            useCase.Execute(request));

            Assert.Contains(ResourceErrorMessages.EMAIL_OR_PASSWORD_INVALID, exception.GetErros());
                                
        }

        [Fact]
        public async Task Error_Password_Not_Match()
        {
            var user = UserBuilder.Build();

            var request = RequestLoginJsonBuilder.Build();
            request.Email = user.Email;
            var useCase = CreateUseCase(user);

            var exception = await Assert.ThrowsAsync<InvalidLoginException>(() =>
                          useCase.Execute(request));

            Assert.Contains(ResourceErrorMessages.EMAIL_OR_PASSWORD_INVALID, exception.GetErros());

        }

        private DoLoginUseCase CreateUseCase(CashFlow.Domain.Entities.User user, string? password = null)
        {
            var passwordEncripter = new PasswordEncripterBuilder().Verify(password).Build();
            var tokenGenerator = JwtTokenGenaratorBuilder.Build();
            var readRepository = new UserReadOnlyRepositoryBuilder().GetUserByEmail(user).Build();

            return new DoLoginUseCase(readRepository, passwordEncripter, tokenGenerator);
        }


    }
}
