using CashFlow.Application.UseCases.Reports.PDF.Fonts;
using CashFlow.Domain.Repositories.Expenses;
using PdfSharp.Fonts;

namespace CashFlow.Application.UseCases.Reports.PDF
{
    public class GenerateExpensesReportPDFUseCase : IGenerateExpensesReportPDFUseCase
    {
        private readonly IExpensesReadOnlyRepository _repository;
        public GenerateExpensesReportPDFUseCase(IExpensesReadOnlyRepository repository)
        {
            _repository = repository;
            GlobalFontSettings.FontResolver = new ExpensesReportFontResolver();
        }

        private const string CURRENCY_SYMBOL = "R$";

        public async Task<byte[]> Execute(DateOnly month)
        {
            var expenses = await _repository.FilterByMonth(month);
            if (expenses.Count == 0)
                return [];

            return [];

        }
    }
}
