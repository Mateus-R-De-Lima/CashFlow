using Microsoft.AspNetCore.Http;

namespace WebApi.Test.Expense.Reports
{
    public class GenerateExpensesReportsTest : CashFlowClassFixture
    {
        private const string METHOD = "api/Report";
        private readonly string _adminToken;
        private readonly string _teamMemberToken;
        private readonly DateTime _expenseDate;


        public GenerateExpensesReportsTest(CustomWebApplicationFactory customWebApplicationFactory) : base(customWebApplicationFactory)
        {
            _adminToken = customWebApplicationFactory.User_Team_Admin.GetToken();
            _teamMemberToken = customWebApplicationFactory.User_Team_Member.GetToken();
            _expenseDate = customWebApplicationFactory.Expense_Admin.GetDate();
        }


        [Fact]

        public async Task Success_Pdf()
        {
         
            var result = await DoGet(requestUri: $"{METHOD}/pdf?month={_expenseDate.ToString("MM/yyyy")}", _adminToken);

         
            Assert.NotNull(result.Content.Headers.ContentType);
            Assert.Equal(StatusCodes.Status200OK, (int)result.StatusCode);
            Assert.Equal("application/pdf", result.Content.Headers.ContentType?.MediaType);

        }
        [Fact]
        public async Task Success_Excel()
        {
            var result = await DoGet(requestUri: $"{METHOD}/excel?month={_expenseDate.ToString("MM/yyyy")}", _adminToken);

          
            Assert.NotNull(result.Content.Headers.ContentType);
            Assert.Equal(StatusCodes.Status200OK, (int)result.StatusCode);
            Assert.Equal("application/octet-stream", result.Content.Headers.ContentType?.MediaType);

        }

        [Fact]
        private async Task Error_Forbidden_User_Not_Allowed_Pdf()
        {
            var result = await DoGet(requestUri: $"{METHOD}/pdf?month={_expenseDate:Y}", _teamMemberToken);
            Assert.Equal(StatusCodes.Status403Forbidden, (int)result.StatusCode);
        }


        [Fact]
        private async Task Error_Forbidden_User_Not_Allowed_Excel()
        {
            var result = await DoGet(requestUri: $"{METHOD}/excel?month={_expenseDate:Y}", _teamMemberToken);
            Assert.Equal(StatusCodes.Status403Forbidden, (int)result.StatusCode);
        }
    }
}
