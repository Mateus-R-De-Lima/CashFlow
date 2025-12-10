using Bogus.Bson;
using CashFlow.Exception;
using CommonTestUtilities.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Users.Register
{
    public class RegisterUserTest : CashFlowClassFixture
    {
        private const string METHOD = "api/User";       

        public RegisterUserTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory) { }
      

        [Fact]
        public async Task Success()
        {
            var request = RequestRegisterUserJsonBuilder.Build();

            var result = await DoPost(METHOD, request);

            var body = await result.Content.ReadAsStreamAsync();

            var response = await JsonDocument.ParseAsync(body);


            Assert.Equal(StatusCodes.Status201Created, (int)result.StatusCode);

            Assert.NotEmpty(response.RootElement.GetProperty("token")
                .GetString());

            Assert.Equal(request.Name, response.RootElement.GetProperty("name").GetString());

        }



        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Empty_Name(string cultureInfo)
        {
            var request = RequestRegisterUserJsonBuilder.Build();
            request.Name = string.Empty;
           

            var result = await DoPost(requestUri: METHOD,
                                      request: request,
                                      cultureInfo: cultureInfo);

            var body = await result.Content.ReadAsStreamAsync();

            var response = await JsonDocument.ParseAsync(body);
            var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray().ToList();
           
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Single(errors);
            var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("NAME_EMPTY", new CultureInfo(cultureInfo));
            Assert.Equal(expectedMessage, errors[0].GetString());

        }


    


    }
}
