using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.User;
using CashFlow.Domain.Security.Cryotography;
using CashFlow.Domain.Security.Token;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using FluentValidation.Results;

namespace CashFlow.Application.UseCases.User.Register
{
    public class RegisterUserUseCase(IMapper mapper,
                                     IUserReadOnlyRepository userReadOnlyRepository,
                                     IUserWriteOnlyRepository userWriteOnlyRepository,
                                     IUnitOfWork unitOfWork,
                                     IPasswordEncripter passwordEncripter,
                                     IAccessTokenGenarator accessTokenGenarator) : IRegisterUserUseCase
    {

        public async Task<ResponseRegisteredUserJson> Execute(RequestUserJson request)
        {
            await Validate(request);

            var user = mapper.Map<Domain.Entities.User>(request);

            user.Password = passwordEncripter.Encrypt(request.Password);
            user.UserIdentifier = Guid.NewGuid();

            await userWriteOnlyRepository.Add(user);

            await unitOfWork.Commit();



            return new ResponseRegisteredUserJson
            {
                Name = user.Name,
                Token = accessTokenGenarator.Generate(user)
            };
        }

        private async Task Validate(RequestUserJson request)
        {
            var result = new RegiserUserValidator().Validate(request);

            if (!result.IsValid)
            {
                var errorMassages = result.Errors.Select(f => f.ErrorMessage).ToList();

                throw new ErrorOnValidationException(errorMassages);
            }

            var emailExist = await userReadOnlyRepository.ExistActiveUserWithEmail(request.Email);

            if (emailExist)
                throw new ErrorOnValidationException([ResourceErrorMessages.EMAIL_ALREADY_REGISTERED]);




        }
    }
}
