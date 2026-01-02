using CashFlow.Application.UseCases.User.ChangePassword;
using CashFlow.Exception;
using CommonTestUtilities.Request;

namespace Validator.Tests.User.ChangePassword
{
    public class ChangePasswordValidatorTest
    {

        [Fact]
        public void Success()
        {
            var validator = new ChangePasswordValidator();

            var request = RequestChangePasswordJsonBuilder.Build();

            var result = validator.Validate(request);

            Assert.True(result.IsValid);

        }


        [Theory]
        [InlineData("")]
        [InlineData("        ")]
        [InlineData(null)]
        public void Error_NewPassword_Empty(string newPassword)
        {
            var validator = new ChangePasswordValidator();

            var request = RequestChangePasswordJsonBuilder.Build();
            request.NewPassword = newPassword;

            var result = validator.Validate(request);

            Assert.False(result.IsValid);

            Assert.Contains(result.Errors, e => e.ErrorMessage == ResourceErrorMessages.INVALID_PASSWORD);
        }
    }
}
