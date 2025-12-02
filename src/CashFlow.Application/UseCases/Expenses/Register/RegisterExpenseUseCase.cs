using AutoMapper;
using CashFlow.Application.Services.LoggerUser;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Enums;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Exception.ExceptionsBase;
using System.Threading.Tasks;

namespace CashFlow.Application.UseCases.Expenses.Register
{
    public class RegisterExpenseUseCase(
        IExpensesWriteOnlyRepository repository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILoggerUser loggerUser
        ) : IRegisterExpenseUseCase
    {
        public async Task<ResponseExpenseJson> Execute(RequestExpenseJson request)
        {
            await Validate(request);

            var user = await loggerUser.Get();

            var entity = mapper.Map<Expense>(request);
            entity.UserId = user.Id;

            await repository.Add(entity);

            await unitOfWork.Commit();

            var response = mapper.Map<ResponseExpenseJson>(entity);

            return response;
        }

        private async Task Validate(RequestExpenseJson request)
        {
            var validator = new ExpenseValidator();

            var result = validator.Validate(request);

            if (!result.IsValid)
            {
                var errorMessages = result.Errors.Select(f => f.ErrorMessage).ToList();

                throw new ErrorOnValidationException(errorMessages);
            }


        }
    }
}
