using CashFlow.Application.UseCases.Reports.PDF.Fonts;
using CashFlow.Domain.Repositories.Expenses;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
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

            var document = CreateDocument(month);

            var page = CreatePage(document);

            var paragraph = page.AddParagraph();

            var title = string.Format(ResourceReportGenerationMessages.TOTAL_SPENT_IN, month.ToString("Y"));
            paragraph.AddFormattedText(title, new Font { Size = 15,Name =FontHelper.RALEWAY_REGULAR});

            paragraph.AddLineBreak(); // Quebra de Linha

            var totalExpenses = expenses.Sum(expenses => expenses.Amount);

            paragraph.AddFormattedText($"{CURRENCY_SYMBOL} {totalExpenses}", new Font { Size = 50, Name = FontHelper.WORKSANS_BLACK });

            return RenderDocument(document);

        }


        private Document CreateDocument(DateOnly month)
        {
            var document = new Document();
            document.Info.Title = $"{ResourceReportGenerationMessages.EXPENSES_FOR} {month:Y}";
            document.Info.Author = "Mateus Lima";

            var style = document.Styles["Normal"];
            style!.Font.Name = FontHelper.RALEWAY_REGULAR;

            return document;
        }


        private Section CreatePage(Document document)
        {
            var section =  document.AddSection();
            // Clone da Paginal Padrão 
            section.PageSetup = document.DefaultPageSetup.Clone();

            // Margin de Layout 
            section.PageSetup.BottomMargin = 80;
            section.PageSetup.TopMargin = 80;
            section.PageSetup.LeftMargin = 40;
            section.PageSetup.RightMargin = 40;


            return section;
        }


        private byte[] RenderDocument(Document document)
        {
            var renderer = new PdfDocumentRenderer{ Document = document,};

            renderer.RenderDocument();

            using var file = new MemoryStream();

            renderer.PdfDocument.Save(file);

            return file.ToArray();

        }
    }
}
