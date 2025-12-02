using AutoMapper;
using CashFlow.Application.Services.LoggerUser;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Expenses;

namespace CashFlow.Application.UseCases.Expenses.GetAll
{
    public class GetAllExpenseUseCase(
          IExpensesReadOnlyRepository repository,
          ILoggerUser loggerUser,
          IMapper mapper
        ) : IGetAllExpenseUseCase
    {

        public async Task<ResponsesExpenseJson> Execute()
        {
            var user = await loggerUser.Get();

            var result = await repository.GetAll(user);

            return new ResponsesExpenseJson
            {
                Expenses = mapper.Map<List<ResponseShortExpenseJson>>(result)
            };
        }


    }
}
