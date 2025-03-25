﻿using CashFlow.Communication.Enums;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using System.Data;

namespace CashFlow.Application.UseCases.Expenses.Register
{
    public class RegisterExpenseUseCase
    {
        public ResponseRegisteredJson Execute(RequestRegisterExpenseJson request)
        {
            Validate(request);

            return new ResponseRegisteredJson();
        }

        /// <summary>
        /// Validar os dados de request
        /// </summary>
        /// <param name="request"></param>
        private void Validate(RequestRegisterExpenseJson request)
        {   
            var titleIsEmpty = string.IsNullOrWhiteSpace(request.Title);
            if (titleIsEmpty)
            {
                throw new ArgumentException("The title is required.");
            }

            if(request.Amount <= 0)
            {
                throw new ArgumentException("The Amount must be greater than zero.");
            }

            var result = DateTime.Compare(request.Date,DateTime.UtcNow);
            if(result > 0)
            {
                throw new ArgumentException("Expenses cannot be for the future");
            }

            var paymentTypeIsValid = Enum.IsDefined(typeof(PaymentTypes), request.PaymentType);
            if (!paymentTypeIsValid)
            {
                throw new ArgumentException("Payment Type is not valid.");
            }
        }
    }
}
