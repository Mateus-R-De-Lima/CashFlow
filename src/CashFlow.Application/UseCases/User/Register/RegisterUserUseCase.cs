using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Entities;
using CashFlow.Exception.ExceptionsBase;
using System.ComponentModel.DataAnnotations;

namespace CashFlow.Application.UseCases.User.Register
{
    public class RegisterUserUseCase(IMapper mapper) : IRegisterUserUseCase
    {

        public async Task<ResponseRegisteredUserJson> Execute(RequestUserJson request)
        {
            Validate(request);
            var user = mapper.Map<Domain.Entities.User>(request);

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
