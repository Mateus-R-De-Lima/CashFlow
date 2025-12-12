using CashFlow.Communication.Requests;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace WebApi.Test.Users.Login.DoLogin
{
    public class DoLoginTest : IClassFixture<CustomWebApplicationFactory>
    {
        private const string METHOD = "api/Login";
        private readonly HttpClient _httpClient;

        private readonly string _email;
        private readonly string _name;
        private readonly string _password;

        public DoLoginTest(CustomWebApplicationFactory webApplicationFactory)
        {
            _httpClient = webApplicationFactory.CreateClient();

            _email = webApplicationFactory.User_Team_Member.GetEmail();
            _name = webApplicationFactory.User_Team_Member.GetName();
            _password = webApplicationFactory.User_Team_Member.GetPassword();   
        }

        [Fact]
        public async Task Success()
        {
            var request = new RequestLoginJson
            {
                Email = _email,
                Password = _password
            };

            var response = await _httpClient.PostAsJsonAsync(METHOD, request);

            var body = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(body);


            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            Assert.NotEmpty(responseData.RootElement.GetProperty("token")
                .GetString());

            Assert.Equal(_name, responseData.RootElement.GetProperty("name").GetString());



        }
    }
}
