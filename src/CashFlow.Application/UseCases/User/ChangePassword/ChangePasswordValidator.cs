using CashFlow.Communication.Requests;
using FluentValidation;

namespace CashFlow.Application.UseCases.User.ChangePassword
{
    public class ChangePasswordValidator : AbstractValidator<RequestChangePasswordUserJson>
    {
        public ChangePasswordValidator()
        {
            RuleFor(p => p.NewPassword).SetValidator(new PassordValidator<RequestChangePasswordUserJson>());
        }
    }
}
