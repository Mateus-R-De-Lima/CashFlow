using CashFlow.Application.UseCases.Expenses.Register;
using CashFlow.Communication.Enums;
using CashFlow.Communication.Requests;

namespace Validator.Tests.Expenses.Register
{
    public class RegisterExpenseValidatorTests
    {
        [Fact]
        public void Sucesso()
        {

            // Arrange
            var validator = new RegisterExpenseValidator();
            var request = new RequestRegisterExpenseJson
            {
                Amount = 100,
                Date = DateTime.Now.AddDays(-1),
                Title = "Title",
                Description = "Description",
                PaymentType = PaymentTypes.EletronicTransfer
            };

            //Act

            var result = validator.Validate(request);

            //Assert
            Assert.True(result.IsValid);
        }
    }
}
