using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Enums;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.Expenses.Register
{
    public class RegisterExpenseUseCase(
        IExpensesRepository repository,
        IUnitOfWork unitOfWork
        ) : IRegisterExpenseUseCase
    {
        public  async Task<ResponseRegisteredJson> Execute(RequestRegisterExpenseJson request)
        {
            Validate(request);

            var entity = new Expense
            {
                Amount = request.Amount,
                Date = request.Date,
                Description = request.Description,
                Title = request.Title,
                PaymentType = (PaymentTypes)request.PaymentType
            };

           await repository.Add(entity);

            await unitOfWork.Commit();

            var response = new ResponseRegisteredJson() {
                Title =entity.Title,
                
            };
            return response;
        }

        private void Validate(RequestRegisterExpenseJson request)
        {
            var validator = new RegisterExpenseValidator();

            var result = validator.Validate(request);

            if (!result.IsValid)
            {
                var errorMessages = result.Errors.Select(f => f.ErrorMessage).ToList();

                throw new ErrorOnValidationException(errorMessages);
            }
        }
    }
}
