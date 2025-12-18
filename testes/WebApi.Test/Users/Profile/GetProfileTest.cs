using System.Net;
using System.Text.Json;

namespace WebApi.Test.Users.Profile
{
    public class GetProfileTest : CashFlowClassFixture
    {
        private readonly string METHOD = "api/User";

        private readonly string _token;
        private readonly string _userName;
        private readonly string _userEmail;
        public GetProfileTest(CustomWebApplicationFactory customWebApplicationFactory) : base(customWebApplicationFactory)
        {
            _token = customWebApplicationFactory.User_Team_Admin.GetToken();
            _userEmail = customWebApplicationFactory.User_Team_Admin.GetEmail();
            _userName = customWebApplicationFactory.User_Team_Admin.GetName();
        }

        [Fact]
        public async Task Success()
        {
            // Arrange

            var response = await DoGet(METHOD, _token);
            var body = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(body);


            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            Assert.Equal(_userName, responseData.RootElement.GetProperty("name").GetString());
            Assert.Equal(_userEmail, responseData.RootElement.GetProperty("email").GetString());


        }
    }
}
