using AutoMapper;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Expenses;

namespace CashFlow.Application.UseCases.Expenses.GetAll
{
    public class GetAllExpenseUseCase(
          IExpensesRepository repository,
          IMapper mapper
        ) : IGetAllExpenseUseCase
    {

        public async Task<ResponsesExpenseJson> Execute()
        {
            var result = await repository.GetAll();


            return new ResponsesExpenseJson
            {
                Expenses = mapper.Map<List<ResponseShortExpenseJson>>(result)
            };
        }


    }
}
