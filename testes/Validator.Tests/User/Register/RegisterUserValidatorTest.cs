using CashFlow.Application.UseCases.User.Register;
using CashFlow.Exception;
using CommonTestUtilities.Request;

namespace Validator.Tests.User.Register
{
    public class RegisterUserValidatorTest
    {
        [Fact]
        public void Success()
        {
            // Arrange
            var validator = new RegiserUserValidator();
            var request = RequestRegisterUserJsonBuilder.Build();

            // Act
            var result = validator.Validate(request);

            //Assert

            Assert.True(result.IsValid);
        }


        [Theory]
        [InlineData("")]
        [InlineData("       ")]
        [InlineData(null)]
        public void Error_name_Invalid(string name)
        {
            // Arrange
            var validator = new RegiserUserValidator();
            var request = RequestRegisterUserJsonBuilder.Build();
            request.Name = name;

            // Act
            var result = validator.Validate(request);

            //Assert

            Assert.False(result.IsValid);

            Assert.Contains(result.Errors, e => e.ErrorMessage.Contains(ResourceErrorMessages.NAME_EMPTY));
        }
    }
}
