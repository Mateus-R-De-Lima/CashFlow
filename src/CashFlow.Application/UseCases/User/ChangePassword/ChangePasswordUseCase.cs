using CashFlow.Application.Services.LoggerUser;
using CashFlow.Communication.Requests;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.User;
using CashFlow.Domain.Security.Cryotography;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using System.Threading.Tasks;

namespace CashFlow.Application.UseCases.User.ChangePassword
{
    public class ChangePasswordUseCase(ILoggerUser loggerUser,
                                       IUserUpdateOnlyRepository userUpdateOnlyRepository,
                                       IUnitOfWork unitOfWork,
                                       IPasswordEncripter passwordEncripter) : IChangePasswordUseCase
    {

        public async Task Execute(RequestChangePasswordUserJson request)
        {
            var loggedUser = await loggerUser.Get();

            Validate(request, loggedUser);
            var user = await userUpdateOnlyRepository.GetById(loggedUser.Id);
            user.Password = passwordEncripter.Encrypt(request.NewPassword);
            userUpdateOnlyRepository.Update(user);
            await unitOfWork.Commit();

        }

        private void Validate(RequestChangePasswordUserJson request, Domain.Entities.User loggedUser)
        {
            var result = new ChangePasswordValidator().Validate(request);
            if (!result.IsValid)
            {
                var errorMassages = result.Errors.Select(f => f.ErrorMessage).ToList();

                throw new ErrorOnValidationException(errorMassages);
            }

            var passwordMatch = passwordEncripter.Verify(request.Password,loggedUser.Password);

            if (!passwordMatch)
                throw new ErrorOnValidationException([ResourceErrorMessages.PASSWORD_DIFFERENT_CURRENT_PASSWORD]);

        }
    }
}
