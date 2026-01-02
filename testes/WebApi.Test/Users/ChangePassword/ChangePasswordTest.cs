using CashFlow.Communication.Requests;
using CashFlow.Exception;
using CommonTestUtilities.Request;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Users.ChangePassword
{
    public class ChangePasswordTest : CashFlowClassFixture
    {
        private const string METHOD = "api/user/change-password";
        private readonly string _token;
        private readonly string _password;
        private readonly string _email;
        public ChangePasswordTest(CustomWebApplicationFactory customWebApplicationFactory) : base(customWebApplicationFactory)
        {
            _email = customWebApplicationFactory.User_Team_Member.GetEmail();
            _password = customWebApplicationFactory.User_Team_Member.GetPassword();
            _token = customWebApplicationFactory.User_Team_Member.GetToken();
        }

        [Fact]
        public async Task Success()
        {
            var request = RequestChangePasswordJsonBuilder.Build();
            request.Password = _password;
            var response = await DoPut(METHOD, request, _token);
            Assert.Equal(StatusCodes.Status204NoContent, (int)response.StatusCode);

            var looginRequest = new RequestLoginJson
            {
                Email = _email,
                Password = _password
            };

            response = await DoPost("api/login", looginRequest);
            Assert.Equal(StatusCodes.Status401Unauthorized, (int)response.StatusCode);

            looginRequest.Password = request.NewPassword;

            response = await DoPost("api/login", looginRequest);
            Assert.Equal(StatusCodes.Status200OK, (int)response.StatusCode);


        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]

        public async Task Task_Password_Different_Current_Password(string culture)
        {
            var request = RequestChangePasswordJsonBuilder.Build();

            var result = await DoPut(METHOD, request, _token, culture);
            var content = await result.Content.ReadAsStringAsync();

            var body = await result.Content.ReadAsStreamAsync();
            var response = await JsonDocument.ParseAsync(body);
            var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray().ToList();
            Assert.Equal(StatusCodes.Status400BadRequest, (int)result.StatusCode);
            Assert.Single(errors);           
            var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("PASSWORD_DIFFERENT_CURRENT_PASSWORD", new CultureInfo(culture));
            Assert.Equal(expectedMessage, errors[0].GetString());


        }



    }
}
