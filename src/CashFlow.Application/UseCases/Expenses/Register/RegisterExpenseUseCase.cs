using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;

namespace CashFlow.Application.UseCases.Expenses.Register
{
    public class RegisterExpenseUseCase
    {
        public ResponseRegisteredJson Execute(RequestRegisterExpenseJson request)
        {
            // TO DO Validations


            return new ResponseRegisteredJson();
        }
    }
}
