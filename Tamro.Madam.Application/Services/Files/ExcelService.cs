using ClosedXML.Excel;

namespace Tamro.Madam.Application.Services.Files;

public class ExcelService : IExcelService
{
    public async Task<byte[]> ExportAsync<TData>(IEnumerable<TData> data, Dictionary<string, Func<TData, object?>> mappers, string sheetName)
    {
        using var workbook = new XLWorkbook();
        workbook.Properties.Author = "";
        var ws = workbook.Worksheets.Add(sheetName);
        var colIndex = 1;
        var rowIndex = 1;
        var headers = mappers.Keys.Select(x => x).ToList();
        foreach (var header in headers)
        {
            var cell = ws.Cell(rowIndex, colIndex);
            var fill = cell.Style.Fill;
            fill.PatternType = XLFillPatternValues.Solid;
            fill.SetBackgroundColor(XLColor.LightBlue);
            var border = cell.Style.Border;
            border.BottomBorder =
                border.BottomBorder =
                    border.BottomBorder =
                        border.BottomBorder = XLBorderStyleValues.Thin;

            cell.Value = header;

            colIndex++;
        }
        var dataList = data.ToList();
        foreach (var item in dataList)
        {
            colIndex = 1;
            rowIndex++;

            var result = headers.Select(header => mappers[header](item));

            foreach (var value in result)
            {
                ws.Cell(rowIndex, colIndex++).Value = value == null ? Blank.Value : value.ToString();
            }
        }
        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        stream.Seek(0, SeekOrigin.Begin);
        return await Task.FromResult(stream.ToArray());
    }
}
