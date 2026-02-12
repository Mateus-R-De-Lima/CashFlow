using CashFlow.Application.Services.LoggerUser;
using CashFlow.Communication.Requests;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.User;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using FluentValidation.Results;
using StackExchange.Redis;

namespace CashFlow.Application.UseCases.User.UpdateProfile
{
    public class UpdateUserProfileUseCase(ILoggerUser loggerUser,
                                          IUserUpdateOnlyRepository userUpdateOnlyRepository,
                                          IUserReadOnlyRepository userReadOnlyRepository,
                                          IUnitOfWork unitOfWork,
                                          IConnectionMultiplexer redis) : IUpdateUserProfileUseCase
    {


        public async Task Execute(RequestUpdateProfileUserJson request)
        {
            var user = await loggerUser.Get();

            await Validate(request, user.Email);

            var userEntity = await userUpdateOnlyRepository.GetById(user.Id);

            userEntity!.Name = request.Name;
            userEntity.Email = request.Email;

            userUpdateOnlyRepository.Update(userEntity);

            await unitOfWork.Commit();

            var redisDb = redis.GetDatabase();
            await redisDb.KeyDeleteAsync($"userId:{user.Id}");

        }

        private async Task Validate(RequestUpdateProfileUserJson request, string email)
        {
            var validator = new UpdateUserValidator();

            var result = validator.Validate(request);         

            if (!email.Equals(request.Email))
            {
                var userExist = await userReadOnlyRepository.ExistActiveUserWithEmail(request.Email);
                if (userExist)
                    result.Errors.Add(new ValidationFailure("Email", ResourceErrorMessages.EMAIL_ALREADY_REGISTERED));

            }

            if (!result.IsValid)
            {
                var errorMessagens = result.Errors.Select(f => f.ErrorMessage).ToList();
                throw new ErrorOnValidationException(errorMessagens);
            }


        }
    }
}
