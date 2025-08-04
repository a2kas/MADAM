namespace Tamro.Madam.Application.Services.Files;

public interface IExcelService
{
    Task<byte[]> ExportAsync<TData>(IEnumerable<TData> data, Dictionary<string, Func<TData, object?>> mappers, string sheetName);
}
