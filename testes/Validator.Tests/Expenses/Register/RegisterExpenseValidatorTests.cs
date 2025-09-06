using CashFlow.Application.UseCases.Expenses.Register;
using CashFlow.Communication.Enums;
using CashFlow.Communication.Requests;
using CommonTestUtilities.Request;

namespace Validator.Tests.Expenses.Register
{
    public class RegisterExpenseValidatorTests
    {
        [Fact]
        public void Sucesso()
        {

            // Arrange
            var validator = new RegisterExpenseValidator();
            var request = RequestRegisterExpenseJsonBuilder.Build();

            //Act
            var result = validator.Validate(request);

            //Assert
            Assert.True(result.IsValid);
        }
    }
}
