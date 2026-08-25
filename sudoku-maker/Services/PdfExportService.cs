using System.Collections.Generic;
using System.Linq;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using sudoku_maker.ViewModels;

namespace sudoku_maker.Services;

public class PdfExportService
{
    public void ExportToPdf(IEnumerable<SudokuCellViewModel> cells, string filePath, string title)
    {
        QuestPDF.Settings.License = LicenseType.Community;

        var cellList = cells.ToList();

        Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(30);
                page.DefaultTextStyle(x => x.FontSize(12));

                page.Content().Column(column =>
                {
                    column.Item().Text(title).FontSize(20).Bold().AlignCenter();
                    column.Item().PaddingTop(20);

                    column.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            for (int i = 0; i < 9; i++)
                            {
                                columns.RelativeColumn();
                            }
                        });

                        for (int row = 0; row < 9; row++)
                        {
                            for (int col = 0; col < 9; col++)
                            {
                                var cell = cellList.First(c => c.Row == row && c.Column == col);
                                string text = cell.IsGiven ? cell.SolutionValue.ToString() : string.Empty;

                                float left = col == 0 ? 2f : 1f;
                                float top = row == 0 ? 2f : 1f;
                                float right = (col + 1) % 3 == 0 ? 3f : 1f;
                                float bottom = (row + 1) % 3 == 0 ? 3f : 1f;

                                table.Cell()
                                    .BorderLeft(left).BorderTop(top).BorderRight(right).BorderBottom(bottom)
                                    .BorderColor(Colors.Grey.Darken4)
                                    .Height(45)
                                    .AlignCenter().AlignMiddle()
                                    .Text(text).FontSize(18).Bold();
                            }
                        }
                    });
                });
            });
        })
        .GeneratePdf(filePath);
    }
}