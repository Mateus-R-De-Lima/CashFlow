using CashFlow.Application.UseCases.User.UpdateProfile;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Request;
using System.Threading.Tasks;

namespace UseCase.Tests.User.Profile
{
    public class UpdateProfileUseCaseTest
    {

        [Fact]
        public void Success()
        {
            var user = UserBuilder.Build();
            var request = RequestUpdateProfileUserJsonBuilder.Build();

            var useCase = CreateUseCase(user);

            var result = useCase.Execute(request);

            Assert.NotNull(result);

        }

        [Theory]
        [InlineData("")]
        [InlineData("         ")]
        [InlineData(null)]
        public async Task Error_Name_Empty(string name)
        {
            var user = UserBuilder.Build();
            var request = RequestUpdateProfileUserJsonBuilder.Build();
            request.Name = name;
            var useCase = CreateUseCase(user);
            var exception = await Assert.ThrowsAsync<ErrorOnValidationException>(() =>
                                       useCase.Execute(request));

            Assert.Contains(ResourceErrorMessages.NAME_EMPTY, exception.GetErros());
        }


        [Fact]
        public async void Error_Email_Invalid()
        {
            var user = UserBuilder.Build();
            var request = RequestUpdateProfileUserJsonBuilder.Build();
            request.Email = "invalid-email-format";
            var useCase = CreateUseCase(user);
            var exception = await Assert.ThrowsAsync<ErrorOnValidationException>(() =>
                                      useCase.Execute(request));

            Assert.Contains(ResourceErrorMessages.EMAIL_INVALID, exception.GetErros()); ;


        }

        [Fact]
        public async void Error_Email_Already_Exists()
        {
            var user = UserBuilder.Build();
            var request = RequestUpdateProfileUserJsonBuilder.Build();
            var useCase = CreateUseCase(user, request.Email);
            var exception = await Assert.ThrowsAsync<ErrorOnValidationException>(() =>
                                      useCase.Execute(request));
            Assert.Contains(ResourceErrorMessages.EMAIL_ALREADY_REGISTERED, exception.GetErros()); ;

        }
        private UpdateUserProfileUseCase CreateUseCase(CashFlow.Domain.Entities.User user, string? email = null)
        {
            var unitOfWork = UnitOfWorkBuilder.Build();
            var updateRepository = UserUpdateOnlyRepositoryBuilder.Build(user);
            var loogedUser = LoggedUserBuilder.Build(user);
            var readRepository = new UserReadOnlyRepositoryBuilder();

            if (!string.IsNullOrWhiteSpace(email))
                readRepository.ExistActiveUserWithEmail(email);

            return new UpdateUserProfileUseCase(loogedUser,
                                                updateRepository,
                                                readRepository.Build(),
                                                unitOfWork);

        }

    }
}
