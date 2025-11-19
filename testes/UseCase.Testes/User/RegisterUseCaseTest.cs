using CashFlow.Application.UseCases.User.Register;
using CashFlow.Domain.Repositories.User;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Request;
using CommonTestUtilities.Token;

namespace UseCase.Tests.User
{
    public class RegisterUseCaseTest
    {

        [Fact]
        public async Task Success()
        {
            // Arrange
            var request = RequestRegisterUserJsonBuilder.Build();
            var useCase = CreateUseCase();
            // Act
            var result = await useCase.Execute(request);

            Assert.NotNull(result);

            Assert.Equal(request.Name, result.Name);
        }

        [Fact]
        public async Task Error_Name_Empty()
        {
            // Arrange
            var request = RequestRegisterUserJsonBuilder.Build();
            request.Name = string.Empty;
            var useCase = CreateUseCase();
            // Act
            var exception = await Assert.ThrowsAsync<ErrorOnValidationException>(() =>
                             useCase.Execute(request));

            Assert.Contains(ResourceErrorMessages.NAME_EMPTY, exception.GetErros());

        }
        private RegisterUserUseCase CreateUseCase()
        {
            var mapper = MapperBuilder.Build();

            var unitOfWork = UnitOfWorkBuilder.Build();

            var readOnly = new UserReadOnlyRepositoryBuilder().Build();

            var writeOnly = UserWriteOnlyRepositoryBuilder.Build();

            var passowrdEncripter = PasswordEncripterBuilder.Build();

            var jwtTokenGenaratorBuilder = JwtTokenGenaratorBuilder.Buid();

            return new RegisterUserUseCase(mapper,
                                           readOnly,
                                           writeOnly,
                                           unitOfWork,
                                           passowrdEncripter,
                                           jwtTokenGenaratorBuilder);
        }



    }
}
