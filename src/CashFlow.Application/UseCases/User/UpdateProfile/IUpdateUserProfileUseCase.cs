using CashFlow.Communication.Requests;

namespace CashFlow.Application.UseCases.User.UpdateProfile
{
    public interface IUpdateUserProfileUseCase
    {
        Task Execute(RequestUpdateProfileUserJson request);
    }
}