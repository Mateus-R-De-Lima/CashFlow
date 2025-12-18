using AutoMapper;
using CashFlow.Application.Services.LoggerUser;
using CashFlow.Communication.Responses;

namespace CashFlow.Application.UseCases.User.Profile
{
    public class GetUserProfileUseCase(ILoggerUser loggerUser, IMapper mapper) : IGetUserProfileUseCase
    {
        public async Task<ResponseUserProfileJson> Execute()
        {
            var user = await loggerUser.Get();
            var response = mapper.Map<ResponseUserProfileJson>(user);
            return response;
        }
    }
}
