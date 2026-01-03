using CashFlow.Application.UseCases.Expenses;
using CashFlow.Communication.Enums;
using CashFlow.Exception;
using CommonTestUtilities.Request;

namespace Validator.Tests.Expenses.Register
{
    public class ExpenseValidatorTests
    {
        [Fact]
        public void Sucesso()
        {

            // Arrange
            var validator = new ExpenseValidator();
            var request = RequestRegisterExpenseJsonBuilder.Build();

            //Act
            var result = validator.Validate(request);

            //Assert
            Assert.True(result.IsValid);
        }


        [Theory]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData(null)]
        public void Error_Title_Empty(string title)
        {

            // Arrange
            var validator = new ExpenseValidator();
            var request = RequestRegisterExpenseJsonBuilder.Build();
            request.Title = title;            

            //Act
            var result = validator.Validate(request);

            //Assert
            Assert.False(result.IsValid); 
          
            Assert.Contains(result.Errors, e => e.ErrorMessage.Contains(ResourceErrorMessages.TITLE_REQUIRED)); // garante que a mensagem contem algo esperado

        }



        [Fact]
        public void Error_Date_Future()
        {

            // Arrange
            var validator = new ExpenseValidator();
            var request = RequestRegisterExpenseJsonBuilder.Build();
            request.Date = DateTime.UtcNow.AddDays(1);

            //Act
            var result = validator.Validate(request);

            //Assert
            Assert.False(result.IsValid); // garante que a validação falhou           
            Assert.Contains(result.Errors, e => e.ErrorMessage.Contains(ResourceErrorMessages.EXPENSES_CANNOT_FOR_THE_FUTURE)); // garante que a mensagem contem algo esperado

        }

        [Fact]
        public void Error_Payment_Type_Invalid()
        {

            // Arrange
            var validator = new ExpenseValidator();
            var request = RequestRegisterExpenseJsonBuilder.Build();
            request.PaymentType = (PaymentTypes)700;
            //Act
            var result = validator.Validate(request);

            //Assert
            Assert.False(result.IsValid); // garante que a validação falhou           
            Assert.Contains(result.Errors, e => e.ErrorMessage.Contains(ResourceErrorMessages.PAYMENT_TYPE_INVALID)); // garante que a mensagem contem algo esperado

        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-2)]
        [InlineData(-7)]
        public void Error_Amount_Invalid(decimal amount)
        {

            // Arrange
            var validator = new ExpenseValidator();
            var request = RequestRegisterExpenseJsonBuilder.Build();
            request.Amount = amount;
            //Act
            var result = validator.Validate(request);

            //Assert
            Assert.False(result.IsValid); // garante que a validação falhou           
            Assert.Contains(result.Errors, e => e.ErrorMessage.Contains(ResourceErrorMessages.AMOUNT_MUST_BE_GREATER_THAN_ZERO)); // garante que a mensagem contem algo esperado

        }

        [Fact]
        public void Error_Tag_Invalid()
        {
            var validator = new ExpenseValidator();
            var request = RequestRegisterExpenseJsonBuilder.Build();
            request.Tags.Add((Tag)999);

            //Act
            var result = validator.Validate(request);

            //Assert
            Assert.False(result.IsValid); // garante que a validação falhou
            Assert.Contains(result.Errors, e => e.ErrorMessage.Contains(ResourceErrorMessages.TAG_TYPE_NOT_SUPPORTED)); // garante que a mensagem contem algo esperado
        }


    }
}
