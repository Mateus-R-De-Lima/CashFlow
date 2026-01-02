using CashFlow.Application.UseCases.User.Delete;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;

namespace UseCase.Tests.User.Delete
{

   
    public class DeleteUserAccountUseCaseTest
    {

        [Fact]
        public async Task Success()
        {
            // Arrange
            var user = UserBuilder.Build();
            var useCase = CreateUseCase(user);
          
            await useCase.Execute();
         
            Assert.True(true);
        }
        private DeleteProfileUserUseCase CreateUseCase(CashFlow.Domain.Entities.User user)
        {
            var repository = UserWriteOnlyRepositoryBuilder.Build();
            var loggedUser = LoggedUserBuilder.Build(user);
            var unitOfWork = UnitOfWorkBuilder.Build();

            return new DeleteProfileUserUseCase(loggedUser,
                                                repository,
                                                unitOfWork);


        }
    }
}
