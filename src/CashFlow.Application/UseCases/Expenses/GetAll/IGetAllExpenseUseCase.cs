using CashFlow.Communication.Responses;

namespace CashFlow.Application.UseCases.Expenses.GetAll
{
    public interface IGetAllExpenseUseCase
    {
        Task<ResponsesExpenseJson> Execute();
    }
}