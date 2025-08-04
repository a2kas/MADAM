namespace Tamro.Madam.Application.Models.Common;

public class PaginationFilter
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 15;
    public string OrderBy { get; set; } = "Id";
    public string SortDirection { get; set; } = "Descending";
    public override string ToString() => $"PageNumber:{PageNumber},PageSize:{PageSize},OrderBy:{OrderBy},SortDirection:{SortDirection}";
}
