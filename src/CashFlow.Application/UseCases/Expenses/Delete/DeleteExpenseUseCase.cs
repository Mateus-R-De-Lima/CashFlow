using AutoMapper;
using CashFlow.Application.UseCases.Expenses.GetById;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlow.Application.UseCases.Expenses.Delete
{
    public class DeleteExpenseUseCase(IExpensesWriteOnlyRepository repository, IUnitOfWork unitOfWork) : IDeleteExpenseUseCase
    {

        public async Task Execute(long id)
        {
            var result = await repository.Delete(id);

            if (result.Equals(false))
                throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);

            await unitOfWork.Commit();

        }


    }
}