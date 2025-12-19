using CashFlow.Application.UseCases.User.GetProfile;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;

namespace UseCase.Tests.User.Profile
{
    public class GetUserProfileUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            var loggedUser = UserBuilder.Build();
            var useCase = CreateUseCase(loggedUser);
            var result = await useCase.Execute();
            Assert.NotNull(result);
            Assert.Equal(loggedUser.Name, result.Name);
            Assert.Equal(loggedUser.Email, result.Email);
        }

        private GetUserProfileUseCase CreateUseCase(CashFlow.Domain.Entities.User user)
        {
            var mapper = MapperBuilder.Build();
            var loggedUser = LoggedUserBuilder.Build(user);

            return new GetUserProfileUseCase(loggedUser, mapper);
        }
    }
}
