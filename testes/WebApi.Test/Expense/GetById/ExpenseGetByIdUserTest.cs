using Bogus.Bson;
using CashFlow.Communication.Enums;
using CashFlow.Exception;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace WebApi.Test.Expense.GetById
{
    public class ExpenseGetByIdUserTest : CashFlowClassFixture
    {
        private const string METHOD = "api/Expenses";
        private readonly string _token;
        private readonly long _expenseId;
        public ExpenseGetByIdUserTest(CustomWebApplicationFactory customWebApplicationFactory) : base(customWebApplicationFactory)
        {
            _token = customWebApplicationFactory.User_Team_Member.GetToken();
            _expenseId = customWebApplicationFactory.Expense.GetExpenseId();
        }


        [Fact]
        public async Task Success()
        {
            var result = await DoGet(requestUri: $"{METHOD}/{_expenseId}", token: _token);

            Assert.Equal(StatusCodes.Status200OK, (int)result.StatusCode);

            var body = await result.Content.ReadAsStreamAsync();
            var response = await JsonDocument.ParseAsync(body);

            var root = response.RootElement;
            // --- TITLE ---
            Assert.True(root.TryGetProperty("title", out var titleProp));
            Assert.False(string.IsNullOrWhiteSpace(titleProp.GetString()));


            // --- DATE ---
            Assert.True(root.TryGetProperty("date", out var dateProp));
            var date = dateProp.GetDateTime();
            Assert.NotEqual(default, date);

            // --- PAYMENT TYPE ---
            Assert.True(root.TryGetProperty("paymentType", out var paymentProp));
            var paymentValue = paymentProp.GetInt32();
            Assert.True(Enum.IsDefined(typeof(PaymentTypes), paymentValue));

            // --- AMOUNT ---
            Assert.True(root.TryGetProperty("amount", out var amountProp));
            Assert.True(amountProp.GetDecimal() >= 0);

        }
    }
}
