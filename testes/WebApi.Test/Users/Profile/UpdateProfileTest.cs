using CashFlow.Application.UseCases.User.UpdateProfile;
using CashFlow.Domain.Entities;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Request;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Users.Profile
{
    public class UpdateProfileTest : CashFlowClassFixture
    {
        private readonly string METHOD = "api/User";

        private readonly string _token;
        private readonly string _userName;
        private readonly string _userEmail;

        public UpdateProfileTest(CustomWebApplicationFactory customWebApplicationFactory) : base(customWebApplicationFactory)
        {
            _token = customWebApplicationFactory.User_Team_Admin.GetToken();
            _userEmail = customWebApplicationFactory.User_Team_Admin.GetEmail();
            _userName = customWebApplicationFactory.User_Team_Admin.GetName();
        }

        [Fact]
        public async Task Success()
        {
            var user = UserBuilder.Build();
            var request = RequestUpdateProfileUserJsonBuilder.Build();

            var result = await DoPut(METHOD, request, _token);

            Assert.Equal(StatusCodes.Status204NoContent, (int)result.StatusCode);

        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Name_Empty(string culture)
        {
            var user = UserBuilder.Build();
            var request = RequestUpdateProfileUserJsonBuilder.Build();
            request.Name = string.Empty;
            var result = await DoPut(METHOD, request, _token, culture);
            var content = await result.Content.ReadAsStringAsync();

            var body = await result.Content.ReadAsStreamAsync();
            var response = await JsonDocument.ParseAsync(body);

            var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray().ToList();
            Assert.Equal(StatusCodes.Status400BadRequest, (int)result.StatusCode);
            Assert.Single(errors);          
            var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("NAME_EMPTY", new System.Globalization.CultureInfo(culture));
            Assert.Equal(expectedMessage, errors[0].GetString());
        }


        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Email_Invalid(string culture)
        {
            var user = UserBuilder.Build();
            var request = RequestUpdateProfileUserJsonBuilder.Build();
            request.Email = "invalid-email-format";
            var result = await DoPut(METHOD, request, _token,culture);
            var content = await result.Content.ReadAsStringAsync();

            var body = await result.Content.ReadAsStreamAsync();
            var response = await JsonDocument.ParseAsync(body);
            var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray().ToList();
            Assert.Equal(StatusCodes.Status400BadRequest, (int)result.StatusCode);
            Assert.Single(errors);

            var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("EMAIL_INVALID", new System.Globalization.CultureInfo(culture));
            Assert.Equal(expectedMessage, errors[0].GetString());


        }


    }
}
