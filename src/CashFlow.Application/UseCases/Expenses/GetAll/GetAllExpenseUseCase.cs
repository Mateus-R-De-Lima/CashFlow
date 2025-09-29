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

        public async Task<ResponseExpenseJson> Execute()
        {
            var result = await repository.GetAll();


            return new ResponseExpenseJson
            {
                Expenses = mapper.Map<List<ResponseShortExpenseJson>>(result)
            };
        }


    }
}
