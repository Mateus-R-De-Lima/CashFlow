using CashFlow.Application.UseCases.User.UpdateProfile;
using CashFlow.Exception;
using CommonTestUtilities.Request;

namespace Validator.Tests.User.Profile
{
    public class UpdateProfileValidatorTest
    {
        [Fact]
        public void Success()
        {
            var validator = new UpdateUserValidator();
            var request = RequestUpdateProfileUserJsonBuilder.Build();

            var result = validator.Validate(request);

            Assert.True(result.IsValid);
        }

        [Theory]
        [InlineData("")]
        [InlineData("         ")]
        [InlineData(null)]
        public void Error_Name_Empty(string name)
        {
            var validator = new UpdateUserValidator();
            var request = RequestUpdateProfileUserJsonBuilder.Build();
            request.Name = name;
            var result = validator.Validate(request);
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.ErrorMessage.Contains(ResourceErrorMessages.NAME_EMPTY));
        }


        [Fact]
        public void Error_Email_Invalid()
        {
            var validator = new UpdateUserValidator();
            var request = RequestUpdateProfileUserJsonBuilder.Build();
            request.Email = "invalid-email-format";
            var result = validator.Validate(request);
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.ErrorMessage.Contains(ResourceErrorMessages.EMAIL_INVALID));


        }
    }
}
