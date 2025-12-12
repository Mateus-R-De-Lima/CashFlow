using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace WebApi.Test.Expense.GetAll
{
    public class ExpenseGetAllUserTest : CashFlowClassFixture
    {
        private const string METHOD = "api/Expenses";
        private readonly string _token;
        public ExpenseGetAllUserTest(CustomWebApplicationFactory customWebApplicationFactory) : base(customWebApplicationFactory)
        {
            _token = customWebApplicationFactory.User_Team_Member.GetToken();
        }


        [Fact]
        public async Task Success()
        {
            var result = await DoGet(requestUri: METHOD, token: _token);

            Assert.Equal(StatusCodes.Status200OK, (int)result.StatusCode);

            var body = await result.Content.ReadAsStreamAsync();
            var response = await JsonDocument.ParseAsync(body);

            // Garante que existe o array "expenses"
            Assert.True(response.RootElement.TryGetProperty("expenses", out var expensesElement),
                "A propriedade 'expenses' deve existir.");

            // Garante que é um array
            Assert.Equal(JsonValueKind.Array, expensesElement.ValueKind);

            // Deve conter ao menos 1 item
            Assert.True(expensesElement.GetArrayLength() > 0,
                "A lista de despesas não pode estar vazia.");

            // Valida cada item da lista
            foreach (var item in expensesElement.EnumerateArray())
            {
                Assert.True(item.TryGetProperty("id", out var idProp));
                Assert.True(idProp.GetInt64() > 0);

                Assert.True(item.TryGetProperty("title", out var titleProp));
                Assert.False(string.IsNullOrWhiteSpace(titleProp.GetString()));

                Assert.True(item.TryGetProperty("amount", out var amountProp));
                Assert.True(amountProp.GetDecimal() >= 0);
            }
        }

    }
}
