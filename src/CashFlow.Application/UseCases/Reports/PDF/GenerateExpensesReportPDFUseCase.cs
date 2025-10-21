using CashFlow.Application.UseCases.Reports.PDF.Fonts;
using CashFlow.Domain.Repositories.Expenses;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using PdfSharp.Fonts;
using MigraDoc.DocumentObjectModel.Tables;
using System.Reflection;

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
            CreateHeaderWithProfilePhotoAndName(page);

            var totalExpenses = expenses.Sum(expense => expense.Amount);
            CreateTotalSpentSection(page, month, totalExpenses);


            foreach (var expense in expenses)
            {
                var table = CreateExpenseTable(page);
            }

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

        private void CreateHeaderWithProfilePhotoAndName(Section page)
        {
            var table = page.AddTable();
            table.AddColumn("210");
            table.AddColumn("300");

            var row = table.AddRow();

            var assembly = Assembly.GetExecutingAssembly();
            var directoryName = Path.GetDirectoryName(assembly.Location);
            var pathFile = Path.Combine(directoryName!, "Logo", "foto.png");

            row.Cells[0].AddImage(pathFile);

            row.Cells[1].AddParagraph("Hey, Mateus Lima");
            row.Cells[1].Format.Font = new Font { Name = FontHelper.RALEWAY_BLACK, Size = 16 };
            row.Cells[1].VerticalAlignment = MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center;
        }

        private void CreateTotalSpentSection(Section page, DateOnly month, decimal totalExpenses)
        {
            var paragraph = page.AddParagraph();
            paragraph.Format.SpaceBefore = "40";
            paragraph.Format.SpaceAfter = "40";

            var title = string.Format(ResourceReportGenerationMessages.TOTAL_SPENT_IN, month.ToString("Y"));

            paragraph.AddFormattedText(title, new Font { Name = FontHelper.RALEWAY_REGULAR, Size = 15 });

            paragraph.AddLineBreak();

            paragraph.AddFormattedText($"{CURRENCY_SYMBOL} {totalExpenses}", new Font { Name = FontHelper.WORKSANS_BLACK, Size = 50 });
        }

        private Table CreateExpenseTable(Section page)
        {
            var table = page.AddTable();

            table.AddColumn("195").Format.Alignment = ParagraphAlignment.Left;
            table.AddColumn("80").Format.Alignment = ParagraphAlignment.Center;
            table.AddColumn("120").Format.Alignment = ParagraphAlignment.Center;
            table.AddColumn("120").Format.Alignment = ParagraphAlignment.Right;

            return table;
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
