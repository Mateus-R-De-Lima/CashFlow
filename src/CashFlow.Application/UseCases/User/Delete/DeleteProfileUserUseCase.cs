using CashFlow.Application.Services.LoggerUser;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.User;

namespace CashFlow.Application.UseCases.User.Delete
{
    public class DeleteProfileUserUseCase(ILoggerUser loggerUser,
                                          IUserWriteOnlyRepository userWriteOnlyRepository,
                                          IUnitOfWork unitOfWork) : IDeleteProfileUserUseCase
    {

        public async Task Execute()
        {
            var loggedUser = await loggerUser.Get();

            await userWriteOnlyRepository.Delete(loggedUser);

            await unitOfWork.Commit();
        }
    }
}
