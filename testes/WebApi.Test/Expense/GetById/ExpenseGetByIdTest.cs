using Bogus.Bson;
using CashFlow.Communication.Enums;
using CashFlow.Exception;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Expense.GetById
{
    public class ExpenseGetByIdTest : CashFlowClassFixture
    {
        private const string METHOD = "api/Expenses";
        private readonly string _token;
        private readonly long _expenseId;
        public ExpenseGetByIdTest(CustomWebApplicationFactory customWebApplicationFactory) : base(customWebApplicationFactory)
        {
            _token = customWebApplicationFactory.User_Team_Member.GetToken();
            _expenseId = customWebApplicationFactory.Expense_Team_Member.GetExpenseId();
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

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Expense_Not_Found(string culture)
        {
            var result = await DoGet(requestUri: $"{METHOD}/1000", token: _token, culture);

            var body = await result.Content.ReadAsStreamAsync();

            var response = await JsonDocument.ParseAsync(body);

            var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray().ToList();

            Assert.NotNull(result);
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
            Assert.Single(errors);
            
            var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("EXPENSE_NOT_FOUND", new CultureInfo(culture));
            Assert.Equal(expectedMessage, errors[0].GetString());

        }
    }
}
