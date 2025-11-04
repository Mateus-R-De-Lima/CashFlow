using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Security.Cryotography;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.User.Register
{
    public class RegisterUserUseCase(IMapper mapper, IPasswordEncripter passwordEncripter) : IRegisterUserUseCase
    {

        public async Task<ResponseRegisteredUserJson> Execute(RequestUserJson request)
        {
            Validate(request);
            
            var user = mapper.Map<Domain.Entities.User>(request);

            user.Password = passwordEncripter.Encrypt(user.Password);

            return new ResponseRegisteredUserJson
            {
                Name = user.Name,
            };
        }

        private void Validate(RequestUserJson request)
        {
            var result = new RegiserUserValidator().Validate(request);

            if (!result.IsValid)
            {
                var errorMassages = result.Errors.Select(f => f.ErrorMessage).ToList();

                throw new ErrorOnValidationException(errorMassages);
            }
        }
    }
}
