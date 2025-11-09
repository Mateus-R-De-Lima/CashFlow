using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Repositories.User;
using CashFlow.Domain.Security.Cryotography;
using CashFlow.Domain.Security.Token;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.DoLogin
{
    public class DoLoginUseCase(IUserReadOnlyRepository repository,
                                IPasswordEncripter passwordEncripter,
                                IAccessTokenGenarator accessTokenGenarator) : IDoLoginUseCase
    {

        public async Task<ResponseRegisteredUserJson> Execute(RequestLoginJson request)
        {

            var user = await repository.GetUserByEmail(request.Email);
            if (user is null)
                throw new InvalidLoginException();

            var passwordMatch = passwordEncripter.Verify(request.Password, user.Password);

            if (!passwordMatch)
                throw new InvalidLoginException();


            return new ResponseRegisteredUserJson
            {
                Name = user.Name,
                Token = accessTokenGenarator.Generate(user)
            };
        }


    }
}
