
namespace CashFlow.Application.UseCases.Reports.PDF
{
    public interface IGenerateExpensesReportPDFUseCase
    {
        Task<byte[]> Execute(DateOnly month);
    }
}