using CashFlow.Application.Services.LoggerUser;
using CashFlow.Application.UseCases.Reports.PDF.Colors;
using CashFlow.Application.UseCases.Reports.PDF.Fonts;
using CashFlow.Domain.Repositories.Expenses;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using PdfSharp.Fonts;
using System.Reflection;

namespace CashFlow.Application.UseCases.Reports.PDF
{
    public class GenerateExpensesReportPDFUseCase : IGenerateExpensesReportPDFUseCase
    {
        private readonly IExpensesReadOnlyRepository _repository;
        private readonly ILoggerUser _loggerUser;
        private const int HEIGHT_ROW_EXPENSE_TABLE = 25;
        public GenerateExpensesReportPDFUseCase(IExpensesReadOnlyRepository repository, ILoggerUser loggerUser)
        {
            _repository = repository;
            _loggerUser = loggerUser;
            GlobalFontSettings.FontResolver = new ExpensesReportFontResolver();
        }

        private const string CURRENCY_SYMBOL = "R$";

        public async Task<byte[]> Execute(DateOnly month)
        {
            var user = await _loggerUser.Get();
            var expenses = await _repository.FilterByMonth(user,month);
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


                var row = table.AddRow();
                row.Height = 25;

                row.Cells[0].AddParagraph(expense.Title);
                row.Cells[0].Format.Font = new Font { Name = FontHelper.RALEWAY_BLACK, Size = 14, Color = ColorsHelper.BLACK };
                row.Cells[0].Shading.Color = ColorsHelper.RED_LIGHT;
                row.Cells[0].VerticalAlignment = VerticalAlignment.Center;
                row.Cells[0].MergeRight = 2;
                row.Cells[0].Format.LeftIndent = 20;

                row.Cells[3].AddParagraph(ResourceReportGenerationMessages.AMOUNT);
                row.Cells[3].Format.Font = new Font { Name = FontHelper.RALEWAY_BLACK, Size = 14, Color = ColorsHelper.WHITE };
                row.Cells[3].Shading.Color = ColorsHelper.RED_DARK;
                row.Cells[3].VerticalAlignment = VerticalAlignment.Center;

                row = table.AddRow();
                row.Height = 25;

                row.Cells[0].AddParagraph(expense.Date.ToString("D"));
                row.Cells[0].Format.Font = new Font { Name = FontHelper.WORKSANS_REGULAR, Size = 12, Color = ColorsHelper.BLACK };
                row.Cells[0].Shading.Color = ColorsHelper.GREEN_DARK;
                row.Cells[0].VerticalAlignment = VerticalAlignment.Center;
                row.Cells[0].Format.LeftIndent = 20;

                row.Cells[1].AddParagraph(expense.Date.ToString("t"));
                row.Cells[1].Format.Font = new Font { Name = FontHelper.WORKSANS_REGULAR, Size = 12, Color = ColorsHelper.BLACK };
                row.Cells[1].Shading.Color = ColorsHelper.GREEN_DARK;
                row.Cells[1].VerticalAlignment = VerticalAlignment.Center;

                row.Cells[3].AddParagraph($"-{expense.Amount} {CURRENCY_SYMBOL}");
                row.Cells[3].Format.Font = new Font { Name = FontHelper.WORKSANS_REGULAR, Size = 14, Color = ColorsHelper.BLACK };
                row.Cells[3].Shading.Color = ColorsHelper.WHITE;
                row.Cells[3].VerticalAlignment = VerticalAlignment.Center;

                if (string.IsNullOrWhiteSpace(expense.Description) == false)
                {
                    var descriptionRow = table.AddRow();
                    descriptionRow.Height = HEIGHT_ROW_EXPENSE_TABLE;

                    descriptionRow.Cells[0].AddParagraph(expense.Description);
                    descriptionRow.Cells[0].Format.Font = new Font { Name = FontHelper.WORKSANS_REGULAR, Size = 10, Color = ColorsHelper.BLACK };
                    descriptionRow.Cells[0].Shading.Color = ColorsHelper.GREEN_LIGHT;
                    descriptionRow.Cells[0].VerticalAlignment = VerticalAlignment.Center;
                    descriptionRow.Cells[0].MergeRight = 2;
                    descriptionRow.Cells[0].Format.LeftIndent = 20;

                    row.Cells[3].MergeDown = 1;
                }

                row = table.AddRow();
                row.Height = 30;
                row.Borders.Visible = false;
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
