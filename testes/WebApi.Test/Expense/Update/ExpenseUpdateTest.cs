using CashFlow.Communication.Requests;
using CashFlow.Domain.Entities;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using CommonTestUtilities.Request;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Expense.Update
{
    public class ExpenseUpdateTest : CashFlowClassFixture
    {
        private const string METHOD = "api/Expenses";
        private readonly string _token;
        private readonly long _expenseId;

        public ExpenseUpdateTest(CustomWebApplicationFactory customWebApplicationFactory) : base(customWebApplicationFactory)
        {
            _token = customWebApplicationFactory.User_Team_Member.GetToken();
            _expenseId = customWebApplicationFactory.Expense.GetExpenseId();
        }

        [Fact]
        public async Task Success()
        {
            var request = RequestRegisterExpenseJsonBuilder.Build();

            var result = await DoPut(requestUri: $"{METHOD}/{_expenseId}", request, _token);

            Assert.Equal(StatusCodes.Status204NoContent, (int)result.StatusCode);
        }


        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Expense_Not_Found(string culture)
        {
            var request = RequestRegisterExpenseJsonBuilder.Build();
            var result = await DoPut(requestUri: $"{METHOD}/1000", request, _token,culture);

            var content = await result.Content.ReadAsStringAsync();

            var body = await result.Content.ReadAsStreamAsync();
            var response = await JsonDocument.ParseAsync(body);

            var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray().ToList();
            Assert.Equal(StatusCodes.Status404NotFound, (int)result.StatusCode);
            Assert.Single(errors);            
            var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("EXPENSE_NOT_FOUND", new System.Globalization.CultureInfo(culture));
            Assert.Equal(expectedMessage, errors[0].GetString());
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Title_Empty(string culture)
        {
            var request = RequestRegisterExpenseJsonBuilder.Build();

            request.Title = string.Empty;

            var result = await DoPut(requestUri: $"{METHOD}/{_expenseId}", request, _token, culture);

            var content = await result.Content.ReadAsStringAsync();           

            var body = await result.Content.ReadAsStreamAsync();
            var response = await JsonDocument.ParseAsync(body);

            var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray().ToList();
            Assert.Equal(StatusCodes.Status400BadRequest,(int)result.StatusCode);
            Assert.Single(errors);           
            var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("TITLE_REQUIRED", new System.Globalization.CultureInfo(culture));
            Assert.Equal(expectedMessage, errors[0].GetString());



        }


    }
}
