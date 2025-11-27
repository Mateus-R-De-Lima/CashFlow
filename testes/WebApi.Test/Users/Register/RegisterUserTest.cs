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

namespace WebApi.Test.Users.Register
{
    public class RegisterUserTest : IClassFixture<CustomWebApplicationFactory>
    {
        private const string METHOD = "api/User";
        private readonly HttpClient _httpClient;

        public RegisterUserTest(CustomWebApplicationFactory webApplicationFactory)
        {
            _httpClient = webApplicationFactory.CreateClient();
        }


        [Fact]
        public async Task Success()
        {
            var request = RequestRegisterUserJsonBuilder.Build();

            var result = await _httpClient.PostAsJsonAsync(METHOD, request);

            var body = await result.Content.ReadAsStreamAsync();

            var response = await JsonDocument.ParseAsync(body);


            Assert.Equal(StatusCodes.Status201Created, (int)result.StatusCode);

            Assert.NotEmpty(response.RootElement.GetProperty("token")
                .GetString());

            Assert.Equal(request.Name, response.RootElement.GetProperty("name").GetString());

        }



        [Fact]
        public async Task Error_Empty_Name()
        {
            var request = RequestRegisterUserJsonBuilder.Build();
            request.Name = string.Empty;

            var language = CultureInfo.CurrentCulture.Name;
            _httpClient.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue(language));

            var result = await _httpClient.PostAsJsonAsync(METHOD, request);

            var body = await result.Content.ReadAsStreamAsync();

            var response = await JsonDocument.ParseAsync(body);
            var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray().ToList();
           
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Single(errors);
            Assert.Equal(ResourceErrorMessages.NAME_EMPTY, errors[0].GetString());

        }


        [Fact]
        public async Task Error_Empty_Name_FR()
        {
            var request = RequestRegisterUserJsonBuilder.Build();
            request.Name = string.Empty;
            
            _httpClient.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("fr"));

            var result = await _httpClient.PostAsJsonAsync(METHOD, request);

            var body = await result.Content.ReadAsStreamAsync();

            var response = await JsonDocument.ParseAsync(body);
            var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray().ToList();

            var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("NAME_EMPTY", new CultureInfo("fr"));

            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Single(errors);
            Assert.Equal(expectedMessage, errors[0].GetString());

        }


    }
}
