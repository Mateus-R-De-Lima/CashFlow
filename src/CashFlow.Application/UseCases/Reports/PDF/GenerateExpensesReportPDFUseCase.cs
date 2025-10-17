using CashFlow.Domain.Repositories.Expenses;

namespace CashFlow.Application.UseCases.Reports.PDF
{
    public class GenerateExpensesReportPDFUseCase(
         IExpensesReadOnlyRepository repository
        ) : IGenerateExpensesReportPDFUseCase
    {

        private const string CURRENCY_SYMBOL = "R$";

        public async Task<byte[]> Execute(DateOnly month)
        {
            var expenses = await repository.FilterByMonth(month);
            if (expenses.Count == 0)
                return [];

            return [];

        }
    }
}
