using Microsoft.AspNetCore.Http;

namespace WebApi.Test.Users.Delete
{
    public class DeleteUserTest : CashFlowClassFixture
    {
        private readonly string METHOD = "api/User";
        private readonly string _token;
        public DeleteUserTest(CustomWebApplicationFactory customWebApplicationFactory) : base(customWebApplicationFactory)
        {
            _token = customWebApplicationFactory.User_Team_Admin.GetToken();
        }
        [Fact]
        public async Task Success()
        {
            var result = await DoDelete(METHOD, _token);
            Assert.Equal(StatusCodes.Status204NoContent, (int)result.StatusCode);
        }
    }
    
}
